using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Awesomium.Core;
using System.Reflection;
using System.IO;

namespace RobinHood
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            WebCoreConfig config = new WebCoreConfig();
            config.SaveCacheAndCookies = true;
            config.UserDataPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            WebCore.Initialize(config, true);
        }
    }
}
