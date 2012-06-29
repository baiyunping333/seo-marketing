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
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace KeywordExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Webflow wf;
        private UserData userData = new UserData();
        private Operation currentOperation;
        private IHTMLScriptElement script = null;
        private List<string> scriptRefs = new List<string>();
        private List<InjectionSetting> settings = new List<InjectionSetting>();
        public string xxx = "abc";

        public MainWindow()
        {
            InitializeComponent();
            webBrowser.ObjectForScripting = userData;
            webBrowser.Navigating += new NavigatingCancelEventHandler(webBrowser_Navigating);
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
                ScriptText = "$('#username').val('wbxfire@gmail.com');$('#password').val('123456ab');setTimeout(function(){$('#loginbtn').click();},1500);"
            });

            settings.Add(new InjectionSetting
            {
                UseJquery = false,
                Name = "麦库记事（已进入）",
                UrlPattern = new Regex("https://note.sdo.com/my$"),
                ScriptText = "setTimeout(function(){window.location.href = 'https://note.sdo.com/my#!note/create/';},500);"
            });

            settings.Add(new InjectionSetting
            {
                UseJquery = false,
                Name = "麦库记事（写内容）",
                UrlPattern = new Regex("https://note.sdo.com/my#!note/create/"),
                ScriptText = "setTimeout(function(){$('.note-detail-inp:eq(0)').val('test title');frames['baidu_editor_0'].document.body.innerHTML='adfljsalfjsafd'},500);"
            });

            this.dgUserData.ItemsSource = userData.Data;
            //dgInjection.ItemsSource = settings;
        }

        void webBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.WriteLog("Navigating:" + e.Uri.ToString());
        }

        private void webBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            this.WriteLog("LoadCompleted: " + e.Uri.ToString());
            if (currentOperation != null && currentOperation.Type == "等待页面加载")
            {
                currentOperation.Status = OperationStatus.Completed;
            }
            try
            {
                var doc = webBrowser.Document as HTMLDocument;
                if (doc != null)
                {
                    script = doc.createElement("script") as IHTMLScriptElement;
                    var body = doc.body as IHTMLDOMNode;

                    body.appendChild(script as IHTMLDOMNode);
                    //var firebug = doc.createElement("script") as IHTMLScriptElement;
                    //firebug.src = "https://getfirebug.com/firebug-lite.js";
                    //body.appendChild(firebug as IHTMLDOMNode);

                    //var jquery = doc.createElement("script") as IHTMLScriptElement;
                    //jquery.src = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2.min.js";
                    //body.appendChild(jquery as IHTMLDOMNode);

                    NextOperation();
                    this.ExecuteOperation();
                    //this.InjectScript(setting.ScriptText);

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
            this.WriteLog("Navigated: " + e.Uri.ToString());
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
            this.WriteLog("Executing Script.");
            if (script != null)
            {
                script.text = "(function (){ var userData=window.external;" + text + "})()";
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML file|*.xml";

            if (dlg.ShowDialog() == true)
            {
                using (var file = File.OpenText(dlg.FileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Workflow));
                    wf = (Workflow)ser.Deserialize(file);
                    file.Close();
                }
            }

            if (wf != null)
            {
                dgWorkflow.ItemsSource = wf.Operations;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (wf != null)
            {
                string url = wf.Url;
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
        }

        private void ExecuteOperation()
        {
            if (this.currentOperation != null)
            {
                this.WriteLog(this.currentOperation.Name);
                if (currentOperation.Type == "引用脚本")
                {
                    currentOperation.Status = OperationStatus.Running;
                    this.scriptRefs.Add(currentOperation.Parameter);
                    currentOperation.Status = OperationStatus.Completed;
                    NextOperation();
                    this.ExecuteOperation();
                }
                else if (currentOperation.Type == "执行脚本")
                {
                    currentOperation.Status = OperationStatus.Running;
                    this.InjectScript(currentOperation.Parameter);
                    currentOperation.Status = OperationStatus.Completed;
                    NextOperation();
                    this.ExecuteOperation();
                }
                else if (currentOperation.Type == "等待页面加载")
                {
                    currentOperation.Status = OperationStatus.Running;
                }
            }
        }

        private void NextOperation()
        {
            if (this.currentOperation == null)
            {
                this.currentOperation = this.wf.Operations[0];
            }
            else
            {
                int index = this.wf.Operations.IndexOf(this.currentOperation);
                if (index >= 0 && index < this.wf.Operations.Count - 1)
                {
                    this.currentOperation = this.wf.Operations[index + 1];
                }
                else
                {
                    this.currentOperation = null;
                }
            }
        }

        private void WriteLog(string text)
        {
            this.tbLog.Text = text + "\r\n" + this.tbLog.Text;
        }
    }
}
