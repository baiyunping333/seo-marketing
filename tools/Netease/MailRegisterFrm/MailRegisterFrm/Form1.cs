using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.IO;

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
            HTMLDocument comDoc = doc.DomDocument as HTMLDocument;
            HTMLBody comBody = comDoc.body as HTMLBody;
            doc.GetElementById("unameInp").SetAttribute("value", "afei__002");
            doc.GetElementById("passwInp").SetAttribute("value", "happy123");
            doc.GetElementById("passConfim").SetAttribute("value", "happy123");

            HtmlElement vcode = doc.GetElementById("vcode_img");
            IHTMLControlRange range = (IHTMLControlRange)comBody.createControlRange();
            range.add(vcode.DomElement as IHTMLControlElement);
            range.execCommand("Copy", false, null);

            var image = Clipboard.GetImage();
            pictureBox1.Image = image; 
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
