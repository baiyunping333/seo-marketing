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
using mshtml;

namespace KeywordExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            webBrowser.Navigated += new NavigatedEventHandler(webBrowser_Navigated);
            webBrowser.LoadCompleted += new LoadCompletedEventHandler(webBrowser_LoadCompleted);
        }

        void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var doc = webBrowser.Document as HTMLDocument;
            if (doc != null)
            {
                var jquery = doc.createElement("script") as IHTMLScriptElement;
                var script = doc.createElement("script") as IHTMLScriptElement;
                var body = doc.body as IHTMLDOMNode;
                jquery.src = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2.min.js";
                script.text = "$(function(){$('#kw').val('test');$('#su').click();});";
                body.appendChild(jquery as IHTMLDOMNode);
                body.appendChild(script as IHTMLDOMNode);

                tbCode.Text = doc.documentElement.outerHTML;
            }
        }

        void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            tbAddress.Text = e.Uri.ToString();
        }

        protected void Navigate()
        {
            string url = tbAddress.Text;

            if (!url.StartsWith("http://") || !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                webBrowser.Navigate(url);
            }
            else
            {
                MessageBox.Show("无效的地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            this.Navigate();
        }

        private void tbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Navigate();
            }
        }
    }
}
