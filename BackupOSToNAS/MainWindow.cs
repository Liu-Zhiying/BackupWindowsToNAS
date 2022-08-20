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
            //获取所有盘符
            List<string> ps = Functions.GetCurrentMountPoints();
            partitionComboBox.Items.AddRange(ps.ToArray());
            try
            {
                BackupAndRestoreConfig config = new BackupAndRestoreConfig();
                config.Read("config.json");
                string value;
                config.KeyValues.TryGetValue(BackupAndRestoreConfig.NASNameProperty, out value);
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
            }
            catch
            {

            }
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
                NASPathBox.Text,
                NASUserBox.Text,
                NASPasswordBox.Text
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
                NASPathBox.Text,
                NASUserBox.Text,
                NASPasswordBox.Text
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
            string NASSharedFolderName = null;
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

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                NASPathBox.Text = openFileDialog.FileName;
                NASPathBox.Text = NASPathBox.Text.Replace($"\\\\{NASNameBox.Text}\\", "");
            }

            info.Arguments = $"/C \"net use * /delete /y\"";
            process = Process.Start(info);
            process.WaitForExit();
        }
    }
}