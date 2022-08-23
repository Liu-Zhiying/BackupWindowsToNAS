using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    public partial class PreparationNotice : Form
    {
        //为了支持.NET Framework 3.0，建立的委托类型
        delegate void SimpleAction();
        public PreparationNotice(Form fatherForm, string target, string operation, string NASName, string NASPath, string NASUser, string NASPassword)
        {
            InitializeComponent();
            this.fatherForm = fatherForm;
            fatherForm.Enabled = false;
            this.NASName = NASName;
            this.NASPath = NASPath;
            this.NASPassword = NASPassword;
            this.NASUser = NASUser;
            this.target = target;

            Thread taskThread = new Thread(new ParameterizedThreadStart(StartOperate))
            {
                IsBackground = true
            };
            taskThread.Start(operation);
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

            if (!Functions.ParamIsVaild(NASName, NASPath, NASUser, NASPassword))
            {
                MessageBox.Show("NAS参数设置错误");
                Invoke(new SimpleAction(Close));
                return;
            }

            PartitionLocation partitionLocation = new PartitionLocation();
            bool callCode = Functions.GetPartitionLocation(target.Replace("\\", ""), out partitionLocation);

            if (callCode)
            {
                Guid defaultBootItemGuid = Guid.Empty;
                callCode = Functions.ParseGuid(defaultBootItemGuidStr, out defaultBootItemGuid);
                if (callCode)
                {
                    Guid deviceGuid = Guid.NewGuid(), OSLoaderGuid = Guid.NewGuid();
                    BackupAndRestoreConfig config = new BackupAndRestoreConfig
                    (
                        //硬盘号是0 base，传递给PERunner的时候改为1 base
                        $"{partitionLocation.diskNumber}:{partitionLocation.partitionNumber}",
                        NASName,
                        NASPath,
                        NASUser,
                        NASPassword,
                        (string)operation,
                        deviceGuid,
                        OSLoaderGuid,
                        defaultBootItemGuid
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
    }
}
