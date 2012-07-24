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
using SendEmail.ViewModels;
using SendEmail.Models;
using SendEmail.Core;

namespace SendEmail
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
            NetEaseWebflow web163 = new NetEaseWebflow();
            string resString = web163.Login("","");
            string sid;
            //this.txtRes.Text=resString;
            if (!string.IsNullOrEmpty(resString)) {
                sid = web163.getSessionId(resString);
                //MessageBox.Show("Login Success!");
                //web163.SendMail();
            }
            
            
        }


    }
}
