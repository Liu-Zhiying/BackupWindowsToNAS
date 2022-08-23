# BackupWindowsToNAS

A application for users who want backup their Windows partition to NAS by SMB protocol  

## Fix of the V2-fix

* Fix a programming problem that is WinForm flushing UI with multi threads.

* Fix a problem that application stacks when initailizing failed.

* Add support for Windows Vista and 32bit Windows (See system requirement)

* Change technique, Windows 7 and Windows Vista version is built with .NET Framework 3.0 (See The technique we use now)

## The technique we use now

* NET Framework 3.0 / 4.0  

* Visual C++ 2022 (in Visual Studio 2022)  

## System requirement

* Windows Vista and Windows Server 2008 or higher

* .NET Framework 4.0 (Windows 8 and higher) or .NET Framework 3.0 (for Windows 7 and Windows Vista)

* Do not need install Visual C++ Redistributable, all native libraries is stataic linked to Visual C++ Runtime  

## Usage

* Go to [Release](https://github.com/Liu-Zhiying/BackupWindowsToNAS/releases) and download software  

* If you are Windows 7 user,using Software build with .NET Framework 3.0

* If you are Windows 8 or higher user,using Software build with .NET Framework 4.0

* X86 user using X86 version, X64 user using X64 version

* Run BackupOSToNAS.exe in package and input parameters then click button

* Note Vista user should install wimmount.inf in Tools Folder before first use.

### Note

* SMB1.0 is unsupported

* NAS Path can not begining with \

* Can not change SMB port (a feature of Windows)

* Application do not check file is exist when restoring.

* if you machine which has SMB shared folder to backup, Note do not input the parameter of this machine (this problem will not solve in new version)

* When browsing the folder, clear the NAS path is recommanded, or input a right path,you should still add the suffi of backup file by yourself.

* This software using Ghost for backuping, please write the suffix of file to gho

## How to compile this program

[Document in English](README_COMPILE_EN_US.md)

## About author

* QQ:1103660629

* Email:1103660629@qq.com  

Welcome to post issue or commnuite with me for this project

## Annountation of V3

* Will add dism backup support

* Will allow user to input extra parameter to backup process

* Will support backup in local.

* Will not need to add suffix to backup file

* Will check file exists in restoring

* PS:the last tow changes are not added to this version is my lazy
