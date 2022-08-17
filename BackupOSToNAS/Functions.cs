using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace BackupOSToNAS
{
    internal static class Functions
    {
        static Random random;
        static Functions()
        {
            random = new Random();
        }
        //添加WindowsPE启动项
        internal static bool AddWinPEBootLoader(Guid devicePropertyGuid, Guid OSLoaderPropertyGuid)
        {
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
                string applicationPath = GetApplicationFullPath();
                string applicationPartitionName = GetPartitionNameFromFullPath(applicationPath);
                string applicationPathWithoutPartitonName = GetPathWithoutPartitionNameFromFullPath(applicationPath);
                if (applicationPartitionName == null)
                    throw new Exception("Wrong path of application");

                //设置引导
                string pathArg = isUEFIBoot ? "\\windows\\system32\\winload.efi" : "\\windows\\system32\\winload.exe";
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
                    $"/set {'{' + OSLoaderPropertyGuid.ToString() + '}'} bootmenupolicy Standard",
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
                        UseShellExecute = true,
                        FileName = "bcdedit.exe",
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
                        UseShellExecute = true,
                        FileName = "bcdedit.exe",
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
            string targetSubKeyName = null;
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
            if (targetSubKeyName != null)
            {
                RegistryKey tempKey = regKey.OpenSubKey(targetSubKeyName + "\\Elements\\23000003");
                defaultObjName = (string)tempKey.GetValue("Element");
                tempKey.Close();
            }
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
            for (int i = applicationPath.Length - 1; i != 0; --i)
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
                        strBulider.Clear();
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
        //获取分区的Win32 Namespace 路径，核心是调用QueryDosDeviceW
        internal static string GetPhysicalPath(string deviceName)
        {
            string result = "";
            if (deviceName == null)
                return result;
            int bufferBytes = sizeof(char) * 32768;
            IntPtr pBuffer = Marshal.AllocHGlobal(bufferBytes);
            uint callCode = Native.QueryDosDeviceW(deviceName, pBuffer, (uint)bufferBytes / sizeof(char) - 1);
            if (callCode != 0)
                result = Marshal.PtrToStringUni(pBuffer);
            Marshal.FreeHGlobal(pBuffer);
            return result;
        }
        //获取分区的硬盘号和分区号，调用GetPartitionLocationW
        //注意：被调用的函数非Windows API，其定义见项目PartitionLocation
        internal static bool GetPartitionLocation(string win32NamespaceName, out PartitionLocation result)
        {
            result = new PartitionLocation() { partitionNumber = 0, diskNumber = 0 };
            IntPtr pDiskNumber = Marshal.AllocHGlobal(sizeof(uint)), pPartitionNumber = Marshal.AllocHGlobal(sizeof(uint));
            bool callCode = Native.GetPartitionLocationW(win32NamespaceName, pDiskNumber, pPartitionNumber);
            if (callCode)
            {
                result.diskNumber = (uint)Marshal.ReadInt32(pDiskNumber, 0);
                result.partitionNumber = (uint)Marshal.ReadInt32(pPartitionNumber, 0);
            }
            Marshal.FreeHGlobal(pDiskNumber);
            Marshal.FreeHGlobal(pPartitionNumber);
            return callCode;
        }
        internal static Guid GenRandomGuid()
        {
            return new Guid
            (
                random.Next(),
                (short)random.Next(short.MaxValue),
                (short)random.Next(short.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue),
                (byte)random.Next(byte.MaxValue)
            );
        }
    }
    internal class PartitionLocation
    {
        public uint diskNumber;
        public uint partitionNumber;
    }
}
