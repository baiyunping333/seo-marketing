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
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Threading;

namespace KeywordExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IHTMLScriptElement script = null;
        private List<InjectionSetting> settings = new List<InjectionSetting>();

        public MainWindow()
        {
            InitializeComponent();
            webBrowser.Navigated += new NavigatedEventHandler(webBrowser_Navigated);
            webBrowser.LoadCompleted += new LoadCompletedEventHandler(webBrowser_LoadCompleted);
            btnGoBack.IsEnabled = webBrowser.CanGoBack;
            btnGoForward.IsEnabled = webBrowser.CanGoForward;

            string defaultScript = "alert('脚本注入成功')";

            settings.Add(new InjectionSetting
            {
                Name = "网易邮箱登录",
                UrlPattern = new Regex("http://mail.163.com"),
                ScriptText = "$('#idInput').val('afei_test001');$('#pwdInput').val('happy_123');$('#loginBtn').click();"
            });

            settings.Add(new InjectionSetting
            {
                Name = "网易邮箱",
                UrlPattern = new Regex("webmail.mail.163.com/js4"),
                ScriptText = "var doc = frames[0].document;$('a[title=写信]',doc).click();"
            });

            settings.Add(new InjectionSetting
            {
                UseJquery = false,
                Name = "麦库记事首页",
                UrlPattern = new Regex("http://note.sdo.com"),
                ScriptText = "$('#username').val('wbxfire@gmail.com');$('#password').val('123456ab');setTimeout(function(){$('#loginbtn').click();},1500)"
            });

            settings.Add(new InjectionSetting
            {
                Name = "麦库记事（已进入）",
                UrlPattern = new Regex("https://note.sdo.com/my"),
                ScriptText = "window.location.href = 'https://note.sdo.com/my#!note/create/'"
            });

            dgInjection.ItemsSource = settings;
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                var doc = webBrowser.Document as HTMLDocument;
                if (doc != null)
                {
                    script = doc.createElement("script") as IHTMLScriptElement;
                    var body = doc.body as IHTMLDOMNode;

                    body.appendChild(script as IHTMLDOMNode);

                    foreach (var setting in settings)
                    {
                        if (setting.UrlPattern.IsMatch(e.Uri.ToString()))
                        {
                            if (setting.UseJquery)
                            {
                                var jquery = doc.createElement("script") as IHTMLScriptElement;
                                jquery.src = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2.min.js";
                                body.appendChild(jquery as IHTMLDOMNode);
                            }
                            this.InjectScript(setting.ScriptText);
                            break;
                        }
                    }

                    tbCode.Text = doc.documentElement.outerHTML;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void inject()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = "js";
        }

        private void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            tbAddress.Text = e.Uri.ToString();
            btnGoBack.IsEnabled = webBrowser.CanGoBack;
            btnGoForward.IsEnabled = webBrowser.CanGoForward;
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
                webBrowser.Navigate(url);
            }
            else
            {
                MessageBox.Show("无效的地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Refresh();
        }

        private void tbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Navigate();
            }
        }

        private void InjectScript(string text)
        {
            if (script != null)
            {
                try
                {
                    script.text = "function __Main(){" + text + "}";
                    var result = webBrowser.InvokeScript("__Main");
                    if (result != null)
                    {
                        //tbResult.Text = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    //tbResult.Text = "脚本执行错误！\r\n" + ex.Message;
                }
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
    }
}
