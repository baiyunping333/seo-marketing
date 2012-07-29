using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;
using System.Windows.Media;

namespace WebWizard.ViewModels
{
    public abstract class TabViewModel : NotificationObject
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

        private bool _isSelected;
        public bool IsSelected
        {
            get { return this._isSelected; }
            set
            {
                if (this._isSelected != value)
                {
                    this._isSelected = value;
                    this.RaisePropertyChanged("IsSelected");
                }
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return this._isLoading; }
            set
            {
                if (this._isLoading != value)
                {
                    this._isLoading = value;
                    this.RaisePropertyChanged("IsLoading");
                }
            }
        }

        private bool _canClose;
        public bool CanClose
        {
            get { return this._canClose; }
            set
            {
                if (this._canClose != value)
                {
                    this._canClose = value;
                    this.RaisePropertyChanged("CanClose");
                }
            }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get { return this._icon; }
            set
            {
                if (this._icon != value)
                {
                    this._icon = value;
                    this.RaisePropertyChanged("Icon");
                }
            }
        }
    }
}
