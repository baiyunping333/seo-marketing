using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Collections.Generic;

namespace SharpPoster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //MessageBox.Show(string.Format("{0:D2}", 2));
        }



        private void tbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Request();
            }
        }

        private void Request()
        {
            string url = tbUrl.Text;

            if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                url = "http://" + url;
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var response = HttpWebResponseUtility.CreateGetHttpResponse(url, null, null, null);
                using (var stream = response.GetResponseStream())
                {
                    using (var sr = new StreamReader(stream, Encoding.UTF8))
                    {
                        tbResponse.Text = sr.ReadToEnd();
                    }
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //BlueHost.Login("zhenfeic", "aaAbc123456!");
            //BlueHost.AddSubDomain("zxc334", "zhenfei.com", "zxc334");
            //domains["too1"] = "zhenfei.com"
            /*
              Dictionary<string, string> aa = new Dictionary<string, string> { { "abc", "111" }, { "bbb", "aaa" } };
             */
            Dictionary<string, string> domains = new Dictionary<string, string>();
            domains.Add("t001", "qiaojoe.com");
            domains.Add("sjxl", "qiaojoe.com");
            domains.Add("qdcp", "qiaojoe.com");
            domains.Add("qbcp", "qiaojoe.com");
            domains.Add("mysql", "qiaojoe.com");
            domains.Add("lsjf", "qiaojoe.com");
            domains.Add("lsejf", "qiaojoe.com");
            domains.Add("jfmj", "qiaojoe.com");

            //jf01.qiaojoe.com~jf99.qiaojoe.com
            for (int i = 1; i <= 99; i++) {
                domains.Add("jf" + string.Format("{0:D2}", i), "qiaojoe.com");
            }

            foreach (KeyValuePair<string, string> item in domains)
            {
                BlueHost.DeleteDomain(item.Key, item.Value);
            }
            //BlueHost.DeleteDomain("t002", "zhenfei.com");
        }

        /*
         * 1.登录163邮箱
         * 2.发邮件
         */
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            BlueHost.NELogin("afei_test001", "happy_123");
            BlueHost.NEWriteEmail("afei_test001", "afei_test001@163.com", "testTitle22222", "testContent2222.....");
        }



    }
}
