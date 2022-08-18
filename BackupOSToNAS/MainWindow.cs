using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            //获取所有盘符
            List<string> ps = Functions.GetCurrentMountPoints();
            partitionComboBox.Items.AddRange(ps.ToArray());
        }
        //备份和还原的操作只是config一个字段的不同
        //所以这个过程单独写一个方法被按钮事件函数调用
        private void StartOperate(string operate)
        {
            string defaultBootItemGuidStr = Functions.GetBootDefaultItem();
            if (defaultBootItemGuidStr.Length == 0)
            {
                MessageBox.Show("程序需要默认启动项目确定固件类型和启动文件类型\n" +
                    "但是现在没有默认启动项目，请在msconfig中设置默认启动项目");
                return;
            }

            //检查用户是否输入必须的参数
            if (!CheckUserInput())
                return;

            //获取要备份分区
            string targetDevice = (string)partitionComboBox.SelectedItem;
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
                            NASNameBox.Text,
                            NASPathBox.Text,
                            NASUserBox.Text,
                            NASPasswordBox.Text,
                            operate,
                            deviceGuid,
                            OSLoaderGuid,
                            defaultBootItemGuid
                        );
                        config.Write("config.json");

                        //修改WindowsPE，放入备份程序
                        string result = Functions.ModifyWindowsPE();
                        if (result != null)
                        {
                            MessageBox.Show(result);
                            return;
                        }

                        //添加WindowsPE启动项并设置为默认
                        if (!Functions.AddWinPEBootLoader(deviceGuid, OSLoaderGuid))
                        {
                            MessageBox.Show("修改引导配置失败");
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
        private void backupBtn_Click(object sender, EventArgs e)
        {
            StartOperate(BackupAndRestoreConfig.BackupOperation);  
        }
        private void restoreBtn_Click(object sender, EventArgs e)
        {
            StartOperate(BackupAndRestoreConfig.RestoreOperation);
        }
        //检查用户有没有输入必须项目
        private bool CheckUserInput()
        {
            if (partitionComboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择源分区");
                return false;
            }
            if (NASNameBox.Text.Length == 0)
            {
                MessageBox.Show("请填写NAS名称");
                return false;
            }
            if (NASPathBox.Text.Length == 0)
            {
                MessageBox.Show("请填写NAS路径");
                return false;
            }
            if (NASUserBox.Text.Length == 0)
            {
                MessageBox.Show("请填写NAS用户");
                return false;
            }
            return true;
        }
    }
}