﻿using System;
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
using System.Text;

namespace SendEmail.Core
{
    class NetEaseWebflow : DocumentWebflow
    {
        private string result;
        public string getSessionId(string loginStr) {
            loginStr = "index.jsp?sid=NCHSFNTQtROyopYnJFQQoDMSolGPSTQp\" name=";
            string pattern = @"index.jsp?sid=(.*)";
            var MatchCollectionmc = Regex.Matches(loginStr, pattern);
            
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

        public string SendMail(string from, string to, string title, string content) 
        {
            DocumentWebflow docWeb = new DocumentWebflow();
            
            //docWeb.ClearCookie();
            HttpRequestOperation dp = new HttpRequestOperation(
            "SendMail163",
            "http://twebmail.mail.163.com/js4/s?sid=NCHSFNTQtROyopYnJFQQoDMSolGPSTQp&func=mbox:compose&cl_send=2&l=compose&action=deliver",
            //String.Format("var={0}",mailContent),
            "POST", false, (o) =>
            {
                //MessageBox.Show(o.ToString());
                this.result = o.ToString();
            });
            dp.Execute(docWeb);

            //this.txtRes.Text = this.result;
            return this.result;
        }


        //获取发信帐号信息
        public Dictionary<string, string> GetSenderInfo() { 
            OpenFileDialog fileOpen = new OpenFileDialog();
            fileOpen.InitialDirectory = Directory.GetCurrentDirectory()+"\\data";
            fileOpen.Filter = "文本文件|*.txt|所有文件|*.*";
            fileOpen.RestoreDirectory = true;
            fileOpen.FilterIndex = 1;
            fileOpen.ShowDialog();

            string fname = fileOpen.FileName;
            Dictionary<string,string> senderInfo=new Dictionary<string,string>();
            string[] infos=new string[2];

            //文本操作===>读Txt start====
            StreamReader sReader = File.OpenText(fname);
            string str;
            while ((str = sReader.ReadLine()) != null)
            {
                infos=str.Split('|');
                senderInfo.Add(infos[0],infos[1]);
            }
            //文本操作===>读Txt end  ====
            return senderInfo;
        }
        
    }
}
