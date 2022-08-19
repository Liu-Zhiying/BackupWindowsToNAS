# BackupWindowsToNAS

[English](README_EN_US.md)

针对NAS用户的Windows分区备份工具，使用SMB协议

## 技术栈

* NET Framework 3.5 / 4.0  

* Visual C++ 2022 (in Visual Studio 2022)  

## 系统要求

* Windows 7 和 Windows Server 2008 R2 或更高 （限64位 Windows）  

* .NET Framework 4.0 (Windows 8 及更高) 或者 .NET Framework 3.5 (针对 Windows 7)

* 不需要安装Visual C++ Redistributable,所用的本机库静态链接到Visual C++ Runtime.

## 使用方式

到 [Release](https://github.com/Liu-Zhiying/BackupWindowsToNAS/releases) 下载软件包  

Windows 7 64位用户使用 .NET Framework 3.5构建的版本  

Windows 8 和更高版本 64位用户使用 .NET Framework 4.0构建的版本  

下载完成之后，运行BackupOSToNAS.exe，输入参数，选择备份或者还原即可  

### 注意

* 不支持SMB1.0  

* NAS路径开头不要写 \  

* 用户名 密码 路径 需要自己核对  

* 无法更改端口（Windows特性）只能使用默认端口  

* 无法保存上次的参数

* 本软件使用Ghost备份，文件后缀请写gho

## 如何编译本项目

[Document in English](README_COMPILE_EN_US.md)

## 关于作者

* QQ:1103660629  

* Email:1103660629@qq.com  

欢迎提issue或者联系我
