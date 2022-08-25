#include <Windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <json-c/json.h>
#include <string>
#include <vector>
#include <algorithm>

#pragma warning(disable : 4996)

typedef struct TASK_INFO
{
	json_object* obj;
	const char* pTarget;
	const char* pOperation;
	const char* pNASName;
	const char* pNASPath;
	const char* pNASUserName;
	const char* pNASPassword;
	const char* pLastDefaultGuid;
	const char* pOSLoaderGuid;
	const char* pDeviceGuid;
	const char* pBackupFileType;
	const char* pIsLocalBackup;
	const char* pExtraGhoArgument;
	const char* pExtraWimArgument;
	const char* pLocalPath;
	const char* pLocalDevice;

} TASK_INFO;


//通过物理路径获取分区的硬盘号和分区号，调用CreateFile获取句柄，DeviceIoiControl获取硬盘号和分区号
bool CALLBACK GetPartitionLocationA(IN LPCSTR pWin32NamespaceName, OUT LPUINT pDiskNumber, OUT LPUINT pPartitionNumber)
{
	bool bResult = false;
	HANDLE hPartition = CreateFileA(pWin32NamespaceName, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
	if (hPartition != INVALID_HANDLE_VALUE)
	{
		STORAGE_DEVICE_NUMBER info = {};
		DWORD dwReceivedBytes = 0;
		BOOL bCallResult = DeviceIoControl(hPartition, IOCTL_STORAGE_GET_DEVICE_NUMBER, NULL, 0, &info, sizeof info, &dwReceivedBytes, NULL);
		if (bCallResult)
		{
			*pDiskNumber = info.DeviceNumber;
			*pPartitionNumber = info.PartitionNumber;
			bResult = true;
		}
		CloseHandle(hPartition);
	}
	return bResult;
}

//获取主模块路径
std::string GetApplicationPath()
{
	std::string result;
	size_t uResultLength = 0;
	DWORD uBufferSize = 32768 * sizeof(char);
	char* pBuffer = (char*)malloc(uBufferSize);
	if (pBuffer != NULL)
	{
		int length = GetModuleFileNameA(NULL, pBuffer, uBufferSize / sizeof(char));
		if (GetLastError() == ERROR_SUCCESS)
		{
			int i = 0;
			for (i = length - 1; i > 0; --i)
			{
				if (pBuffer[i] == '\\')
				{
					pBuffer[i] = 0;
					break;
				}
			}
			result.append(pBuffer);
		}
		free(pBuffer);
	}
	return result;
}
//读取配置
bool ReadConfig(TASK_INFO* pTaskInfo)
{
	int result = 0;
	std::string configPath = GetApplicationPath() + "\\config.json";
	json_object* obj = json_object_from_file(configPath.c_str());
	if (obj != NULL)
	{
		pTaskInfo->pTarget = json_object_get_string(json_object_object_get(obj, "Target"));
		pTaskInfo->pDeviceGuid = json_object_get_string(json_object_object_get(obj, "DeviceGuid"));
		pTaskInfo->pOSLoaderGuid = json_object_get_string(json_object_object_get(obj, "OSLoaderGuid"));
		pTaskInfo->pLastDefaultGuid = json_object_get_string(json_object_object_get(obj, "LastDefaultGuid"));
		pTaskInfo->pNASName = json_object_get_string(json_object_object_get(obj, "NASName"));
		pTaskInfo->pNASPath = json_object_get_string(json_object_object_get(obj, "NASPath"));
		pTaskInfo->pNASPassword = json_object_get_string(json_object_object_get(obj, "NASPassword"));
		pTaskInfo->pNASUserName = json_object_get_string(json_object_object_get(obj, "NASUser"));
		pTaskInfo->pOperation = json_object_get_string(json_object_object_get(obj, "Operation"));

		pTaskInfo->pBackupFileType = json_object_get_string(json_object_object_get(obj, "BackupFileType"));
		pTaskInfo->pIsLocalBackup = json_object_get_string(json_object_object_get(obj, "IsLocalBackup"));
		pTaskInfo->pExtraGhoArgument = json_object_get_string(json_object_object_get(obj, "ExtraGhoArgument"));
		pTaskInfo->pExtraWimArgument = json_object_get_string(json_object_object_get(obj, "ExtraWimArgument"));
		pTaskInfo->pLocalPath = json_object_get_string(json_object_object_get(obj, "LocalPath"));
		pTaskInfo->pLocalDevice = json_object_get_string(json_object_object_get(obj, "LocalDevice"));

		if (pTaskInfo->pTarget == NULL || pTaskInfo->pDeviceGuid == NULL ||
			pTaskInfo->pLastDefaultGuid == NULL || pTaskInfo->pNASName == NULL ||
			pTaskInfo->pNASPassword == NULL || pTaskInfo->pNASPath == NULL ||
			pTaskInfo->pNASUserName == NULL || pTaskInfo->pOperation == NULL ||
			pTaskInfo->pOSLoaderGuid == NULL || pTaskInfo->pBackupFileType == NULL ||
			pTaskInfo->pIsLocalBackup == NULL || pTaskInfo->pExtraGhoArgument == NULL ||
			pTaskInfo->pExtraWimArgument == NULL || pTaskInfo->pLocalPath == NULL || 
			pTaskInfo->pLocalDevice == NULL)
			return false;
		else
			return true;
	}
	return false;
}

//通过硬盘号和分区号查询盘符
std::string QueryMountPointByNumberA(UINT diskNumber, UINT partitionNumber)
{
	std::string result = "";
	DWORD uBufferSize = 32768 * sizeof(char);
	char* pBuffer = (char*)malloc(uBufferSize);
	if (pBuffer != NULL)
	{
		int length = GetLogicalDriveStringsA((uBufferSize / sizeof(char) - 1), pBuffer);
		if (GetLastError() == ERROR_SUCCESS)
		{
			int index = -1;
			for (size_t i = 0; i < length; i++)
			{
				if (!pBuffer[i])
				{
					std::string temp = "\\\\.\\";
					//temp = pBuffer[index + 1] // char* pBuffer
					temp += &pBuffer[index + 1];
					temp.pop_back();
					UINT dm = -1, pm = -1;
					GetPartitionLocationA(temp.c_str(), &dm, &pm);
					if (dm == diskNumber && pm == partitionNumber)
					{
						result = &pBuffer[index + 1];
						break;
					}
					index = (int)i;
				}
			}
		}
		free(pBuffer);
	}
	return result;
}

//通过配置生成要执行的任务命令
std::vector<std::string> GenTaskCommandLines(TASK_INFO* pTaskInfo)
{
	bool isLocalBackup = false;
	std::string isLocalBackupStr = pTaskInfo->pIsLocalBackup;
	std::transform(isLocalBackupStr.begin(), isLocalBackupStr.end(), isLocalBackupStr.begin(), tolower);
	if (isLocalBackupStr == "yes")
		isLocalBackup = true;


	std::vector<std::string> result;
	std::string cmdStr;

	if (!isLocalBackup)
	{
		std::string rootPathStr = pTaskInfo->pNASPath;
		std::string::iterator i;
		for (i = rootPathStr.begin(); i != rootPathStr.end(); ++i)
		{
			if (*i == '\\')
				break;
		}
		rootPathStr = rootPathStr.substr(0, i - rootPathStr.begin());
		cmdStr += "net use \"\\\\";
		cmdStr += pTaskInfo->pNASName;
		cmdStr += "\\";
		cmdStr += rootPathStr + "\" ";
		cmdStr += pTaskInfo->pNASPassword;
		cmdStr += " /user:\"";
		cmdStr += pTaskInfo->pNASUserName;
		cmdStr += "\"";
		result.push_back(cmdStr);
		cmdStr.clear();
	}

	std::string fileTypeStr = pTaskInfo->pBackupFileType;
	std::transform(fileTypeStr.begin(), fileTypeStr.end(), fileTypeStr.begin(), tolower);
	if (fileTypeStr == "gho")
	{
		std::string temp = pTaskInfo->pOperation;
		std::transform(temp.begin(), temp.end(), temp.begin(), tolower);
		cmdStr += GetApplicationPath() + "\\Ghost ";
		cmdStr += "-clone,";
		if (temp == "restore")
		{
			if (!isLocalBackup)
			{
				temp.clear();
				temp += "\"\\\\";
				temp += pTaskInfo->pNASName;
				temp += "\\";
				temp += pTaskInfo->pNASPath;
				temp += "\"";
			}
			else
			{
				UINT dm, pm;
				sscanf(pTaskInfo->pLocalDevice, "%u:%u", &dm, &pm);
				--dm;
				temp = "\"";
				temp += QueryMountPointByNumberA(dm, pm);
				temp += pTaskInfo->pLocalPath;
				temp += "\"";
			}

			cmdStr += "mode=pload,";
			cmdStr += "src=";
			cmdStr += temp + ":1";
			cmdStr += ",dst=";
			cmdStr += pTaskInfo->pTarget;
			cmdStr += " ";
			cmdStr += "-sure";
		}
		else if (temp == "backup")
		{
			if (!isLocalBackup)
			{
				temp.clear();
				temp += "\"\\\\";
				temp += pTaskInfo->pNASName;
				temp += "\\";
				temp += pTaskInfo->pNASPath;
				temp += "\"";
			}
			else
			{
				UINT dm, pm;
				sscanf(pTaskInfo->pLocalDevice, "%u:%u", &dm, &pm);
				--dm;
				temp = "\"";
				temp += QueryMountPointByNumberA(dm, pm);
				temp += pTaskInfo->pLocalPath;
				temp += "\"";
			}

			cmdStr += "mode=pdump,";
			cmdStr += "src=";
			cmdStr += pTaskInfo->pTarget;
			cmdStr += ",dst=";
			cmdStr += temp;
			cmdStr += " ";
			cmdStr += pTaskInfo->pExtraGhoArgument;
		}
		else
		{
			result.clear();
			return result;
		}
		result.push_back(cmdStr);
		cmdStr.clear();
	}
	else if (fileTypeStr == "wim")
	{
		std::string temp = "";

		temp = pTaskInfo->pOperation;
		std::transform(temp.begin(), temp.end(), temp.begin(), tolower);
		cmdStr += "dism ";
		if (temp == "restore")
		{
			std::string srcMountPoint;
			UINT dm, pm;
			sscanf(pTaskInfo->pTarget, "%u:%u", &dm, &pm);
			--dm;
			temp = QueryMountPointByNumberA(dm, pm);
			srcMountPoint = temp;
			temp.pop_back();
			temp = "format " + temp;
			temp += " /Q /Y";
			cmdStr = temp;
			result.push_back(cmdStr);
			cmdStr.clear();

			cmdStr = "dism ";

			if (!isLocalBackup)
			{
				temp.clear();
				temp += "\"\\\\";
				temp += pTaskInfo->pNASName;
				temp += "\\";
				temp += pTaskInfo->pNASPath;
				temp += "\"";
			}
			else
			{
				UINT dm, pm;
				sscanf(pTaskInfo->pLocalDevice, "%u:%u", &dm, &pm);
				--dm;
				temp = "\"";
				temp += QueryMountPointByNumberA(dm, pm);
				temp += pTaskInfo->pLocalPath;
				temp += "\"";
			}

			cmdStr += "/Apply-Image ";
			cmdStr += "/ImageFile:";
			cmdStr += temp;
			cmdStr += " /Index:1";
			cmdStr += " /ApplyDir:";
			cmdStr += srcMountPoint;
		}
		else if (temp == "backup")
		{
			std::string srcMountPoint;
			UINT dm, pm;
			printf(pTaskInfo->pTarget);
			sscanf(pTaskInfo->pTarget, "%u:%u", &dm, &pm);
			--dm;
			srcMountPoint = QueryMountPointByNumberA(dm, pm);
			if (!isLocalBackup)
			{
				temp.clear();
				temp += "\\\\";
				temp += pTaskInfo->pNASName;
				temp += "\\";
				temp += pTaskInfo->pNASPath;
				DeleteFileA(temp.c_str());
				temp = "\"" + temp + "\"";
			}
			else
			{
				temp.clear();
				UINT dm, pm;
				sscanf(pTaskInfo->pLocalDevice, "%u:%u", &dm, &pm);
				--dm;
				temp += QueryMountPointByNumberA(dm, pm);
				temp += pTaskInfo->pLocalPath;
				DeleteFileA(temp.c_str());
				temp = "\"" + temp + "\"";
			}

			cmdStr += "/Capture-Image ";
			cmdStr += "/ImageFile:";
			cmdStr += temp;
			cmdStr += " /CaptureDir:";
			cmdStr += srcMountPoint;
			cmdStr += " /Name:";
			cmdStr += srcMountPoint;
			cmdStr += " ";
			cmdStr += pTaskInfo->pExtraWimArgument;
		}
		else
		{
			result.clear();
			return result;
		}
		result.push_back(cmdStr);
		cmdStr.clear();
	}
	else
	{
		result.clear();
		return result;
	}
	return result;
}

//通过配置生成要执行的清理命令
std::vector<std::string> GenCleanupCommandLines(TASK_INFO* pTaskInfo)
{
	std::string cmdStr;
	std::vector<std::string> result;
	cmdStr += "bcdedit /set {bootmgr} default {";
	cmdStr += pTaskInfo->pLastDefaultGuid;
	cmdStr += "}";
	result.push_back(cmdStr);
	cmdStr.clear();

	cmdStr += "bcdedit /delete {";
	cmdStr += pTaskInfo->pDeviceGuid;
	cmdStr += "}";
	cmdStr += " /f /cleanup";
	result.push_back(cmdStr);
	cmdStr.clear();

	cmdStr += "bcdedit /delete ";
	cmdStr += "{";
	cmdStr += pTaskInfo->pOSLoaderGuid;
	cmdStr += "}";
	cmdStr += " /f /cleanup";
	result.push_back(cmdStr);
	cmdStr.clear();
	return result;
}

//依次执行命令，成功返回空串，失败返回执行失败的命令
std::string RunTask(std::vector<std::string> cmds)
{
	for (std::string cmd : cmds)
	{
		int dwExitCode = system(cmd.c_str());
		if (dwExitCode != 0)
			return cmd;
	}
	return "";
}

int main()
{
	std::string shutdownCmdStr = GetApplicationPath() + "\\shutdown /r /t 0";

	printf("Start task in Windows PE\n");

	//读取配置
	TASK_INFO taskInfo = { NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL };
	printf("Start reading config file!\n");
	if (!ReadConfig(&taskInfo))
	{
		MessageBoxA(NULL, "Reading config file failed,task abort!", "Runtime Error:", MB_ICONERROR | MB_OK);
		json_object_put(taskInfo.obj);
		system("bcdedit /delete {current} /f /cleanup");
		system(shutdownCmdStr.c_str());
		return 1;
	}
	printf("Reading config file success!\n");

	//生成命令行
	printf("Generating command lines...\n");
	std::vector<std::string> taskCmds = GenTaskCommandLines(&taskInfo), cleanCmds = GenCleanupCommandLines(&taskInfo);
	if (!taskCmds.size() || !cleanCmds.size())
	{
		MessageBoxA(NULL, "Generating command lines failed!", "Runtime Error:", MB_ICONERROR | MB_OK);
		json_object_put(taskInfo.obj);
		system("bcdedit /delete {current} /f /cleanup");
		system(shutdownCmdStr.c_str());
		return 1;
	}
	printf("Generating command lines success\n");

	json_object_put(taskInfo.obj);

	printf("run command lines...\n");
	//执行命令
	std::string failedCmd = RunTask(taskCmds);
	RunTask(cleanCmds);
	if (failedCmd.length())
	{
		failedCmd = "Command \"" + failedCmd;
		failedCmd += "\" run failed!";
		MessageBoxA(NULL, failedCmd.c_str(), "Runtime Error:", MB_ICONERROR | MB_OK);
		system(shutdownCmdStr.c_str());
		return 1;
	}
	printf("run command lines success,task finished!\n");

	system(shutdownCmdStr.c_str());

	return 0;
}