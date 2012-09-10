namespace sendMailForResume
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
            this.sendMail = new System.Windows.Forms.Button();
            this.gb_template = new System.Windows.Forms.GroupBox();
            this.btn_importMailBody = new System.Windows.Forms.Button();
            this.rb_mailBody = new System.Windows.Forms.RichTextBox();
            this.label_mailBody = new System.Windows.Forms.Label();
            this.label_mailTitle = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_to = new System.Windows.Forms.Label();
            this.tb_to = new System.Windows.Forms.TextBox();
            this.label_cc = new System.Windows.Forms.Label();
            this.label_from = new System.Windows.Forms.Label();
            this.tb_cc = new System.Windows.Forms.TextBox();
            this.tb_from = new System.Windows.Forms.TextBox();
            this.gb_saveXML = new System.Windows.Forms.GroupBox();
            this.gb_template.SuspendLayout();
            this.SuspendLayout();
            // 
            // sendMail
            // 
            this.sendMail.Location = new System.Drawing.Point(32, 433);
            this.sendMail.Name = "sendMail";
            this.sendMail.Size = new System.Drawing.Size(80, 40);
            this.sendMail.TabIndex = 0;
            this.sendMail.Text = "发送邮件";
            this.sendMail.UseVisualStyleBackColor = true;
            this.sendMail.Click += new System.EventHandler(this.sendMail_Click);
            // 
            // gb_template
            // 
            this.gb_template.Controls.Add(this.btn_importMailBody);
            this.gb_template.Controls.Add(this.rb_mailBody);
            this.gb_template.Controls.Add(this.label_mailBody);
            this.gb_template.Controls.Add(this.label_mailTitle);
            this.gb_template.Controls.Add(this.textBox1);
            this.gb_template.Controls.Add(this.label_to);
            this.gb_template.Controls.Add(this.tb_to);
            this.gb_template.Controls.Add(this.label_cc);
            this.gb_template.Controls.Add(this.label_from);
            this.gb_template.Controls.Add(this.tb_cc);
            this.gb_template.Controls.Add(this.tb_from);
            this.gb_template.Location = new System.Drawing.Point(12, 12);
            this.gb_template.Name = "gb_template";
            this.gb_template.Size = new System.Drawing.Size(760, 230);
            this.gb_template.TabIndex = 11;
            this.gb_template.TabStop = false;
            this.gb_template.Text = "设置模板";
            // 
            // btn_importMailBody
            // 
            this.btn_importMailBody.Location = new System.Drawing.Point(330, 77);
            this.btn_importMailBody.Name = "btn_importMailBody";
            this.btn_importMailBody.Size = new System.Drawing.Size(49, 125);
            this.btn_importMailBody.TabIndex = 21;
            this.btn_importMailBody.Text = "浏览导入EDM模板";
            this.btn_importMailBody.UseVisualStyleBackColor = true;
            // 
            // rb_mailBody
            // 
            this.rb_mailBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rb_mailBody.Location = new System.Drawing.Point(385, 61);
            this.rb_mailBody.Name = "rb_mailBody";
            this.rb_mailBody.Size = new System.Drawing.Size(358, 141);
            this.rb_mailBody.TabIndex = 20;
            this.rb_mailBody.Text = "";
            // 
            // label_mailBody
            // 
            this.label_mailBody.AutoSize = true;
            this.label_mailBody.Location = new System.Drawing.Point(327, 61);
            this.label_mailBody.Name = "label_mailBody";
            this.label_mailBody.Size = new System.Drawing.Size(55, 13);
            this.label_mailBody.TabIndex = 19;
            this.label_mailBody.Text = "邮件内容";
            // 
            // label_mailTitle
            // 
            this.label_mailTitle.AutoSize = true;
            this.label_mailTitle.Location = new System.Drawing.Point(327, 32);
            this.label_mailTitle.Name = "label_mailTitle";
            this.label_mailTitle.Size = new System.Drawing.Size(55, 13);
            this.label_mailTitle.TabIndex = 18;
            this.label_mailTitle.Text = "邮件标题";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(385, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(358, 20);
            this.textBox1.TabIndex = 17;
            // 
            // label_to
            // 
            this.label_to.AutoSize = true;
            this.label_to.Location = new System.Drawing.Point(22, 134);
            this.label_to.Name = "label_to";
            this.label_to.Size = new System.Drawing.Size(43, 13);
            this.label_to.TabIndex = 16;
            this.label_to.Text = "收件人";
            // 
            // tb_to
            // 
            this.tb_to.Location = new System.Drawing.Point(68, 131);
            this.tb_to.Multiline = true;
            this.tb_to.Name = "tb_to";
            this.tb_to.Size = new System.Drawing.Size(227, 71);
            this.tb_to.TabIndex = 15;
            // 
            // label_cc
            // 
            this.label_cc.AutoSize = true;
            this.label_cc.Location = new System.Drawing.Point(10, 72);
            this.label_cc.Name = "label_cc";
            this.label_cc.Size = new System.Drawing.Size(55, 13);
            this.label_cc.TabIndex = 13;
            this.label_cc.Text = "添加抄送";
            // 
            // label_from
            // 
            this.label_from.AutoSize = true;
            this.label_from.Location = new System.Drawing.Point(10, 37);
            this.label_from.Name = "label_from";
            this.label_from.Size = new System.Drawing.Size(55, 13);
            this.label_from.TabIndex = 14;
            this.label_from.Text = "我的邮箱";
            // 
            // tb_cc
            // 
            this.tb_cc.Location = new System.Drawing.Point(68, 69);
            this.tb_cc.Multiline = true;
            this.tb_cc.Name = "tb_cc";
            this.tb_cc.Size = new System.Drawing.Size(227, 56);
            this.tb_cc.TabIndex = 11;
            // 
            // tb_from
            // 
            this.tb_from.Location = new System.Drawing.Point(68, 34);
            this.tb_from.Name = "tb_from";
            this.tb_from.Size = new System.Drawing.Size(227, 20);
            this.tb_from.TabIndex = 12;
            // 
            // gb_saveXML
            // 
            this.gb_saveXML.Location = new System.Drawing.Point(12, 261);
            this.gb_saveXML.Name = "gb_saveXML";
            this.gb_saveXML.Size = new System.Drawing.Size(760, 100);
            this.gb_saveXML.TabIndex = 12;
            this.gb_saveXML.TabStop = false;
            this.gb_saveXML.Text = "保存为XML模板";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.gb_saveXML);
            this.Controls.Add(this.gb_template);
            this.Controls.Add(this.sendMail);
            this.Name = "MainFrm";
            this.Text = "邮件模板";
            this.gb_template.ResumeLayout(false);
            this.gb_template.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button sendMail;
        private System.Windows.Forms.GroupBox gb_template;
        private System.Windows.Forms.Button btn_importMailBody;
        private System.Windows.Forms.RichTextBox rb_mailBody;
        private System.Windows.Forms.Label label_mailBody;
        private System.Windows.Forms.Label label_mailTitle;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_to;
        private System.Windows.Forms.TextBox tb_to;
        private System.Windows.Forms.Label label_cc;
        private System.Windows.Forms.Label label_from;
        private System.Windows.Forms.TextBox tb_cc;
        private System.Windows.Forms.TextBox tb_from;
        private System.Windows.Forms.GroupBox gb_saveXML;
    }
}

