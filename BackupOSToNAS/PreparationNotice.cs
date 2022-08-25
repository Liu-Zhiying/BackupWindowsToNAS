using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    public partial class PreparationNotice : Form
    {
        //为了支持.NET Framework 3.0，建立的委托类型
        delegate void SimpleAction();
        public PreparationNotice(Form fatherForm, string target
            , string operation, string NASName
            , string NASPath, string NASUser
            , string NASPassword, string FileType
            , string IsLocalBackup, string ExtraGhoArgument
            , string ExtraWimArgument, string LocalPath)
        {
            InitializeComponent();
            this.fatherForm = fatherForm;
            fatherForm.Enabled = false;
            this.NASName = NASName;
            this.NASPath = NASPath;
            this.NASPassword = NASPassword;
            this.NASUser = NASUser;
            this.target = target;
            this.FileType = FileType;
            this.IsLocalBackup = IsLocalBackup;
            this.ExtraGhoArgument = ExtraGhoArgument;
            this.ExtraWimArgument = ExtraWimArgument;
            this.LocalPath = LocalPath;
            this.Operation = operation;
        }

        //备份和还原的操作只是config一个字段的不同
        //所以这个过程单独写一个方法被按钮事件函数调用
        private void StartOperate(object operation)
        {
            string defaultBootItemGuidStr = Functions.GetBootDefaultItem();
            if (defaultBootItemGuidStr.Length == 0)
            {
                MessageBox.Show("程序需要默认启动项目确定固件类型和启动文件类型\n" +
                    "但是现在没有默认启动项目，请在msconfig中设置默认启动项目");
                Invoke(new SimpleAction(Close));
                return;
            }

            string errorStr = Functions.ParamIsVaild(NASName, NASPath,
                NASUser, NASPassword,
                target, LocalPath,
                IsLocalBackup.ToLower() == BackupAndRestoreConfig.Yes, (string)operation,
                out bool warningOverwrite);
            if (errorStr.Length != 0)
            {
                MessageBox.Show(errorStr);
                Invoke(new SimpleAction(Close));
                return;
            }

            if (warningOverwrite && MessageBox.Show("文件操作，继续备份会覆盖文件", "警告：", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                Invoke(new SimpleAction(Close));
                return;
            }

            PartitionLocation partitionLocation1 = new PartitionLocation(), partitionLocation2 = new PartitionLocation();
            bool callCode1 = Functions.GetPartitionLocation(target.Replace("\\", ""), out partitionLocation1);
            bool callCode2;
            if (LocalPath.Length != 0 && IsLocalBackup.ToLower() == BackupAndRestoreConfig.Yes)
                callCode2 = Functions.GetPartitionLocation(Directory.GetDirectoryRoot(LocalPath).Replace("\\", ""), out partitionLocation2);
            else
                callCode2 = true;
            if (callCode1 && callCode2)
            {
                Guid defaultBootItemGuid = Guid.Empty;
                callCode1 = Functions.ParseGuid(defaultBootItemGuidStr, out defaultBootItemGuid);
                if (callCode1)
                {
                    Guid deviceGuid = Guid.NewGuid(), OSLoaderGuid = Guid.NewGuid();

                    BackupAndRestoreConfig config = new BackupAndRestoreConfig
                    (
                        //硬盘号是0 base，传递给PERunner的时候改为1 base
                        $"{partitionLocation1.diskNumber}:{partitionLocation1.partitionNumber}",
                        NASName,
                        NASPath,
                        NASUser,
                        NASPassword,
                        (string)operation,
                        deviceGuid,
                        OSLoaderGuid,
                        defaultBootItemGuid,
                        FileType,
                        IsLocalBackup,
                        ExtraGhoArgument,
                        ExtraWimArgument,
                        Path.GetFullPath(LocalPath).Replace(Directory.GetDirectoryRoot(LocalPath), ""),
                        $"{partitionLocation2.diskNumber}:{partitionLocation2.partitionNumber}"
                    );
                    config.Write("config.json");

                    //修改WindowsPE，放入备份程序
                    string result = Functions.ModifyWindowsPE(new Action<int>(StepIncrement));
                    if (result != null)
                    {
                        MessageBox.Show(result);
                        Invoke(new SimpleAction(Close));
                        return;
                    }

                    //添加WindowsPE启动项并设置为默认
                    StepIncrement(4);
                    if (!Functions.AddWinPEBootLoader(deviceGuid, OSLoaderGuid))
                    {
                        MessageBox.Show("修改引导配置失败");
                        Invoke(new SimpleAction(Close));
                        return;
                    }

                    //重启电脑
                    Process.Start("shutdown", "/r /t 0");
                    Invoke(new SimpleAction(Close));
                    return;
                }
            }
            //若分区的物理路径，硬盘号，分区号之一获取错误，视为参数错误
            MessageBox.Show("备份参数错误");
            Invoke(new SimpleAction(Close));
        }

        private void PreparationNotice_FormClosed(object sender, FormClosedEventArgs e)
        {
            Invoke(new Action<Control>(EnableControl), fatherForm);
        }

        private void StepIncrement(int step)
        {
            switch (step)
            {
                case 1:
                    Invoke(new Action<Control>(EnableControl), step1);
                    break;
                case 2:
                    Invoke(new Action<Control>(EnableControl), step2);
                    break;
                case 3:
                    Invoke(new Action<Control>(EnableControl), step3);
                    break;
                case 4:
                    Invoke(new Action<Control>(EnableControl), step4);
                    break;
                default:
                    break;
            }
        }

        private void EnableControl(Control control)
        {
            control.Enabled = true;
        }

        Form fatherForm;
        string NASUser;
        string NASPath;
        string NASName;
        string NASPassword;
        string target;
        string FileType;
        string IsLocalBackup;
        string ExtraGhoArgument;
        string ExtraWimArgument;
        string LocalPath;
        string Operation;

        private void PreparationNotice_Load(object sender, EventArgs e)
        {
            Thread taskThread = new Thread(new ParameterizedThreadStart(StartOperate))
            {
                IsBackground = true
            };
            taskThread.Start(Operation);
        }
    }
}
