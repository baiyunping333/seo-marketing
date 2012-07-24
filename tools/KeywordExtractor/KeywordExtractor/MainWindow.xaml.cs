using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Serialization;
using Microsoft.Win32;
using mshtml;
using Webflow;
using Webflow.Operations;
using Webflow.Triggers;
using System.Text.RegularExpressions;
using Awesomium.Core;

namespace KeywordExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DocumentWebflow webflow;

        public MainWindow()
        {
            InitializeComponent();

            webBrowser.BeginNavigation += new Awesomium.Core.BeginNavigationEventHandler(webBrowser_BeginNavigation);
            webBrowser.DomReady += new EventHandler(webBrowser_DomReady);
            webBrowser.LoadCompleted += new EventHandler(webBrowser_LoadCompleted);

            webflow = WebflowSamples.BlueHost as DocumentWebflow;
            webflow.Logger = new TextBoxLogger(this.tbLog);
        }

        private void webBrowser_BeginNavigation(object sender, Awesomium.Core.BeginNavigationEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void webBrowser_DomReady(object sender, EventArgs e)
        {
            tbAddress.Text = webBrowser.Source.ToString();
        }

        private void webBrowser_LoadCompleted(object sender, EventArgs e)
        {
            if (webBrowser.Source.Host == "note.sdo.com")
            {
                webBrowser.ExecuteJavascript(@"

                    $('#username').val('wbxfire@gmail.com');
                    $('#password').val('123456ab');
                    $('#loginbtn').click();

                ");
            }
            //throw new NotImplementedException();
        }

        protected void Navigate()
        {
            string url = tbAddress.Text;

            if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                url = "http://" + url;
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                webBrowser.LoadURL(url);
            }
            else
            {
                MessageBox.Show("无效的地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Navigate();
            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.GoBack();
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.GoForward();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            WorkflowDialog dlg = new WorkflowDialog();
            dlg.ShowDialog();
            if (dlg.DataContext != null)
            {
                //wf = (dlg.DataContext as WebflowViewModel).Model;
                //dgWorkflow.ItemsSource = wf.Operations;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML file|*.xml";

            if (dlg.ShowDialog() == true)
            {
                using (var file = File.OpenText(dlg.FileName))
                {
                    //XmlSerializer ser = new XmlSerializer(typeof(ScriptingWebflow));
                    //wf = (Webflow)ser.Deserialize(file);
                    file.Close();
                }
            }

            //if (wf != null)
            //{
            //    dgWorkflow.ItemsSource = wf.Operations;
            //}
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            //if (wf != null)
            //{
            //    string url = wf.Url;
            //    if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            //    {
            //        url = "http://" + url;
            //    }

            //    if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            //    {
            //        webBrowser.Navigate(url);
            //    }
            //    else
            //    {
            //        MessageBox.Show("无效的地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }
    }
}
