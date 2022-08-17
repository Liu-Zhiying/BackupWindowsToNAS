using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BackupOSToNAS
{
    internal class Native
    {
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLogicalDriveStringsW")]
        extern public static uint GetLogicalDriveStringsW(uint nBufferLength, IntPtr lpBuffer);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "QueryDosDeviceW", CharSet = CharSet.Unicode)]
        extern public static uint QueryDosDeviceW(string lpDeviceName, IntPtr lpTargetPath, uint ucchMax);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDriveTypeW", CharSet = CharSet.Unicode)]
        extern public static uint GetDriveTypeW(string lpRootPathName);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "AllocConsole")]
        extern public static bool AllocConsole();
        [DllImport("PartitionLocation.dll", CallingConvention = CallingConvention.StdCall,EntryPoint = "GetPartitionLocationW",CharSet = CharSet.Unicode)]
        extern public static bool GetPartitionLocationW(string pWin32NamespaceName, IntPtr pDiskNumber, IntPtr pPartitionNumber);
    }
    //固件类型枚举，用于GetFirmwareType
    enum FIRMWARE_TYPE
    {
        FirmwareTypeUnknown = 0,
        FirmwareTypeBios = 1,
        FirmwareTypeUefi = 2,
        FirmwareTypeMax = 4
    }
    //分区设备类型枚举，用于GetDeviceTypeW
    enum DEVICE_TYPE
    {
        DRIVE_UNKNOWN = 0,
        DRIVE_NO_ROOT_DIR = 1,
        DRIVE_REMOVABLE = 2,
        DRIVE_FIXED = 3,
        DRIVE_REMOTE = 4,
        DRIVE_CDROM = 5,
        DRIVE_RAMDISK = 6
    }
}
