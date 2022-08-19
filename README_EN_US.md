# BackupWindowsToNAS

A application for users who want backup their Windows partition to NAS by SMB protocol  

## The technique we use now

* NET Framework 3.5 / 4.0  

* Visual C++ 2022 (in Visual Studio 2022)  

## System requirement

* Windows 7 and Windows Server 2008 R2 or higher (64bit Windows only)  

* .NET Framework 4.0 (Windows 8 and higher) or .NET Framework 3.5 (for Windows 7)

* Do not need install Visual C++ Redistributable, all native libraries is stataic linked to Visual C++ Runtime  

## Usage

* Go to [Release](https://github.com/Liu-Zhiying/BackupWindowsToNAS/releases) and download software  

* If you are Windows 7 x64 user,using Software build with .NET Framework 3.5

* If you are Windows 8 or higher x64 user,using Software build with .NET Framework 4

* Run BackupOSToNAS.exe in package and input parameters then click button

### Note

* SMB1.0 is unsupported

* NAS Path can not begining with \

* You should check username,password,path by yourself

* Can not change SMB port (a feature of Windows)

* Can not save parameters last using.

* This software using Ghost for backuping, please write the suffix of file to gho

## How to compile this program

[Document in English](README_COMPILE_EN_US.md)

## About author

* QQ:1103660629

* Email:1103660629@qq.com  

Welcome to post issue or commnuite with me for this project
