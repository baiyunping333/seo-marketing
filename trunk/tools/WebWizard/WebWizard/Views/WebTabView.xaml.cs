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

namespace WebWizard.Views
{
    /// <summary>
    /// Interaction logic for WebTabView.xaml
    /// </summary>
    public partial class WebTabView : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(WebTabView), new UIPropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public WebTabView()
        {
            InitializeComponent();
            this.webControl.Source = new Uri("http://www.baidu.com");
            this.webControl.BeginNavigation += new Awesomium.Core.BeginNavigationEventHandler(webControl_BeginNavigation);
            this.webControl.DomReady += new EventHandler(webControl_DomReady);
            this.webControl.LoadCompleted += new EventHandler(webControl_LoadCompleted);
            this.webControl.OpenExternalLink += new Awesomium.Core.OpenExternalLinkEventHandler(webControl_OpenExternalLink);
            this.RefreshButtonStatus();
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
                this.webControl.LoadURL(url);
            }
            else
            {
                MessageBox.Show("无效的地址", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButtonStatus()
        {
            this.btnBack.IsEnabled = this.webControl.HistoryBackCount > 0;
            this.btnForward.IsEnabled = this.webControl.HistoryForwardCount > 0;
            this.btnRefresh.Visibility = this.webControl.IsLoadingPage ? Visibility.Collapsed : Visibility.Visible;
            this.btnStop.Visibility = this.webControl.IsLoadingPage ? Visibility.Visible : Visibility.Collapsed;
        }

        private void webControl_BeginNavigation(object sender, Awesomium.Core.BeginNavigationEventArgs e)
        {
            this.RefreshButtonStatus();
            this.Title = e.Url.ToString();
        }

        private void webControl_DomReady(object sender, EventArgs e)
        {
            this.RefreshButtonStatus();
            this.tbAddress.Text = this.webControl.Source.ToString();
        }

        private void webControl_LoadCompleted(object sender, EventArgs e)
        {
            this.RefreshButtonStatus();
            this.Title = this.webControl.Title;
        }

        private void webControl_OpenExternalLink(object sender, Awesomium.Core.OpenExternalLinkEventArgs e)
        {
            this.webControl.Source = new Uri(e.Url);
        }

        private void tbAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Navigate();
            }
        }

        private void btnBack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.webControl.GoBack();
        }

        private void btnForward_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.webControl.GoForward();
        }

        private void btnRefresh_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.webControl.Reload();
        }

        private void btnStop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.webControl.Stop();
        }

        private void btnConfig_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
        }
    }
}
