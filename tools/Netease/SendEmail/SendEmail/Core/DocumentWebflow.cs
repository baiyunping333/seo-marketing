using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SendEmail.Core;
using System.IO;
using Microsoft.Win32;
using Webflow;
using Webflow.Triggers;
using Webflow.Operations;
using System.Text.RegularExpressions;
using System.Net;
using System.Web;


namespace SendEmail.Core
{
    class NetEaseWebflow : DocumentWebflow
    {
        private string result;
        public string getSessionId(string loginStr) {
            //loginStr = "index.jsp?sid=NCHSFNTQtROyopYnJFQQoDMSolGPSTQp\" name=";
            //string pattern = "?sid=(.*)\";</script>";
            //Regex rgx = new Regex("?sid=(.*)\";</script>");
            //rgx.Match(loginStr);
            string[] tmpStr = loginStr.Split('=');
            tmpStr = tmpStr[3].Split(';');
            tmpStr = tmpStr[0].Split('"');
            loginStr = tmpStr[0];
            return loginStr;
        }
        public string Login(string username ,string password) 
        {
            DocumentWebflow docWeb = new DocumentWebflow();
            docWeb.ClearCookie();


            HttpRequestOperation dp = new HttpRequestOperation(
            "login163",
            "https://ssl.mail.163.com/entry/coremail/fcg/ntesdoor2?df=webmail163&from=web&funcid=loginone&iframe=1&language=-1&net=t&passtype=1&product=mail163&race=-2_-2_-2_db&style=-1&uid=afei_test001@163.com",
            "savelogin=0&url2=http%3A%2F%2Fmail.163.com%2Ferrorpage%2Ferr_163.htm&username=afei_test001&password=happy_123",
            "POST", false, (o) => {
                //MessageBox.Show(o.ToString());
                this.result = o.ToString();
            });
            dp.Execute(docWeb);
            
            //this.txtRes.Text = this.result;
            return this.result;
        }

        public string SendMail(string sid,string from, string to, string title, string content) 
        {
            DocumentWebflow docWeb = new DocumentWebflow();
            string mailContent = HttpUtility.UrlEncode("<?xml version=\"1.0\"?><object><string name=\"id\">c:1343220470584</string><object name=\"attrs\"><string name=\"account\">\"afei_test001\"&lt;afei_test001@163.com&gt;</string><boolean name=\"showOneRcpt\">false</boolean><array name=\"to\"><string>1290657123@qq.com</string></array><array name=\"cc\"/><array name=\"bcc\"/><string name=\"subject\">ttt</string><boolean name=\"isHtml\">true</boolean><string name=\"content\">&lt;div style='line-height:1.7;color:#000000;font-size:14px;font-family:arial'&gt;cccc&lt;/div&gt;</string><int name=\"priority\">3</int><boolean name=\"saveSentCopy\">true</boolean><boolean name=\"requestReadReceipt\">false</boolean><string name=\"charset\">GBK</string></object><boolean name=\"returnInfo\">false</boolean><string name=\"action\">deliver</string><int name=\"saveSentLimit\">1</int></object>");
            //docWeb.ClearCookie();
            HttpRequestOperation dp = new HttpRequestOperation(
            "SendMail163",
            String.Format("http://twebmail.mail.163.com/js4/s?sid={0}&func=mbox:compose&cl_send=2&l=compose&action=deliver",sid),
            String.Format("var={0}",mailContent),
            "POST", true, (o) =>
            {
                MessageBox.Show(o.ToString());
                this.result = o.ToString();
            });
            dp.Execute(docWeb);

            //this.txtRes.Text = this.result;
            return this.result;
        }


        ////获取发信帐号信息
        //public Dictionary<string, string> GetSenderInfo() { 
        //    OpenFileDialog fileOpen = new OpenFileDialog();
        //    fileOpen.InitialDirectory = Directory.GetCurrentDirectory()+"\\data";
        //    fileOpen.Filter = "文本文件|*.txt|所有文件|*.*";
        //    fileOpen.RestoreDirectory = true;
        //    fileOpen.FilterIndex = 1;
        //    fileOpen.ShowDialog();

        //    string fname = fileOpen.FileName;
        //    Dictionary<string,string> senderInfo=new Dictionary<string,string>();
        //    string[] infos=new string[2];

        //    //文本操作===>读Txt start====
        //    StreamReader sReader = File.OpenText(fname);
        //    string str;
        //    while ((str = sReader.ReadLine()) != null)
        //    {
        //        infos=str.Split('|');
        //        senderInfo.Add(infos[0],infos[1]);
        //    }
        //    //文本操作===>读Txt end  ====
        //    return senderInfo;
        //}
        
    }
}
