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
            webControl.Source =  new Uri("http://store.apple.com/us/configure/MD234LL/A?add-to-cart=add-to-cart&cppart=UNLOCKED%2FUS");
            webControl.DomReady += new EventHandler(webControl_DomReady);
            webControl.LoadCompleted += new EventHandler(webControl_LoadCompleted);
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
        }

        private void webControl_DomReady(object sender, EventArgs e)
        {
            
            //throw new NotImplementedException();
        }
    }
}
