# BackupWindowsToNAS

[English](README_EN_US.md)

针对NAS用户的Windows分区备份工具，使用SMB协议

## V3 更新内容

* 添加DISM备份支持

* 支持自定义备份参数

* 支持本地备份

* 无需添加后缀（如果后缀不存在）

* 在还原时将检查文件是否存在

## 技术栈

* .NET Framework 3.0 / 4.0  

* Visual C++ 2022 (in Visual Studio 2022)  

## 系统要求

* Windows Vista 和 Windows Server 2008 或更高  

* .NET Framework 4.0 (Windows 8 及更高) 或者 .NET Framework 3.0 (针对 Windows 7 和 Windows Vista)

* 不需要安装Visual C++ Redistributable,所用的本机库静态链接到Visual C++ Runtime.

## 使用方式

到 [Release](https://github.com/Liu-Zhiying/BackupWindowsToNAS/releases) 下载软件包  

* Windows 7 和 Windows Vista 用户使用 .NET Framework 3.0构建的版本  

* Windows 8 和更高版本 用户使用 .NET Framework 4.0构建的版本  

* 32位用户使用X86版本，64位用户使用X64版本

* 下载完成之后，运行BackupOSToNAS.exe，输入参数，选择备份或者还原即可  

* Vista 用户运行前额外操作 请安装 Tools 下的 wimmount.inf

### 注意

* 不支持SMB1.0  

* NAS路径开头不要写 \  

* 如果你备份的机器本身有SMB共享，切勿填入本机SMB参数

* 浏览时NAS路径建议清空，不清空的话，路径要正确

* 无法更改端口（Windows特性）只能使用默认端口  

## 如何编译本项目

[Document in English](README_COMPILE_EN_US.md)

## 关于作者

* QQ:1103660629  

* Email:1103660629@qq.com  

欢迎提issue或者联系我

## 预告-V4

* UI 双语言支持（简体中文 / English）  

* 修改资源中的Wim，使之支持SMB1.0  
