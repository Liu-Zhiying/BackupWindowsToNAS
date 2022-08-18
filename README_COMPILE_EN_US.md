# How to compile this program

## Environment

* Visual Studio 2022 which installed C++ desktop development pack and .NET desktop development

* Windows wchich enabled .NET Framework 3.5

* .NET Framework 4.0 Target Pack  

## Preparation

Download source

Download and setting VCPKG, See [VCPKG](https://github.com/microsoft/vcpkg)  

Install json-c 64bit static library in VCPKG(Command Line: "vcpkg install json-c:x64-windows-static")  

Set Integration with VCPKG and PERunner VC++ project in this source  

## Compile source

Open BackOSToNAS.sln and compile in Visual Studio

## Copy files and finish

Create a new folder

Copy files under Resource to new folder.

Copy PartitionLocation.dll PERunner.exe Newtonsoft.Json.dll Newtonsoft.Json.xml BackupOSToNAS.exe to new folder  

Create a enpty folder named "mount" in new folder
