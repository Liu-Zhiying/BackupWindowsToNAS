namespace BackupOSToNAS
{
    partial class PreparationNotice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.step3 = new System.Windows.Forms.Label();
            this.step2 = new System.Windows.Forms.Label();
            this.step4 = new System.Windows.Forms.Label();
            this.step1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // step3
            // 
            this.step3.AutoSize = true;
            this.step3.Enabled = false;
            this.step3.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.step3.Location = new System.Drawing.Point(12, 100);
            this.step3.Name = "step3";
            this.step3.Size = new System.Drawing.Size(166, 28);
            this.step3.TabIndex = 1;
            this.step3.Text = "3.保存WinPE";
            // 
            // step2
            // 
            this.step2.AutoSize = true;
            this.step2.Enabled = false;
            this.step2.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.step2.Location = new System.Drawing.Point(12, 54);
            this.step2.Name = "step2";
            this.step2.Size = new System.Drawing.Size(166, 28);
            this.step2.TabIndex = 2;
            this.step2.Text = "2.修改WinPE";
            // 
            // step4
            // 
            this.step4.AutoSize = true;
            this.step4.Enabled = false;
            this.step4.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.step4.Location = new System.Drawing.Point(12, 145);
            this.step4.Name = "step4";
            this.step4.Size = new System.Drawing.Size(152, 28);
            this.step4.TabIndex = 3;
            this.step4.Text = "4.设置引导";
            // 
            // step1
            // 
            this.step1.AutoSize = true;
            this.step1.Enabled = false;
            this.step1.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.step1.Location = new System.Drawing.Point(12, 9);
            this.step1.Name = "step1";
            this.step1.Size = new System.Drawing.Size(166, 28);
            this.step1.TabIndex = 0;
            this.step1.Text = "1.挂载WinPE";
            // 
            // PreparationNotice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 223);
            this.ControlBox = false;
            this.Controls.Add(this.step4);
            this.Controls.Add(this.step2);
            this.Controls.Add(this.step3);
            this.Controls.Add(this.step1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PreparationNotice";
            this.Text = "PreparationNotice";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PreparationNotice_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label step3;
        private System.Windows.Forms.Label step2;
        private System.Windows.Forms.Label step4;
        private System.Windows.Forms.Label step1;
    }
}