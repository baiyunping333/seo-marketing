using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;
using Awesomium.Core;
using System.Windows.Media.Imaging;

namespace WebWizard.ViewModels
{
    public class WebTabViewModel : TabViewModel
    {
        private Uri _source;
        public Uri Source
        {
            get { return this._source; }
            set
            {
                if (this._source != value)
                {
                    this._source = value;
                    this.RaisePropertyChanged("Source");
                }
            }
        }

        private bool _canGoBack;
        public bool CanGoBack
        {
            get { return this._canGoBack; }
            set
            {
                if (this._canGoBack != value)
                {
                    this._canGoBack = value;
                    this.RaisePropertyChanged("CanGoBack");
                }
            }
        }

        private bool _canGoForward;
        public bool CanGoForward
        {
            get { return this._canGoForward; }
            set
            {
                if (this._canGoForward != value)
                {
                    this._canGoForward = value;
                    this.RaisePropertyChanged("CanGoForward");
                }
            }
        }

        public WebTabViewModel()
        {
            this.Source = new Uri(WebCore.HomeURL);
            this.Icon = new BitmapImage(new Uri("/WebWizard;component/Assets/icons/glyphicons_340_globe.png", UriKind.RelativeOrAbsolute));
        }

        public WebTabViewModel(string url)
        {
            this.Source = new Uri(string.IsNullOrEmpty(url) ? WebCore.HomeURL : url);
            this.Icon = new BitmapImage(new Uri("/WebWizard;component/Assets/icons/glyphicons_340_globe.png", UriKind.RelativeOrAbsolute));
        }
    }
}
