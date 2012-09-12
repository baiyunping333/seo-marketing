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
using System.IO;
using Awesomium.Windows.Controls;
using System.Reflection;
using Awesomium.Core;

namespace RobinHood
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string scriptDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "scripts");

        public MainWindow()
        {
            InitializeComponent();
            
            webControl.DomReady += new EventHandler(webControl_DomReady);
            webControl.LoadCompleted += new EventHandler(webControl_LoadCompleted);
            webControl.CreateObject("awe");
            webControl.SetObjectCallback("awe", "statusChanged", (sender, e) =>
            {
                tStatus.Text = e.Arguments[0].ToString();
            });
        }

        private void RunScript(string filename)
        {
            string path = Path.Combine(scriptDir, filename);
            if (File.Exists(path))
            {
                using (StreamReader reader = File.OpenText(path))
                {
                    webControl.ExecuteJavascript(reader.ReadToEnd());
                }
            }
        }

        private void webControl_LoadCompleted(object sender, EventArgs e)
        {
            this.RunScript("core.js");
            this.RunScript("checkout.js");
            this.RunScript("signin.js");
            this.RunScript("data.js");
            this.RunScript("payment.js");
        }

        private void webControl_DomReady(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebCore.ClearCookies();
            string url = string.Empty;

            if (string.IsNullOrEmpty(tbAppleId.Text))
            {
                MessageBox.Show("Please input email address.");
                tbAppleId.Focus();
                return;
            }
            else
            {
                webControl.SetObjectProperty("awe", "email", new JSValue(tbAppleId.Text));
            }

            if (string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Please input password.");
                tbPassword.Focus();
                return;
            }
            else
            {
                webControl.SetObjectProperty("awe", "password", new JSValue(tbPassword.Text));
            }

            if (colorBlack.IsChecked == true)
            {
                if (model16.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD234LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model32.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD241LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model64.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD257LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
            }
            else if (colorWhite.IsChecked == true)
            {
                if (model16.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD237LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model32.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD244LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
                else if (model64.IsChecked == true)
                {
                    url = "http://store.apple.com/us/configure/MD260LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS";
                }
            }

            if (!string.IsNullOrEmpty(url))
            {
                webControl.Source = new Uri(url);
            }
        }
    }
}
