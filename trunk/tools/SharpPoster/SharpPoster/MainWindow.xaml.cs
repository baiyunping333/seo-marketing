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
            BlueHost.Login("zhenfeic", "aaAbc123456!");
            //BlueHost.AddSubDomain("zxc334", "zhenfei.com", "zxc334");
            Dictionary<string, string> domains = new Dictionary<string, string>();
            domains.Add("zxkf", "qiaojoe.com");
            domains.Add("zxrj", "qiaojoe.com");
            domains.Add("zyqb", "qiaojoe.com");
            domains.Add("zxc333", "zhenfei.com");
            domains.Add("zxc125", "zhenfei.com");
            domains.Add("zxc123", "zhenfei.com");
            foreach (KeyValuePair<string, string> item in domains)
            {
                BlueHost.DeleteDomain(item.Key, item.Value);
            }
            //BlueHost.DeleteDomain("t002", "zhenfei.com");
        }



    }
}
