using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Net.Mime;

namespace sendMailForResume
{
    public partial class MainFrm : Form
    {
        SmtpClient SmtpClient = new SmtpClient();   //设置SMTP协议
        



        public MainFrm()
        {
            InitializeComponent();
            this.initConfig();
        }

        public void initConfig() {
            SmtpClient.Host = "smtp.qq.com";
            SmtpClient.Port = 25;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_SendCompleted);
        }

        public void SendMailBySmtp(string mail_from,string mail_fromPwd,string mail_to,string mail_subject,string mail_body,string attachment=null) {
            SmtpClient.Credentials = new NetworkCredential(mail_from, mail_fromPwd);
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            Byte[] b = Encoding.Default.GetBytes(mail_body);
            string strBody = Encoding.GetEncoding("gb2312").GetString(b).ToString();
            MailMessage message = new MailMessage(mail_from, mail_to, mail_subject, strBody);
            
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            
            //添加附件：
            if (!String.IsNullOrEmpty(attachment)) {
                message.Attachments.Add(new Attachment(attachment));
            }
            

            SmtpClient.Send(message);
            
        }


        public void SaveXmlTeplate() { 
            
        }

        void SmtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("OK!发送完成！");
        }


        private void sendMail_Click(object sender, EventArgs e)
        {
            this.SendMailBySmtp("1290657123@qq.com","happyfox","88603982@qq.com","test2","好吧测试一下<b>NiuBi</b>!!!");
        }


        



        
    }
}
