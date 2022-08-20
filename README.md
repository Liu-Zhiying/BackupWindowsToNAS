# BackupWindowsToNAS

[English](README_EN_US.md)

针对NAS用户的Windows分区备份工具，使用SMB协议

## 第二版更新内容

* 现在在输入NAS参数后可以浏览决定备份文件的位置

* 现在可以读取上一次的NAS备份配置

* 现在会对NAS备份参数进行检查

* UI优化，隐藏了调用命令的cmd窗口

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

* 如果你备份的机器本身有SMB共享，切勿填入本机SMB参数（这个问题后面也不会处理）

* 浏览时NAS路径建议清空，不清空的话，路径要正确，此外备份文件后缀仍需自己添加

* 还原时不会检查文件是否存在

* 无法更改端口（Windows特性）只能使用默认端口  

* 本软件使用Ghost备份，文件后缀请写gho

## 如何编译本项目

[Document in English](README_COMPILE_EN_US.md)

## 关于作者

* QQ:1103660629  

* Email:1103660629@qq.com  

欢迎提issue或者联系我

## 预告-下一个版本

* 将添加DISM备份支持

* 将支持自定义备份参数

* 将支持本地备份

* 将无需添加后缀

* 在还原时将检查文件是否存在

* PS:后面两点没搞纯属我懒
