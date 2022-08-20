using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    public partial class PreparationNotice : Form
    {
        public PreparationNotice(Form fatherForm, string target, string operation, string NASName, string NASPath, string NASUser, string NASPassword)
        {
            InitializeComponent();
            this.fatherForm = fatherForm;
            fatherForm.Enabled = false;
            step = -1;
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
                Invoke(new Action(Close));
                return;
            }

            if(!Functions.ParamIsVaild(NASName,NASPath,NASUser,NASPassword))
            {
                MessageBox.Show("NAS参数设置错误");
                Invoke(new Action(Close));
                return;
            }

            //获取要备份分区
            string targetDevice = target;
            StringBuilder targetPhysicalPathBuilder = new StringBuilder(Functions.GetPhysicalPath(targetDevice.Substring(0, targetDevice.Length - 1)));
            //获取备份分区的物理路径和硬盘号和分区号
            if (targetPhysicalPathBuilder.Length != 0)
            {
                targetPhysicalPathBuilder.Replace("\\Device\\", "\\\\.\\");

                PartitionLocation partitionLocation = new PartitionLocation();
                bool callCode = Functions.GetPartitionLocation(targetPhysicalPathBuilder.ToString(), out partitionLocation);

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
                            $"{partitionLocation.diskNumber + 1}:{partitionLocation.partitionNumber}",
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
                        string result = Functions.ModifyWindowsPE(new Action(StepIncrement));
                        if (result != null)
                        {
                            MessageBox.Show(result);
                            Invoke(new Action(Close));
                            return;
                        }

                        //添加WindowsPE启动项并设置为默认
                        StepIncrement();
                        if (!Functions.AddWinPEBootLoader(deviceGuid, OSLoaderGuid))
                        {
                            MessageBox.Show("修改引导配置失败");
                            Invoke(new Action(Close));
                            return;
                        }

                        //重启电脑
                        Process.Start("shutdown", "/r /t 0");
                        return;
                    }
                }
            }
            //若分区的物理路径，硬盘号，分区号之一获取错误，视为参数错误
            MessageBox.Show("备份参数错误");
        }

        private void PreparationNotice_FormClosed(object sender, FormClosedEventArgs e)
        {
            fatherForm.Enabled = true;
        }

        private void StepIncrement()
        {
            lock(this)
            {
                ++step;
                switch (step)
                {
                    case 0:
                        step1.Enabled = true;
                        break;
                    case 1:
                        step2.Enabled = true;
                        break;
                    case 2:
                        step3.Enabled = true;
                        break;
                    case 3:
                        step4.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        Form fatherForm;
        int step;
        string NASUser;
        string NASPath;
        string NASName;
        string NASPassword;
        string target;
    }
}
