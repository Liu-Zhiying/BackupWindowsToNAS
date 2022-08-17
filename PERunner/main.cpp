#include <Windows.h>
#include <stdio.h>
#include <json-c/json.h>

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
} TASK_INFO;

size_t GetApplicationPath(const char** pResult)
{
	*pResult = NULL;
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
			uResultLength = (strlen(pBuffer) + 1) * sizeof(char);
			*pResult = (const char*)malloc(uResultLength);
			memcpy((void*)*pResult, pBuffer, uResultLength);
		}
		free(pBuffer);
	}
	return uResultLength;
}

int ReadConfig(TASK_INFO* pTaskInfo)
{
	int result = 0;
	json_object* obj = json_object_from_file("config.json");
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
		if (pTaskInfo->pTarget == NULL || pTaskInfo->pDeviceGuid == NULL ||
			pTaskInfo->pLastDefaultGuid == NULL || pTaskInfo->pNASName == NULL ||
			pTaskInfo->pNASPassword == NULL || pTaskInfo->pNASPath == NULL ||
			pTaskInfo->pNASUserName == NULL || pTaskInfo->pOperation == NULL ||
			pTaskInfo->pOSLoaderGuid == NULL)
			return 0;
		else
			return 1;
	}
	return 0;
}

int RunTask(TASK_INFO* pTaskInfo)
{
	return 0;
}

BOOL CALLBACK GetPartitionLocationW(IN LPCWSTR pWin32NamespaceName, OUT LPUINT pDiskNumber, OUT LPUINT pPartitionNumber)
{
	BOOL bResult = FALSE;
	HANDLE hPartition = CreateFileW(pWin32NamespaceName, 0, FILE_SHARE_READ | FILE_SHARE_WRITE, NULL, OPEN_EXISTING, 0, NULL);
	if (hPartition != INVALID_HANDLE_VALUE)
	{
		STORAGE_DEVICE_NUMBER info = {};
		DWORD dwReceivedBytes = 0;
		BOOL bCallResult = DeviceIoControl(hPartition, IOCTL_STORAGE_GET_DEVICE_NUMBER, NULL, 0, &info, sizeof info, &dwReceivedBytes, NULL);
		if (bCallResult)
		{
			*pDiskNumber = info.DeviceNumber;
			*pPartitionNumber = info.PartitionNumber;
			bResult = TRUE;
		}
		CloseHandle(hPartition);
	}
	return bResult;
}

int main()
{
	printf("Start backup in Windows PE\n");
	//设置工作目录
	const char* applicationPath = NULL;
	GetApplicationPath(&applicationPath);
	SetCurrentDirectoryA(applicationPath);
	free((void*)applicationPath);

	//读取配置
	TASK_INFO taskInfo = { NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL };
	printf("Start reading config file!\n");
	if (!ReadConfig(&taskInfo))
	{
		MessageBoxA(NULL, "Reading config file failed,task abort!", "Runtime Error:", MB_ICONERROR | MB_OK);
		json_object_put(taskInfo.obj);
		return 0;
	}
	printf("Reading config file success!");
	json_object_put(taskInfo.obj);
}