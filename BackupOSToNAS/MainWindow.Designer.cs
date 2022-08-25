namespace BackupOSToNAS
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PartitionLabel = new System.Windows.Forms.Label();
            this.partitionComboBox = new System.Windows.Forms.ComboBox();
            this.NASNameLabel = new System.Windows.Forms.Label();
            this.NASNameBox = new System.Windows.Forms.TextBox();
            this.NASPathLabel = new System.Windows.Forms.Label();
            this.NASPathBox = new System.Windows.Forms.TextBox();
            this.TipLabel1 = new System.Windows.Forms.Label();
            this.NASPasswordLabel = new System.Windows.Forms.Label();
            this.NASUserLabel = new System.Windows.Forms.Label();
            this.NASPasswordBox = new System.Windows.Forms.TextBox();
            this.NASUserBox = new System.Windows.Forms.TextBox();
            this.restoreBtn = new System.Windows.Forms.Button();
            this.backupBtn = new System.Windows.Forms.Button();
            this.lookupNASBtn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.fileTypeBox = new System.Windows.Forms.ComboBox();
            this.backupFileTypeLabel = new System.Windows.Forms.Label();
            this.argLabel = new System.Windows.Forms.Label();
            this.argsBox = new System.Windows.Forms.TextBox();
            this.useLocalBox = new System.Windows.Forms.CheckBox();
            this.localPathBox = new System.Windows.Forms.TextBox();
            this.lookupLocalBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PartitionLabel
            // 
            this.PartitionLabel.AutoSize = true;
            this.PartitionLabel.Location = new System.Drawing.Point(16, 12);
            this.PartitionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PartitionLabel.Name = "PartitionLabel";
            this.PartitionLabel.Size = new System.Drawing.Size(52, 15);
            this.PartitionLabel.TabIndex = 0;
            this.PartitionLabel.Text = "分区：";
            // 
            // partitionComboBox
            // 
            this.partitionComboBox.FormattingEnabled = true;
            this.partitionComboBox.Location = new System.Drawing.Point(111, 8);
            this.partitionComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.partitionComboBox.Name = "partitionComboBox";
            this.partitionComboBox.Size = new System.Drawing.Size(219, 23);
            this.partitionComboBox.TabIndex = 1;
            // 
            // NASNameLabel
            // 
            this.NASNameLabel.AutoSize = true;
            this.NASNameLabel.Location = new System.Drawing.Point(338, 12);
            this.NASNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASNameLabel.Name = "NASNameLabel";
            this.NASNameLabel.Size = new System.Drawing.Size(76, 15);
            this.NASNameLabel.TabIndex = 2;
            this.NASNameLabel.Text = "NAS名称：";
            // 
            // NASNameBox
            // 
            this.NASNameBox.Location = new System.Drawing.Point(426, 6);
            this.NASNameBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NASNameBox.Name = "NASNameBox";
            this.NASNameBox.Size = new System.Drawing.Size(223, 25);
            this.NASNameBox.TabIndex = 2;
            // 
            // NASPathLabel
            // 
            this.NASPathLabel.AutoSize = true;
            this.NASPathLabel.Location = new System.Drawing.Point(16, 40);
            this.NASPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASPathLabel.Name = "NASPathLabel";
            this.NASPathLabel.Size = new System.Drawing.Size(76, 15);
            this.NASPathLabel.TabIndex = 4;
            this.NASPathLabel.Text = "NAS路径：";
            // 
            // NASPathBox
            // 
            this.NASPathBox.Location = new System.Drawing.Point(111, 37);
            this.NASPathBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NASPathBox.Name = "NASPathBox";
            this.NASPathBox.Size = new System.Drawing.Size(136, 25);
            this.NASPathBox.TabIndex = 3;
            // 
            // TipLabel1
            // 
            this.TipLabel1.AutoSize = true;
            this.TipLabel1.Location = new System.Drawing.Point(338, 40);
            this.TipLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TipLabel1.Name = "TipLabel1";
            this.TipLabel1.Size = new System.Drawing.Size(296, 15);
            this.TipLabel1.TabIndex = 6;
            this.TipLabel1.Text = "（使用Windows路径格式,包括备份文件名）";
            // 
            // NASPasswordLabel
            // 
            this.NASPasswordLabel.AutoSize = true;
            this.NASPasswordLabel.Location = new System.Drawing.Point(338, 71);
            this.NASPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASPasswordLabel.Name = "NASPasswordLabel";
            this.NASPasswordLabel.Size = new System.Drawing.Size(76, 15);
            this.NASPasswordLabel.TabIndex = 7;
            this.NASPasswordLabel.Text = "NAS密码：";
            // 
            // NASUserLabel
            // 
            this.NASUserLabel.AutoSize = true;
            this.NASUserLabel.Location = new System.Drawing.Point(16, 71);
            this.NASUserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASUserLabel.Name = "NASUserLabel";
            this.NASUserLabel.Size = new System.Drawing.Size(76, 15);
            this.NASUserLabel.TabIndex = 8;
            this.NASUserLabel.Text = "NAS用户：";
            // 
            // NASPasswordBox
            // 
            this.NASPasswordBox.Location = new System.Drawing.Point(426, 68);
            this.NASPasswordBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NASPasswordBox.Name = "NASPasswordBox";
            this.NASPasswordBox.Size = new System.Drawing.Size(225, 25);
            this.NASPasswordBox.TabIndex = 6;
            // 
            // NASUserBox
            // 
            this.NASUserBox.Location = new System.Drawing.Point(111, 68);
            this.NASUserBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.NASUserBox.Name = "NASUserBox";
            this.NASUserBox.Size = new System.Drawing.Size(219, 25);
            this.NASUserBox.TabIndex = 5;
            // 
            // restoreBtn
            // 
            this.restoreBtn.Location = new System.Drawing.Point(551, 172);
            this.restoreBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.restoreBtn.Name = "restoreBtn";
            this.restoreBtn.Size = new System.Drawing.Size(100, 28);
            this.restoreBtn.TabIndex = 13;
            this.restoreBtn.Text = "还原";
            this.restoreBtn.UseVisualStyleBackColor = true;
            this.restoreBtn.Click += new System.EventHandler(this.restoreBtn_Click);
            // 
            // backupBtn
            // 
            this.backupBtn.Location = new System.Drawing.Point(13, 172);
            this.backupBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.backupBtn.Name = "backupBtn";
            this.backupBtn.Size = new System.Drawing.Size(100, 28);
            this.backupBtn.TabIndex = 12;
            this.backupBtn.Text = "备份";
            this.backupBtn.UseVisualStyleBackColor = true;
            this.backupBtn.Click += new System.EventHandler(this.backupBtn_Click);
            // 
            // lookupNASBtn
            // 
            this.lookupNASBtn.Location = new System.Drawing.Point(254, 37);
            this.lookupNASBtn.Name = "lookupNASBtn";
            this.lookupNASBtn.Size = new System.Drawing.Size(75, 25);
            this.lookupNASBtn.TabIndex = 4;
            this.lookupNASBtn.Text = "浏览";
            this.lookupNASBtn.UseVisualStyleBackColor = true;
            this.lookupNASBtn.Click += new System.EventHandler(this.lookupBtn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // fileTypeBox
            // 
            this.fileTypeBox.FormattingEnabled = true;
            this.fileTypeBox.Items.AddRange(new object[] {
            "gho",
            "wim"});
            this.fileTypeBox.Location = new System.Drawing.Point(111, 99);
            this.fileTypeBox.Name = "fileTypeBox";
            this.fileTypeBox.Size = new System.Drawing.Size(219, 23);
            this.fileTypeBox.TabIndex = 7;
            this.fileTypeBox.SelectionChangeCommitted += new System.EventHandler(this.fileTypeBox_SelectionChangeCommitted);
            // 
            // backupFileTypeLabel
            // 
            this.backupFileTypeLabel.AutoSize = true;
            this.backupFileTypeLabel.Location = new System.Drawing.Point(16, 102);
            this.backupFileTypeLabel.Name = "backupFileTypeLabel";
            this.backupFileTypeLabel.Size = new System.Drawing.Size(82, 15);
            this.backupFileTypeLabel.TabIndex = 15;
            this.backupFileTypeLabel.Text = "文件类型：";
            // 
            // argLabel
            // 
            this.argLabel.AutoSize = true;
            this.argLabel.Location = new System.Drawing.Point(338, 102);
            this.argLabel.Name = "argLabel";
            this.argLabel.Size = new System.Drawing.Size(82, 15);
            this.argLabel.TabIndex = 16;
            this.argLabel.Text = "额外参数：";
            // 
            // argsBox
            // 
            this.argsBox.Location = new System.Drawing.Point(426, 99);
            this.argsBox.Name = "argsBox";
            this.argsBox.Size = new System.Drawing.Size(223, 25);
            this.argsBox.TabIndex = 8;
            // 
            // useLocalBox
            // 
            this.useLocalBox.AutoSize = true;
            this.useLocalBox.Location = new System.Drawing.Point(12, 137);
            this.useLocalBox.Name = "useLocalBox";
            this.useLocalBox.Size = new System.Drawing.Size(127, 19);
            this.useLocalBox.TabIndex = 9;
            this.useLocalBox.Text = "本地备份/还原";
            this.useLocalBox.UseVisualStyleBackColor = true;
            this.useLocalBox.CheckedChanged += new System.EventHandler(this.useLocalBox_CheckedChanged);
            // 
            // localPathBox
            // 
            this.localPathBox.Enabled = false;
            this.localPathBox.Location = new System.Drawing.Point(145, 135);
            this.localPathBox.Name = "localPathBox";
            this.localPathBox.Size = new System.Drawing.Size(419, 25);
            this.localPathBox.TabIndex = 10;
            // 
            // lookupLocalBtn
            // 
            this.lookupLocalBtn.Enabled = false;
            this.lookupLocalBtn.Location = new System.Drawing.Point(577, 137);
            this.lookupLocalBtn.Name = "lookupLocalBtn";
            this.lookupLocalBtn.Size = new System.Drawing.Size(75, 23);
            this.lookupLocalBtn.TabIndex = 11;
            this.lookupLocalBtn.Text = "浏览";
            this.lookupLocalBtn.UseVisualStyleBackColor = true;
            this.lookupLocalBtn.Click += new System.EventHandler(this.lookupLocalBtn_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 212);
            this.Controls.Add(this.lookupLocalBtn);
            this.Controls.Add(this.localPathBox);
            this.Controls.Add(this.useLocalBox);
            this.Controls.Add(this.argsBox);
            this.Controls.Add(this.argLabel);
            this.Controls.Add(this.backupFileTypeLabel);
            this.Controls.Add(this.fileTypeBox);
            this.Controls.Add(this.lookupNASBtn);
            this.Controls.Add(this.backupBtn);
            this.Controls.Add(this.restoreBtn);
            this.Controls.Add(this.NASUserBox);
            this.Controls.Add(this.NASPasswordBox);
            this.Controls.Add(this.NASUserLabel);
            this.Controls.Add(this.NASPasswordLabel);
            this.Controls.Add(this.TipLabel1);
            this.Controls.Add(this.NASPathBox);
            this.Controls.Add(this.NASPathLabel);
            this.Controls.Add(this.NASNameBox);
            this.Controls.Add(this.NASNameLabel);
            this.Controls.Add(this.partitionComboBox);
            this.Controls.Add(this.PartitionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "BackupOSToNAS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PartitionLabel;
        private System.Windows.Forms.ComboBox partitionComboBox;
        private System.Windows.Forms.Label NASNameLabel;
        private System.Windows.Forms.TextBox NASNameBox;
        private System.Windows.Forms.Label NASPathLabel;
        private System.Windows.Forms.TextBox NASPathBox;
        private System.Windows.Forms.Label TipLabel1;
        private System.Windows.Forms.Label NASPasswordLabel;
        private System.Windows.Forms.Label NASUserLabel;
        private System.Windows.Forms.TextBox NASPasswordBox;
        private System.Windows.Forms.TextBox NASUserBox;
        private System.Windows.Forms.Button restoreBtn;
        private System.Windows.Forms.Button backupBtn;
        private System.Windows.Forms.Button lookupNASBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox fileTypeBox;
        private System.Windows.Forms.Label backupFileTypeLabel;
        private System.Windows.Forms.Label argLabel;
        private System.Windows.Forms.TextBox argsBox;
        private System.Windows.Forms.CheckBox useLocalBox;
        private System.Windows.Forms.TextBox localPathBox;
        private System.Windows.Forms.Button lookupLocalBtn;
    }
}

