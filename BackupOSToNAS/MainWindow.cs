using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BackupOSToNAS
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            ExtraWimArgument = "";
            ExtraGhoArgument = "-sure";
            fileTypeIndex = fileTypeBox.SelectedIndex;
            //获取所有盘符
            List<string> ps = Functions.GetCurrentMountPoints();
            partitionComboBox.Items.AddRange(ps.ToArray());
            try
            {
                BackupAndRestoreConfig config = new BackupAndRestoreConfig();
                config.Read("config.json");
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.NASNameProperty, out string value);
                NASNameBox.Text = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.NASPathProperty, out value);
                NASPathBox.Text = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.NASUserProperty, out value);
                NASUserBox.Text = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.NASPasswordProperty, out value);
                NASPasswordBox.Text = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.TargetProperty, out value);
                string[] numStrs = value.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                int diskNum = Convert.ToInt32(numStrs[0]);
                int partitionNum = Convert.ToInt32(numStrs[1]);
                partitionComboBox.SelectedItem = Functions.QueryMountPointByPartitionLocation(diskNum, partitionNum);
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.IsLocalBackupProperty, out value);
                useLocalBox.Checked = value.ToLower() == BackupAndRestoreConfig.Yes;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.ExtraGhoArgumentProperty, out value);
                ExtraGhoArgument = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.ExtraWimArgumentProperty, out value);
                ExtraWimArgument = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.BackupFileTypeProperty, out value);
                switch (value.ToLower())
                {
                    case "gho":
                        argsBox.Text = ExtraGhoArgument;
                        break;
                    case "wim":
                        argsBox.Text = ExtraWimArgument;
                        break;
                }
                fileTypeBox.SelectedItem = value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.LocalDeviceProperty, out value);
                string[] temp = value.Split(':');
                string mountPoint = Functions.QueryMountPointByPartitionLocation(Convert.ToInt32(temp[0]), Convert.ToInt32(temp[1]));
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.LocalPathProperty, out value);
                string LocalFullPath = mountPoint + value;
                localPathBox.Text = LocalFullPath;
            }
            catch
            {

            }
            fileTypeIndex = fileTypeBox.SelectedIndex;
        }
        private void backupBtn_Click(object sender, EventArgs e)
        {
            //检查用户是否输入必须的参数
            if (!CheckUserInput())
            {
                return;
            }
            new PreparationNotice
            (
                this,
                partitionComboBox.Text,
                BackupAndRestoreConfig.BackupOperation,
                NASNameBox.Text,
                Path.GetExtension(NASPathBox.Text).ToLower() == $".{fileTypeBox.SelectedItem}" ? NASPathBox.Text : NASPathBox.Text + $".{fileTypeBox.SelectedItem}",
                NASUserBox.Text,
                NASPasswordBox.Text,
                (string)fileTypeBox.SelectedItem,
                useLocalBox.Checked ? BackupAndRestoreConfig.Yes : BackupAndRestoreConfig.No,
                fileTypeBox.SelectedIndex != 0 ? ExtraGhoArgument : argsBox.Text,
                fileTypeBox.SelectedIndex != 1 ? ExtraWimArgument : argsBox.Text,
                Path.GetExtension(localPathBox.Text).ToLower() == $".{fileTypeBox.SelectedItem}" ? localPathBox.Text : localPathBox.Text + $".{fileTypeBox.SelectedItem}"
            ).Show();
        }
        private void restoreBtn_Click(object sender, EventArgs e)
        {
            //检查用户是否输入必须的参数
            if (!CheckUserInput())
            {
                return;
            }
            new PreparationNotice
            (
                this,
                partitionComboBox.Text,
                BackupAndRestoreConfig.RestoreOperation,
                NASNameBox.Text,
                Path.GetExtension(NASPathBox.Text).ToLower() == $".{fileTypeBox.SelectedItem}" ? NASPathBox.Text : NASPathBox.Text + $".{fileTypeBox.SelectedItem}",
                NASUserBox.Text,
                NASPasswordBox.Text,
                (string)fileTypeBox.SelectedItem,
                useLocalBox.Checked ? BackupAndRestoreConfig.Yes : BackupAndRestoreConfig.No,
                fileTypeBox.SelectedIndex != 0 ? ExtraGhoArgument : argsBox.Text,
                fileTypeBox.SelectedIndex != 1 ? ExtraWimArgument : argsBox.Text,
                Path.GetExtension(localPathBox.Text).ToLower() == $".{fileTypeBox.SelectedItem}" ? localPathBox.Text : localPathBox.Text + $".{fileTypeBox.SelectedItem}"
            ).Show();
        }

        //检查用户有没有输入必须项目
        private bool CheckUserInput()
        {
            if (partitionComboBox.SelectedItem == null)
            {
                MessageBox.Show("请选择源分区");
                return false;
            }
            if (fileTypeBox.SelectedItem == null)
            {
                MessageBox.Show("请选择文件类型");
                return false;
            }
            if (!useLocalBox.Checked)
            {
                if (NASNameBox.Text == null || NASNameBox.Text.Length == 0)
                {
                    MessageBox.Show("请填写NAS名称");
                    return false;
                }
                if (NASPathBox.Text == null || NASPathBox.Text.Length == 0)
                {
                    MessageBox.Show("请填写NAS路径");
                    return false;
                }
                if (NASUserBox.Text == null || NASUserBox.Text.Length == 0)
                {
                    MessageBox.Show("请填写NAS用户");
                    return false;
                }
            }
            else
            {
                if (localPathBox.Text == null || localPathBox.Text.Length == 0)
                    return false;
            }
            return true;
        }

        private void lookupBtn_Click(object sender, EventArgs e)
        {
            if (NASNameBox.Text == null || NASNameBox.Text.Length == 0)
            {
                MessageBox.Show("请填写NAS名称");
                return;
            }
            if (NASUserBox.Text == null || NASUserBox.Text.Length == 0)
            {
                MessageBox.Show("请填写NAS用户");
                return;
            }
            if (fileTypeBox.SelectedItem == null)
            {
                MessageBox.Show("请选择文件类型");
                return;
            }
            string NASSharedFolderName;
            if (NASPathBox.Text.IndexOf('\\') == -1)
                NASSharedFolderName = NASPathBox.Text;
            else
                NASSharedFolderName = NASPathBox.Text.Substring(0, NASPathBox.Text.IndexOf('\\'));
            string NASConnectCmd;
            if (NASPathBox.Text.Length != 0)
                NASConnectCmd = $"/C \"net use \"\\\\{NASNameBox.Text}\\{NASSharedFolderName}\" {NASPasswordBox.Text} /user:\"{NASUserBox.Text}\"\"";
            else
                NASConnectCmd = $"/C \"net use \"\\\\{NASNameBox.Text}\" {NASPasswordBox.Text} /user:\"{NASUserBox.Text}\"\"";
            
            ProcessStartInfo info = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = "cmd.exe",
                Arguments = NASConnectCmd
            };

            Process process = Process.Start(info);
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                MessageBox.Show("NAS参数错误！");
                return;
            }

            string dirPath = NASPathBox.Text;
            for (int index = dirPath.Length - 1; index > -1; --index)
            {
                if (dirPath[index] == '\\')
                {
                    dirPath = dirPath.Substring(0, index);
                    break;
                }
            }

            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.InitialDirectory = $"\\\\{NASNameBox.Text}\\{dirPath}";
            openFileDialog.Filter = $"(*.{fileTypeBox.SelectedItem})|*.{fileTypeBox.SelectedItem};";
            openFileDialog.AddExtension = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                NASPathBox.Text = openFileDialog.FileName;
                NASPathBox.Text = NASPathBox.Text.Replace($"\\\\{NASNameBox.Text}\\", "");
            }

            info.Arguments = $"/C \"net use * /delete /y\"";
            process = Process.Start(info);
            process.WaitForExit();
        }

        private void useLocalBox_CheckedChanged(object sender, EventArgs e)
        {
            if(useLocalBox.Checked)
            {
                NASNameBox.Enabled = false;
                NASPasswordBox.Enabled = false;
                NASUserBox.Enabled = false;
                NASPathBox.Enabled = false;
                lookupNASBtn.Enabled = false;
                localPathBox.Enabled = true;
                lookupLocalBtn.Enabled = true;
            }
            else
            {
                NASNameBox.Enabled = true;
                NASPasswordBox.Enabled = true;
                NASUserBox.Enabled = true;
                NASPathBox.Enabled = true;
                lookupNASBtn.Enabled = true;
                localPathBox.Enabled = false;
                lookupLocalBtn.Enabled = false;
            }
        }

        private void lookupLocalBtn_Click(object sender, EventArgs e)
        {
            if (fileTypeBox.SelectedItem == null)
            {
                MessageBox.Show("请选择文件类型");
                return;
            }

            openFileDialog.CheckFileExists = false;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Filter = $"(*.{fileTypeBox.SelectedItem})|*.{fileTypeBox.SelectedItem};";
            openFileDialog.AddExtension = false;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                localPathBox.Text = openFileDialog.FileName;
            }
        }
        string ExtraGhoArgument;
        string ExtraWimArgument;
        int fileTypeIndex;

        private void fileTypeBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch(fileTypeIndex)
            {
                case 0:
                    ExtraGhoArgument = argsBox.Text;
                    break;
                case 1:
                    ExtraWimArgument = argsBox.Text;
                    break;
            }

            switch (fileTypeBox.SelectedIndex)
            {
                case 0:
                    argsBox.Text = ExtraGhoArgument;
                    break;
                case 1:
                    argsBox.Text = ExtraWimArgument;
                    break;
            }

            fileTypeIndex = fileTypeBox.SelectedIndex;

        }
    }
}