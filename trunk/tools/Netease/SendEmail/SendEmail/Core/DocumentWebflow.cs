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

namespace SendEmail.Core
{
    class NetEaseWebflow : DocumentWebflow
    {
        public NetEaseWebflow()
        {
            MessageBox.Show("Init!");
        }

        public string Login(string username ,string password) 
        {
            string res="1234";
            DocumentWebflow docWeb = new DocumentWebflow();
            docWeb.ClearCookie();
            UrlTrigger trigger = new UrlTrigger("my.bluehost.com/cgi-bin/cplogin$");
            
            trigger.Operations.Add(new HttpRequestOperation(
            "https://my.bluehost.com/cgi/dm/subdomain/add",
            string.Format("sub={0}&rdomain={1}&docroot={2}", "dsfasdf", "zhenfei.com", "sdfsdfs"),
            "POST", true));

            docWeb.Triggers.Add(trigger);
            
            return res;
        }

        public string SendMail(string from, string to, string title, string content) 
        {
            string res = "124";
            return res;
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
