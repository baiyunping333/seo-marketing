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
            this.gb_template = new System.Windows.Forms.GroupBox();
            this.btn_importMailBody = new System.Windows.Forms.Button();
            this.rb_mailBody = new System.Windows.Forms.RichTextBox();
            this.label_mailBody = new System.Windows.Forms.Label();
            this.label_mailTitle = new System.Windows.Forms.Label();
            this.tb_Subject = new System.Windows.Forms.TextBox();
            this.label_to = new System.Windows.Forms.Label();
            this.tb_to = new System.Windows.Forms.TextBox();
            this.label_cc = new System.Windows.Forms.Label();
            this.label_from = new System.Windows.Forms.Label();
            this.tb_cc = new System.Windows.Forms.TextBox();
            this.tb_from = new System.Windows.Forms.TextBox();
            this.tb_SaveXml = new System.Windows.Forms.Button();
            this.btn_LoadXml = new System.Windows.Forms.Button();
            this.gb_template.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_template
            // 
            this.gb_template.Controls.Add(this.btn_importMailBody);
            this.gb_template.Controls.Add(this.rb_mailBody);
            this.gb_template.Controls.Add(this.label_mailBody);
            this.gb_template.Controls.Add(this.label_mailTitle);
            this.gb_template.Controls.Add(this.tb_Subject);
            this.gb_template.Controls.Add(this.label_to);
            this.gb_template.Controls.Add(this.tb_to);
            this.gb_template.Controls.Add(this.label_cc);
            this.gb_template.Controls.Add(this.label_from);
            this.gb_template.Controls.Add(this.tb_cc);
            this.gb_template.Controls.Add(this.tb_from);
            this.gb_template.Location = new System.Drawing.Point(12, 11);
            this.gb_template.Name = "gb_template";
            this.gb_template.Size = new System.Drawing.Size(760, 212);
            this.gb_template.TabIndex = 11;
            this.gb_template.TabStop = false;
            this.gb_template.Text = "设置模板";
            // 
            // btn_importMailBody
            // 
            this.btn_importMailBody.Location = new System.Drawing.Point(330, 71);
            this.btn_importMailBody.Name = "btn_importMailBody";
            this.btn_importMailBody.Size = new System.Drawing.Size(49, 115);
            this.btn_importMailBody.TabIndex = 21;
            this.btn_importMailBody.Text = "浏览导入EDM模板";
            this.btn_importMailBody.UseVisualStyleBackColor = true;
            // 
            // rb_mailBody
            // 
            this.rb_mailBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rb_mailBody.Location = new System.Drawing.Point(385, 56);
            this.rb_mailBody.Name = "rb_mailBody";
            this.rb_mailBody.Size = new System.Drawing.Size(358, 130);
            this.rb_mailBody.TabIndex = 20;
            this.rb_mailBody.Text = "Just a test!\n\n{sign1}";
            // 
            // label_mailBody
            // 
            this.label_mailBody.AutoSize = true;
            this.label_mailBody.Location = new System.Drawing.Point(327, 56);
            this.label_mailBody.Name = "label_mailBody";
            this.label_mailBody.Size = new System.Drawing.Size(53, 12);
            this.label_mailBody.TabIndex = 19;
            this.label_mailBody.Text = "邮件内容";
            // 
            // label_mailTitle
            // 
            this.label_mailTitle.AutoSize = true;
            this.label_mailTitle.Location = new System.Drawing.Point(327, 30);
            this.label_mailTitle.Name = "label_mailTitle";
            this.label_mailTitle.Size = new System.Drawing.Size(53, 12);
            this.label_mailTitle.TabIndex = 18;
            this.label_mailTitle.Text = "邮件标题";
            // 
            // tb_Subject
            // 
            this.tb_Subject.Location = new System.Drawing.Point(385, 27);
            this.tb_Subject.Name = "tb_Subject";
            this.tb_Subject.Size = new System.Drawing.Size(358, 21);
            this.tb_Subject.TabIndex = 17;
            this.tb_Subject.Text = "{date}邮件内容";
            // 
            // label_to
            // 
            this.label_to.AutoSize = true;
            this.label_to.Location = new System.Drawing.Point(22, 124);
            this.label_to.Name = "label_to";
            this.label_to.Size = new System.Drawing.Size(41, 12);
            this.label_to.TabIndex = 16;
            this.label_to.Text = "收件人";
            // 
            // tb_to
            // 
            this.tb_to.Location = new System.Drawing.Point(68, 121);
            this.tb_to.Multiline = true;
            this.tb_to.Name = "tb_to";
            this.tb_to.Size = new System.Drawing.Size(227, 66);
            this.tb_to.TabIndex = 15;
            this.tb_to.Text = "88603982@qq.com";
            // 
            // label_cc
            // 
            this.label_cc.AutoSize = true;
            this.label_cc.Location = new System.Drawing.Point(10, 66);
            this.label_cc.Name = "label_cc";
            this.label_cc.Size = new System.Drawing.Size(53, 12);
            this.label_cc.TabIndex = 13;
            this.label_cc.Text = "添加抄送";
            // 
            // label_from
            // 
            this.label_from.AutoSize = true;
            this.label_from.Location = new System.Drawing.Point(10, 34);
            this.label_from.Name = "label_from";
            this.label_from.Size = new System.Drawing.Size(53, 12);
            this.label_from.TabIndex = 14;
            this.label_from.Text = "我的邮箱";
            // 
            // tb_cc
            // 
            this.tb_cc.Location = new System.Drawing.Point(68, 64);
            this.tb_cc.Multiline = true;
            this.tb_cc.Name = "tb_cc";
            this.tb_cc.Size = new System.Drawing.Size(227, 52);
            this.tb_cc.TabIndex = 11;
            this.tb_cc.Text = "1290657123@qq.com\r\n491456131@qq.com";
            // 
            // tb_from
            // 
            this.tb_from.Location = new System.Drawing.Point(68, 31);
            this.tb_from.Name = "tb_from";
            this.tb_from.Size = new System.Drawing.Size(227, 21);
            this.tb_from.TabIndex = 12;
            this.tb_from.Text = "afeiship@163.com";
            // 
            // tb_SaveXml
            // 
            this.tb_SaveXml.Location = new System.Drawing.Point(12, 242);
            this.tb_SaveXml.Name = "tb_SaveXml";
            this.tb_SaveXml.Size = new System.Drawing.Size(75, 23);
            this.tb_SaveXml.TabIndex = 12;
            this.tb_SaveXml.Text = "保存为XML";
            this.tb_SaveXml.UseVisualStyleBackColor = true;
            this.tb_SaveXml.Click += new System.EventHandler(this.tb_SaveXml_Click);
            // 
            // btn_LoadXml
            // 
            this.btn_LoadXml.Location = new System.Drawing.Point(93, 242);
            this.btn_LoadXml.Name = "btn_LoadXml";
            this.btn_LoadXml.Size = new System.Drawing.Size(75, 23);
            this.btn_LoadXml.TabIndex = 13;
            this.btn_LoadXml.Text = "载入XML模板";
            this.btn_LoadXml.UseVisualStyleBackColor = true;
            this.btn_LoadXml.Click += new System.EventHandler(this.btn_LoadXml_Click);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 519);
            this.Controls.Add(this.btn_LoadXml);
            this.Controls.Add(this.tb_SaveXml);
            this.Controls.Add(this.gb_template);
            this.Name = "MainFrm";
            this.Text = "邮件模板";
            this.gb_template.ResumeLayout(false);
            this.gb_template.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_template;
        private System.Windows.Forms.Button btn_importMailBody;
        private System.Windows.Forms.RichTextBox rb_mailBody;
        private System.Windows.Forms.Label label_mailBody;
        private System.Windows.Forms.Label label_mailTitle;
        private System.Windows.Forms.TextBox tb_Subject;
        private System.Windows.Forms.Label label_to;
        private System.Windows.Forms.TextBox tb_to;
        private System.Windows.Forms.Label label_cc;
        private System.Windows.Forms.Label label_from;
        private System.Windows.Forms.TextBox tb_cc;
        private System.Windows.Forms.TextBox tb_from;
        private System.Windows.Forms.Button tb_SaveXml;
        private System.Windows.Forms.Button btn_LoadXml;
    }
}

