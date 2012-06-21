using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;

namespace KeywordExtractor
{
    [Serializable]
    public class Workflow : NotificationObject
    {
        #region Properties
        private string _name;
        public string Name
        {
            get { return this._name; }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        private string _url;
        public string Url
        {
            get { return this._url; }
            set
            {
                if (this._url != value)
                {
                    this._url = value;
                    this.RaisePropertyChanged("Url");
                }
            }
        }

        public ObservableCollection<Operation> Operations { get; set; }
        #endregion

        #region Constructors
        public Workflow()
        {
            this.Operations = new ObservableCollection<Operation>();
        }
        #endregion
    }
}
