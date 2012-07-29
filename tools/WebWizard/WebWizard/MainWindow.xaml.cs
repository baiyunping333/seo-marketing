using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WebWizard.ViewModels;
using WebWizard.Views;
using Awesomium.Windows.Controls;
using Awesomium.Core;

namespace WebWizard
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
            WebCore.Initialize(WebCoreConfig.Default, true);
            WebCore.HomeURL = "http://www.baidu.com";
            this.DataContext = ApplicationViewModel.Instance;
		}

		private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.DragMove();
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}