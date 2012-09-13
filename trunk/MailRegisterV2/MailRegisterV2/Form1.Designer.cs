namespace MailRegisterV2
{
    partial class MainFrm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.wb = new Awesomium.Windows.Forms.WebControl();
            this.picBox_Vcode = new System.Windows.Forms.PictureBox();
            this.btn_GetVcode = new System.Windows.Forms.Button();
            this.tb_Vcode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Vcode)).BeginInit();
            this.SuspendLayout();
            // 
            // wb
            // 
            this.wb.Dock = System.Windows.Forms.DockStyle.Top;
            this.wb.Location = new System.Drawing.Point(0, 0);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(1008, 642);
            this.wb.TabIndex = 0;
            // 
            // picBox_Vcode
            // 
            this.picBox_Vcode.Location = new System.Drawing.Point(8, 674);
            this.picBox_Vcode.Name = "picBox_Vcode";
            this.picBox_Vcode.Size = new System.Drawing.Size(100, 50);
            this.picBox_Vcode.TabIndex = 1;
            this.picBox_Vcode.TabStop = false;
            // 
            // btn_GetVcode
            // 
            this.btn_GetVcode.Location = new System.Drawing.Point(114, 674);
            this.btn_GetVcode.Name = "btn_GetVcode";
            this.btn_GetVcode.Size = new System.Drawing.Size(75, 23);
            this.btn_GetVcode.TabIndex = 2;
            this.btn_GetVcode.Text = "提取验证码";
            this.btn_GetVcode.UseVisualStyleBackColor = true;
            this.btn_GetVcode.Click += new System.EventHandler(this.btn_GetVcode_Click);
            // 
            // tb_Vcode
            // 
            this.tb_Vcode.Location = new System.Drawing.Point(114, 704);
            this.tb_Vcode.Name = "tb_Vcode";
            this.tb_Vcode.Size = new System.Drawing.Size(100, 20);
            this.tb_Vcode.TabIndex = 3;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.tb_Vcode);
            this.Controls.Add(this.btn_GetVcode);
            this.Controls.Add(this.picBox_Vcode);
            this.Controls.Add(this.wb);
            this.Name = "MainFrm";
            this.Text = "Mail Register For Neatease";
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Vcode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Awesomium.Windows.Forms.WebControl wb;
        private System.Windows.Forms.PictureBox picBox_Vcode;
        private System.Windows.Forms.Button btn_GetVcode;
        private System.Windows.Forms.TextBox tb_Vcode;
    }
}

