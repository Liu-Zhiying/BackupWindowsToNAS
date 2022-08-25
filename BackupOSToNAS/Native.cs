using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace BackupOSToNAS
{
    internal class Native
    {
        internal const uint FILE_SHARE_READ = 0x00000001;
        internal const uint FILE_SHARE_WRITE = 0x00000002;
        internal const uint OPEN_EXISTING = 3;
        internal const uint IOCTL_STORAGE_GET_DEVICE_NUMBER = 2953344;
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLogicalDriveStringsW")]
        extern public static uint GetLogicalDriveStringsW(uint nBufferLength, IntPtr lpBuffer);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "GetDriveTypeW", CharSet = CharSet.Unicode)]
        extern public static uint GetDriveTypeW(string lpRootPathName);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "AllocConsole")]
        extern public static bool AllocConsole();
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]
        extern public static IntPtr CreateFileW(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "CloseHandle")]
        extern public static bool CloseHandle(IntPtr handle);
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, EntryPoint = "DeviceIoControl")]
        extern public static bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, IntPtr lpOutBuffer, uint nOutBufferSize, out uint lpBytesReturned, IntPtr lpOverlapped);
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
    [StructLayout(LayoutKind.Sequential)]
    struct STORAGE_DEVICE_NUMBER
    {
        [MarshalAs(UnmanagedType.U4)]
        public DEVICE_TYPE DeviceType;
        [MarshalAs(UnmanagedType.U4)]
        public uint DeviceNumber;
        [MarshalAs(UnmanagedType.U4)]
        public uint PartitionNumber;
    };
}
