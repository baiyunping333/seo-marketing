using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MailRegisterFrm
{
    public partial class form_main : Form
    {
        public form_main()
        {
            InitializeComponent();
        }

        private void form_main_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate("http://reg.email.163.com/mailregAll/reg0.jsp?from=163mail_right");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;
            doc.GetElementById("unameInp").SetAttribute("value", "afei__002");
            doc.GetElementById("passwInp").SetAttribute("value", "happy123");
            doc.GetElementById("passConfim").SetAttribute("value", "happy123");

            HtmlElement vcode = doc.GetElementById("vcode_img");
            vcode.Style = "position: absolute; z-index: 9999; top: 0px; left: 0px";
            //抓图
            var b = new Bitmap(vcode.ClientRectangle.Width, vcode.ClientRectangle.Height);
            webBrowser1.DrawToBitmap(b, new Rectangle(new Point(), vcode.ClientRectangle.Size));
            
            //vcode.Style = "";
            //pictureBox1.Image = b;
            b.Save("test.jpg");
            
           
            
        }

        void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) {
                HtmlDocument doc = webBrowser1.Document;
                


                doc.GetElementById("verifyInp").SetAttribute("value", textBox1.Text);
                //doc.GetElementById("regBtn").InvokeMember("click");
            }
        }

        void  textBox1_Enter(object sender, EventArgs e)
        {
            MessageBox.Show("ENter");
 	        
        }

        

        void pictureBox1_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;
            

            pictureBox1.ImageLocation = doc.GetElementById("vcode_img").GetAttribute("src");
        }

    }
}
