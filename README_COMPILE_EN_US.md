# How to compile this program

## Environment

* Visual Studio 2022 which installed C++ desktop development pack and .NET desktop development pack

* Windows wchich enabled .NET Framework 3.5  

* .NET Framework 4.0 Target Pack  

## Preparation

Download source

Download and setting VCPKG, See [VCPKG](https://github.com/microsoft/vcpkg)  

Install json-c 64bit static library in VCPKG(Command Line: "vcpkg install json-c:x64-windows-static") for building 64 bit application

Install json-c 32bit static library in VCPKG(Command Line: "vcpkg install json-c:x86-windows-static") for building 32 bit application

Set Integration with VCPKG and PERunner VC++ project in this source  

## Compile source

Open BackOSToNAS.sln and compile in Visual Studio

## Copy files and finish

Create a new folder

Copy boot.sdi under Resource to new folder.

Copy files with "_X86" suffix under Resource to new folder for 32 bit application.

Copy files with "_X64" suffix under Resource to new folder for 64 bit application.

Copy PartitionLocation.dll PERunner.exe Newtonsoft.Json.dll Newtonsoft.Json.xml BackupOSToNAS.exe to new folder  

Create a enpty folder named "mount" in new folder

Rename files with suffix "_X64" or "_X86" to delete suffix

Rename wim file to "winpe.wim"
