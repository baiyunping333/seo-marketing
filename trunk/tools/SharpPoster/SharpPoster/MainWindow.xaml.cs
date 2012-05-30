using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Text;

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
    }
}
