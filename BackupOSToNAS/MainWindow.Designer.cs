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
            this.SuspendLayout();
            // 
            // PartitionLabel
            // 
            this.PartitionLabel.AutoSize = true;
            this.PartitionLabel.Location = new System.Drawing.Point(18, 14);
            this.PartitionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PartitionLabel.Name = "PartitionLabel";
            this.PartitionLabel.Size = new System.Drawing.Size(62, 18);
            this.PartitionLabel.TabIndex = 0;
            this.PartitionLabel.Text = "分区：";
            // 
            // partitionComboBox
            // 
            this.partitionComboBox.FormattingEnabled = true;
            this.partitionComboBox.Location = new System.Drawing.Point(106, 9);
            this.partitionComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.partitionComboBox.Name = "partitionComboBox";
            this.partitionComboBox.Size = new System.Drawing.Size(180, 26);
            this.partitionComboBox.TabIndex = 1;
            // 
            // NASNameLabel
            // 
            this.NASNameLabel.AutoSize = true;
            this.NASNameLabel.Location = new System.Drawing.Point(297, 14);
            this.NASNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASNameLabel.Name = "NASNameLabel";
            this.NASNameLabel.Size = new System.Drawing.Size(89, 18);
            this.NASNameLabel.TabIndex = 2;
            this.NASNameLabel.Text = "NAS名称：";
            // 
            // NASNameBox
            // 
            this.NASNameBox.Location = new System.Drawing.Point(394, 9);
            this.NASNameBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NASNameBox.Name = "NASNameBox";
            this.NASNameBox.Size = new System.Drawing.Size(274, 28);
            this.NASNameBox.TabIndex = 3;
            // 
            // NASPathLabel
            // 
            this.NASPathLabel.AutoSize = true;
            this.NASPathLabel.Location = new System.Drawing.Point(18, 54);
            this.NASPathLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASPathLabel.Name = "NASPathLabel";
            this.NASPathLabel.Size = new System.Drawing.Size(89, 18);
            this.NASPathLabel.TabIndex = 4;
            this.NASPathLabel.Text = "NAS路径：";
            // 
            // NASPathBox
            // 
            this.NASPathBox.Location = new System.Drawing.Point(106, 50);
            this.NASPathBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NASPathBox.Name = "NASPathBox";
            this.NASPathBox.Size = new System.Drawing.Size(180, 28);
            this.NASPathBox.TabIndex = 5;
            // 
            // TipLabel1
            // 
            this.TipLabel1.AutoSize = true;
            this.TipLabel1.Location = new System.Drawing.Point(297, 54);
            this.TipLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TipLabel1.Name = "TipLabel1";
            this.TipLabel1.Size = new System.Drawing.Size(386, 18);
            this.TipLabel1.TabIndex = 6;
            this.TipLabel1.Text = "（路径使用Windows路径格式,包括备份文件名）";
            // 
            // NASPasswordLabel
            // 
            this.NASPasswordLabel.AutoSize = true;
            this.NASPasswordLabel.Location = new System.Drawing.Point(297, 94);
            this.NASPasswordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASPasswordLabel.Name = "NASPasswordLabel";
            this.NASPasswordLabel.Size = new System.Drawing.Size(89, 18);
            this.NASPasswordLabel.TabIndex = 7;
            this.NASPasswordLabel.Text = "NAS密码：";
            // 
            // NASUserLabel
            // 
            this.NASUserLabel.AutoSize = true;
            this.NASUserLabel.Location = new System.Drawing.Point(18, 94);
            this.NASUserLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.NASUserLabel.Name = "NASUserLabel";
            this.NASUserLabel.Size = new System.Drawing.Size(89, 18);
            this.NASUserLabel.TabIndex = 8;
            this.NASUserLabel.Text = "NAS用户：";
            // 
            // NASPasswordBox
            // 
            this.NASPasswordBox.Location = new System.Drawing.Point(394, 88);
            this.NASPasswordBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NASPasswordBox.Name = "NASPasswordBox";
            this.NASPasswordBox.Size = new System.Drawing.Size(274, 28);
            this.NASPasswordBox.TabIndex = 9;
            // 
            // NASUserBox
            // 
            this.NASUserBox.Location = new System.Drawing.Point(106, 88);
            this.NASUserBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NASUserBox.Name = "NASUserBox";
            this.NASUserBox.Size = new System.Drawing.Size(180, 28);
            this.NASUserBox.TabIndex = 10;
            // 
            // restoreBtn
            // 
            this.restoreBtn.Location = new System.Drawing.Point(558, 144);
            this.restoreBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.restoreBtn.Name = "restoreBtn";
            this.restoreBtn.Size = new System.Drawing.Size(112, 34);
            this.restoreBtn.TabIndex = 11;
            this.restoreBtn.Text = "还原";
            this.restoreBtn.UseVisualStyleBackColor = true;
            this.restoreBtn.Click += new System.EventHandler(this.restoreBtn_Click);
            // 
            // backupBtn
            // 
            this.backupBtn.Location = new System.Drawing.Point(21, 144);
            this.backupBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.backupBtn.Name = "backupBtn";
            this.backupBtn.Size = new System.Drawing.Size(112, 34);
            this.backupBtn.TabIndex = 12;
            this.backupBtn.Text = "备份";
            this.backupBtn.UseVisualStyleBackColor = true;
            this.backupBtn.Click += new System.EventHandler(this.backupBtn_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 196);
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
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
    }
}

