using System;
using System.Collections.Generic;
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
        private void backupBtn_Click(object sender, EventArgs e)
        {
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
                    Guid defaultBootItemGuid = new Guid();
                    callCode = Functions.ParseGuid(Functions.GetBootDefaultItem(), out defaultBootItemGuid);
                    NASUserBox.Text = defaultBootItemGuid.ToString();
                    if (callCode)
                    {
                        Guid deviceGuid = Guid.NewGuid(), OSLoaderGuid = Guid.NewGuid();
                        BackupToNASConfig config = new BackupToNASConfig
                        (
                            //硬盘号是0 base，传递给PERunner的时候改为1 base
                            $"{partitionLocation.diskNumber + 1}:{partitionLocation.partitionNumber}",
                            NASNameBox.Text,
                            NASPathBox.Text,
                            NASUserBox.Text,
                            NASPasswordBox.Text,
                            BackupToNASConfig.BackupOperation,
                            deviceGuid,
                            OSLoaderGuid,
                            defaultBootItemGuid
                        );
                        config.Write("config.json");
                        return;
                    }
                }
            }
            //若分区的物理路径，硬盘号，分区号之一获取错误，视为参数错误
            MessageBox.Show("备份参数错误");
        }
        private void restoreBtn_Click(object sender, EventArgs e)
        {
            if (!CheckUserInput())
                return;
        }
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
