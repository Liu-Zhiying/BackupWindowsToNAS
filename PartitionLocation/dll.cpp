#define DLL_MAKER
#include "partition_location.h"
#include <stdio.h>
#pragma warning(disable : 4996)

//ͨ������·����ȡ������Ӳ�̺źͷ����ţ�����CreateFile��ȡ�����DeviceIoiControl��ȡӲ�̺źͷ�����
C_LINK DLL BOOL CALLBACK GetPartitionLocationW(IN LPCWSTR pWin32NamespaceName, OUT LPUINT pDiskNumber, OUT LPUINT pPartitionNumber)
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