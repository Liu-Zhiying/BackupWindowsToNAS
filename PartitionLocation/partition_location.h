#ifndef PARTITION_LOCATION_H

#ifdef DLL_MAKER
#define DLL __declspec(dllexport)
#else
#define DLL __declspec(dllimport)
#endif

#define C_LINK extern "C"

#include <Windows.h>

C_LINK DLL BOOL CALLBACK GetPartitionLocationW(IN LPCWSTR pWin32NamespaceName, OUT LPUINT pDiskNumber, OUT LPUINT pPartitionNumber);

C_LINK DLL BOOL CALLBACK PartitionLocationToMountPointW(IN UINT diskNumber, IN UINT partitionNumber, IN OUT LPCWSTR lpStrBuffer, IN OUT LPDWORD lpBufferBytes);

#endif // !PARTITION_LOCATION_H
