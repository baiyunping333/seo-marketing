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
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Vcode)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wb
            // 
            this.wb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb.Location = new System.Drawing.Point(0, 0);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(784, 562);
            this.wb.TabIndex = 0;
            // 
            // picBox_Vcode
            // 
            this.picBox_Vcode.Location = new System.Drawing.Point(0, 8);
            this.picBox_Vcode.Name = "picBox_Vcode";
            this.picBox_Vcode.Size = new System.Drawing.Size(100, 50);
            this.picBox_Vcode.TabIndex = 1;
            this.picBox_Vcode.TabStop = false;
            // 
            // btn_GetVcode
            // 
            this.btn_GetVcode.Location = new System.Drawing.Point(106, 8);
            this.btn_GetVcode.Name = "btn_GetVcode";
            this.btn_GetVcode.Size = new System.Drawing.Size(75, 23);
            this.btn_GetVcode.TabIndex = 2;
            this.btn_GetVcode.Text = "提取验证码";
            this.btn_GetVcode.UseVisualStyleBackColor = true;
            this.btn_GetVcode.Click += new System.EventHandler(this.btn_GetVcode_Click);
            // 
            // tb_Vcode
            // 
            this.tb_Vcode.Location = new System.Drawing.Point(97, 37);
            this.tb_Vcode.Name = "tb_Vcode";
            this.tb_Vcode.Size = new System.Drawing.Size(100, 20);
            this.tb_Vcode.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.picBox_Vcode);
            this.panel1.Controls.Add(this.tb_Vcode);
            this.panel1.Controls.Add(this.btn_GetVcode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 502);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 60);
            this.panel1.TabIndex = 4;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.wb);
            this.Name = "MainFrm";
            this.Text = "Mail Register For Neatease";
            ((System.ComponentModel.ISupportInitialize)(this.picBox_Vcode)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Awesomium.Windows.Forms.WebControl wb;
        private System.Windows.Forms.PictureBox picBox_Vcode;
        private System.Windows.Forms.Button btn_GetVcode;
        private System.Windows.Forms.TextBox tb_Vcode;
        private System.Windows.Forms.Panel panel1;
    }
}

