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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SendEmail.ViewModels;
using SendEmail.Models;
using SendEmail.Core;
using System.Text.RegularExpressions;

namespace SendEmail
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            NetEaseWebflow web163 = new NetEaseWebflow();
            string resString = web163.Login("","");
            string sid;
            //string loginStr = "<html><head><script type=\"text/javascript\">top.location.href = \"http://twebmail.mail.163.com/js4/main.jsp?sid=kCzLQEakWKBHVjuEhakkqZFELZpkWCjj\";</script></head><body></body></html>";

            //string[] tmpStr = loginStr.Split('=');
            //tmpStr = tmpStr[3].Split(';');
            //tmpStr=tmpStr[0].Split('"');
            /*
            Regex rgx = new Regex("sid=(.*)\";</script>");
            MatchCollection mc=rgx.Matches(loginStr);
            //mc[0].Value;
            //mc[1].Value;
            //mc[0].Captures
            foreach (Match match in mc){
                 Console.WriteLine("Found '{0}' at position {1}", match.Captures, match.Index);
           }
            */

            if (!string.IsNullOrEmpty(resString)) {
                this.txtRes.Text=resString;
                //MessageBox.Show(resString);
                sid = web163.getSessionId(resString);
                web163.SendMail(sid, "from", "to", "title", "content!!!");
                //MessageBox.Show("Login Success!");
                //web163.SendMail();
            }
        }


    }
}
