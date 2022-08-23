using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    internal static class Functions
    {
        //检查NasName NasPath NasUser NasPassword
        internal static bool ParamIsVaild(string NASName, string NASPath, string NASUser, string NASPassword)
        {
            bool result = false;
            if (NASPath.IndexOf('\\') == -1)
                return result;
            string NASSharedFolderName = NASPath.Substring(0, NASPath.IndexOf('\\'));
            string NASConnectCmd = $"/C \"net use \"\\\\{NASName}\\{NASSharedFolderName}\" {NASPassword} /user:\"{NASUser}\"\"";
            string NASDisconnectCmd = $"/C \"net use \"\\\\{NASName}\\{NASSharedFolderName}\" /delete\"";
            ProcessStartInfo info = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = NASConnectCmd
            };
            Process process = Process.Start(info);
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                string temp = NASPath;
                for (int index = temp.Length - 1; index > -1; index--)
                {
                    if (temp[index] == '\\')
                    {
                        temp = temp.Substring(0, index);
                        break;
                    }
                }
                if (File.Exists($"\\\\{NASName}\\{NASPath}") || Directory.Exists($"\\\\{NASName}\\{temp}"))
                {
                    result = true;
                }
            }
            info.Arguments = NASDisconnectCmd;
            process = Process.Start(info);
            process.WaitForExit();
            return result;
        }
        //修改Windows PE，加入PE备份程序
        internal static string ModifyWindowsPE(Action<int> callback)
        {
            ProcessStartInfo startInfo;
            Process dismProcess;
            string appPath = GetApplicationFullPath();
            try
            {
                callback(1);
                startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = $"{appPath}\\Tools\\imagex.exe",
                    Arguments = $"/Mountrw {'\"' + appPath + "\\winpe.wim" + '\"'} 1 {'\"' + appPath + "\\mount" + '\"'}",
                    CreateNoWindow = true
                };
                dismProcess = Process.Start(startInfo);
                dismProcess.WaitForExit();
                if (dismProcess.ExitCode != 0)
                    throw new Exception("Mount Windows PE failed!");

                callback(2);
                File.Copy(appPath + "\\PERunner.exe", appPath + "\\mount\\PERunner.exe", true);
                File.Copy(appPath + "\\Ghost.exe", appPath + "\\mount\\Ghost.exe", true);
                File.Copy(appPath + "\\config.json", appPath + "\\mount\\config.json", true);
                File.Copy(appPath + "\\shutdown.exe", appPath + "\\mount\\shutdown.exe", true);

                string PEStartupScriptContent = File.ReadAllText($"{appPath}\\mount\\Windows\\System32\\Startnet.cmd");
                if (!PEStartupScriptContent.ToLower().Contains("%systemdrive%\\perunner.exe"))
                {
                    PEStartupScriptContent += "\n%SystemDrive%\\PERunner.exe";
                    File.WriteAllText($"{appPath}\\mount\\Windows\\System32\\Startnet.cmd", PEStartupScriptContent);
                }

                callback(3);
                startInfo.UseShellExecute = false;
                startInfo.FileName = $"{appPath}\\Tools\\imagex.exe";
                startInfo.Arguments = $"/Unmount /Commit {'\"' + appPath + "\\mount" + '\"'}";
                startInfo.CreateNoWindow = true;
                dismProcess = Process.Start(startInfo);
                dismProcess.WaitForExit();
                if (dismProcess.ExitCode != 0)
                    throw new Exception("Unmount Windows PE failed!");
            }
            catch (Exception e)
            {
                startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    FileName = $"{appPath}\\Tools\\imagex.exe",
                    Arguments = $"/Unmount  {'\"' + appPath + "\\mount" + '\"'}",
                    CreateNoWindow = true
                };
                dismProcess = Process.Start(startInfo);
                dismProcess.WaitForExit();
                return e.Message;
            }
            return null;
        }
        //添加WindowsPE启动项
        internal static bool AddWinPEBootLoader(Guid devicePropertyGuid, Guid OSLoaderPropertyGuid)
        {
            string applicationPath = GetApplicationFullPath();
            try
            {
                bool isUEFIBoot = false;
                //判断固件类型
                FIRMWARE_TYPE type = GetFirmwareType();
                if (type == FIRMWARE_TYPE.FirmwareTypeBios)
                    isUEFIBoot = false;
                else if (type == FIRMWARE_TYPE.FirmwareTypeUefi)
                    isUEFIBoot = true;
                else
                    throw new Exception("未知的固件类型！");

                //解析应用全路径，应用盘符，去除应用盘符的全路径

                string applicationPartitionName = GetPartitionNameFromFullPath(applicationPath);
                string applicationPathWithoutPartitonName = GetPathWithoutPartitionNameFromFullPath(applicationPath);
                if (applicationPartitionName == null)
                    throw new Exception("Wrong path of application");

                //设置引导
                string pathArg = isUEFIBoot ? "\\windows\\system32\\boot\\winload.efi" : "\\windows\\system32\\boot\\winload.exe";
                string[] argumentLists = new string[]
                {
                    $"/create {'{' + devicePropertyGuid.ToString() + '}'} /d \"Backup OS To NAS Device Options\" /device",
                    $"/create {'{' + OSLoaderPropertyGuid.ToString() + '}'} /d \"Backup OS To NAS\" /Application OSLOADER",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} device ramdisk=[{applicationPartitionName}]\\{applicationPathWithoutPartitonName + "\\winpe.wim"},{'{' + devicePropertyGuid.ToString() + "}"}",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} osdevice ramdisk=[{applicationPartitionName}]\\{applicationPathWithoutPartitonName + "\\winpe.wim"},{'{' + devicePropertyGuid.ToString() + "}"}",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} systemroot \\windows",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} locale zh-cn",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} inherit {'{'}bootloadersettings{'}'}",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} path {pathArg}",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} nx OptIn",
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} winpe Yes",
                    $"/set {'{' + devicePropertyGuid.ToString() + '}'} ramdisksdidevice partition={applicationPartitionName}",
                    $"/set {'{' + devicePropertyGuid.ToString() + '}'} ramdisksdipath {'\\' + applicationPathWithoutPartitonName + "\\boot.sdi"}",
                    $"/set {'{' + "bootmgr" + '}'} default {'{' + OSLoaderPropertyGuid.ToString() + '}'}",
                    $"/displayorder {'{' + OSLoaderPropertyGuid.ToString() + '}' } /addlast"
                };
                foreach (string argument in argumentLists)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        FileName = $"bcdedit.exe",
                        Arguments = argument,
                        CreateNoWindow = true
                    };
                    Process process = Process.Start(startInfo);
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                        throw new Exception("Set bootloader failed!");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                string[] argumentLists = new string[]
                {
                    $"/delete {'{' + devicePropertyGuid.ToString() + '}'} /f /cleanup",
                    $"/delete {'{' + OSLoaderPropertyGuid.ToString() + '}'} /f /cleanup"
                };
                foreach (string argument in argumentLists)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        FileName = $"bcdedit.exe",
                        Arguments = argument,
                        CreateNoWindow = true
                    };
                    Process.Start(startInfo);
                }
                return false;
            }
            return true;
        }
        //获取Windows启动文件名，本质读取BCD注册表
        internal static string GetWindowsBootFileName()
        {
            string WindowsBootFileName = "";
            string defaultObjGuid = GetBootDefaultItem();
            if (defaultObjGuid.Length != 0)
            {
                RegistryKey tempKey = Registry.LocalMachine.OpenSubKey($"BCD00000000\\Objects\\{defaultObjGuid}\\Elements\\12000002");
                WindowsBootFileName = (string)tempKey.GetValue("Element");
                tempKey.Close();
            }
            return WindowsBootFileName;
        }
        //获取BCD默认启动项的GUID，本质读取BCD注册表
        internal static string GetBootDefaultItem()
        {
            string defaultObjName = "";
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey("BCD00000000\\Objects");
            RegistryKey tempKey = null;
            string targetSubKeyName = null;
            try
            {
                foreach (string subKeyName in regKey.GetSubKeyNames())
                {
                    RegistryKey regSubKey = regKey.OpenSubKey(subKeyName + "\\Description");
                    int value = (int)regSubKey.GetValue("Type");
                    regSubKey.Close();
                    if ((value >> 28) == 0x1 && (value & 0xfffff) == 0x2)
                    {
                        targetSubKeyName = subKeyName;
                        break;
                    }
                }
                tempKey = regKey.OpenSubKey(targetSubKeyName + "\\Elements\\23000003");
                defaultObjName = (string)tempKey.GetValue("Element");
            }
            catch
            {

            }
            if (tempKey != null)
                tempKey.Close();
            if (regKey != null)
                regKey.Close();
            return defaultObjName;
        }
        //基于Windows启动文件名的后缀判断固件类型
        internal static FIRMWARE_TYPE GetFirmwareType()
        {
            FileInfo info = new FileInfo(GetWindowsBootFileName());
            if (info.Extension.ToLower() == ".efi")
                return FIRMWARE_TYPE.FirmwareTypeUefi;
            else if (info.Extension.ToLower() == ".exe")
                return FIRMWARE_TYPE.FirmwareTypeBios;
            else
                return FIRMWARE_TYPE.FirmwareTypeUnknown;
        }
        //获取应用程序的路径
        private static string GetApplicationFullPath()
        {
            string applicationPath = Process.GetCurrentProcess().MainModule.FileName;
            for (int i = applicationPath.Length - 1; i > -1; --i)
            {
                if (applicationPath[i] == '\\')
                {
                    applicationPath = applicationPath.Substring(0, i);
                    break;
                }
            }
            return applicationPath;
        }
        //从全路径中解析盘符
        internal static string GetPartitionNameFromFullPath(string fullPath)
        {
            string[] results = fullPath.Trim().Split('\\');
            return (results != null && results.Length != 0) ? results[0] : null;
        }
        //从全路径中去除盘符
        internal static string GetPathWithoutPartitionNameFromFullPath(string fullPath)
        {
            for (int i = 0; i < fullPath.Length; ++i)
            {
                if (fullPath[i] == '\\')
                    return fullPath.Substring(i + 1, fullPath.Length - i - 1);
            }
            return null;
        }
        //获取当前存在的盘符（限硬盘和移动设备）核心是调用GetLogicalDriveStringsW
        internal static List<string> GetCurrentMountPoints()
        {
            bool isLastCharZero = false;
            List<string> result = new List<string>();
            int bufferBytes = sizeof(char) * 32768;
            IntPtr pBuffer = Marshal.AllocHGlobal(bufferBytes);
            uint callCode = Native.GetLogicalDriveStringsW((uint)bufferBytes / sizeof(char) - 1, pBuffer);
            if (callCode != 0)
            {
                StringBuilder strBulider = new StringBuilder("");
                for (int i = 0; i < bufferBytes; i += sizeof(char))
                {
                    char c = (char)Marshal.ReadInt16(pBuffer, i);
                    if (c != 0)
                    {
                        strBulider.Append(c);
                        isLastCharZero = false;
                    }
                    else if (!isLastCharZero)
                    {
                        string item = strBulider.ToString();
                        callCode = Native.GetDriveTypeW(item);
                        if (callCode == (uint)DEVICE_TYPE.DRIVE_FIXED || callCode == (uint)DEVICE_TYPE.DRIVE_REMOVABLE)
                            result.Add(item);
                        strBulider.Remove(0, strBulider.Length);
                        isLastCharZero = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Marshal.FreeHGlobal(pBuffer);
            return result;
        }
        //获取分区的硬盘号和分区号，调用GetPartitionLocationW
        //注意：被调用的函数非Windows API，其定义见项目PartitionLocation
        internal static bool GetPartitionLocation(string mountPointRoot, out PartitionLocation result)
        {
            mountPointRoot = "\\\\.\\" + mountPointRoot;
            result = new PartitionLocation() { partitionNumber = 0, diskNumber = 0 };
            bool callCode = Native.GetPartitionLocationW(mountPointRoot, out result.diskNumber, out result.partitionNumber);
            result.diskNumber += 1;
            return callCode;
        }
        //从字符串解析Guid，注意写这个方法是因为.NET franmework 3.5 没有 Guid.Parse()
        internal static bool ParseGuid(string guidStr, out Guid result)
        {
            result = Guid.Empty;
            StringBuilder builder = new StringBuilder(guidStr);
            builder.Replace("{", "");
            builder.Replace("}", "");
            string[] guidPartStrs = builder.ToString().Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (
                guidPartStrs.Length != 5 &&
                guidPartStrs[0].Length == 8 &&
                guidPartStrs[1].Length == 4 &&
                guidPartStrs[2].Length == 4 &&
                guidPartStrs[3].Length == 4 &&
                guidPartStrs[0].Length == 12
            )
                return false;
            result = new Guid
            (
                (int)Convert.ToUInt32(guidPartStrs[0], 16),
                (short)Convert.ToUInt16(guidPartStrs[1], 16),
                (short)Convert.ToUInt16(guidPartStrs[2], 16),
                Convert.ToByte(guidPartStrs[3].Substring(0, 2), 16),
                Convert.ToByte(guidPartStrs[3].Substring(2, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(0, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(2, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(4, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(6, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(8, 2), 16),
                Convert.ToByte(guidPartStrs[4].Substring(10, 2), 16)
            );
            return true;
        }
        internal static string QueryMountPointByPartitionLocation(int diskNumber, int partitionNumber)
        {
            List<string> mountPoints = GetCurrentMountPoints();
            foreach (string mountPoint in mountPoints)
            {
                string mountPointRoot = mountPoint.Replace("\\", "");
                GetPartitionLocation("\\\\.\\" + mountPointRoot, out PartitionLocation location);
                if (location.diskNumber == diskNumber && location.partitionNumber == partitionNumber)
                    return mountPoint;
            }
            return "";
        }
    }
    internal class PartitionLocation
    {
        public uint diskNumber;
        public uint partitionNumber;
    }
}
