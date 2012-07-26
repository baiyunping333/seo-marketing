using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Input;

namespace WebWizard.ViewModels
{
    public class WebTabViewModel : NotificationObject
    {
        private string _title;
        public string Title
        {
            get { return this._title; }
            set
            {
                if (this._title != value)
                {
                    this._title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

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

        //public bool
    }
}
