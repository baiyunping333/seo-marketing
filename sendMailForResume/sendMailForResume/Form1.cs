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
using System.Xml.Serialization;

namespace sendMailForResume
{
    public partial class MainFrm : Form
    {
        SmtpClient SmtpClient = new SmtpClient();   //设置SMTP协议


        MailInfo info = new MailInfo();

        public MainFrm()
        {
            InitializeComponent();
            this.initConfig();
        }

        public void initConfig() {
            SmtpClient.Host = "xchcasha.cisco.com";
            SmtpClient.Port = 25;
            SmtpClient.UseDefaultCredentials = false;
            //SmtpClient.SendCompleted += new SendCompletedEventHandler(SmtpClient_SendCompleted);
        }

        public void SendMailBySmtp(string mail_from,string mail_fromPwd,string mail_to,string mail_subject,string mail_body,string attachment=null) {

            

            SmtpClient.Credentials = new NetworkCredential(mail_from, mail_fromPwd);
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            Byte[] b = Encoding.Default.GetBytes(mail_body);
            string strBody = Encoding.GetEncoding("gb2312").GetString(b).ToString();
            MailMessage message = new MailMessage(mail_from, mail_to, mail_subject, strBody);
            //message.From.Address = mail_from;
            //message.From.Address = (String)mail_from; 

            


            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            
            //添加附件：
            if (!String.IsNullOrEmpty(attachment)) {
                message.Attachments.Add(new Attachment(attachment));
            }
            

            SmtpClient.Send(message);
            
        }


        public void SaveXmlTeplate(string filepath) {
            info.From = tb_from.Text;
            info.To = tb_to.Text.Split('\n');
            info.Cc = tb_cc.Text.Split('\n');
            info.Subject = tb_Subject.Text;
            info.Body = rb_mailBody.Text;
            XmlSerializer ser = new XmlSerializer(typeof(MailInfo));

            using (Stream s = File.OpenWrite(filepath))
            {
                ser.Serialize(s, info);
                //s.Close();
            }
        }

        public void FillXmlTeplate(string filepath) { 
            
        }

        private void tb_SaveXml_Click(object sender, EventArgs e)
        {
            //SaveXmlTeplate();
            OpenSaveDialog();
        }


        public void OpenSaveDialog() {
            SaveFileDialog dlg = new SaveFileDialog();
            //dlg.InitialDirectory = "c://";
            dlg.InitialDirectory = "d://";
            dlg.Filter = "XML模板文件|*.xml|所有文件|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if(dlg.ShowDialog()==DialogResult.OK){
                SaveXmlTeplate(dlg.FileName);
            }
        }

        public void OpenLoadDialog() {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.InitialDirectory = "c://";
            dlg.InitialDirectory = "d://";
            dlg.Filter = "XML模板文件|*.xml|所有文件|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            DateTime dt = new DateTime();

            XmlSerializer ser = new XmlSerializer(typeof(MailInfo));

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                using (Stream s = dlg.OpenFile()) {
                    MailInfo a = ser.Deserialize(s) as MailInfo;
                    tb_from.Text = a.From;
                    tb_to.Text = "Need to resolve the problem";
                    tb_cc.Text = "ccList!";
                    tb_Subject.Text = a.Subject.Replace("{date}","2012-09-18");
                    rb_mailBody.Text = a.Body.Replace("{sign1}","这里是邮件签名区域~");
                    //tb_to.Text = a.To.Join();
                }
            }
        }


        private void btn_LoadXml_Click(object sender, EventArgs e)
        {
            OpenLoadDialog();
        }

        private void btn_SendEmail_Click(object sender, EventArgs e)
        {
            SendMailBySmtp(tb_from.Text,"SK_123.com",tb_to.Text,tb_Subject.Text,rb_mailBody.Text);
        }




        


        



        
    }
}
