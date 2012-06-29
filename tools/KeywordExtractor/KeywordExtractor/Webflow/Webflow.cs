using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.ViewModel;
using System.Xml.Serialization;

namespace KeywordExtractor
{
    [Serializable]
    [XmlInclude(typeof(ScriptingWebflow))]
    public abstract class Webflow : NotificationObject
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
        public Webflow()
        {
            this.Operations = new ObservableCollection<Operation>();
        }
        #endregion

        #region Public Methods
        public void Start()
        {
        }
        #endregion
    }
}
