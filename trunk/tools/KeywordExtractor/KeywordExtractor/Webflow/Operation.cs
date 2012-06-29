using System;
using Microsoft.Practices.Prism.ViewModel;
using System.Xml.Serialization;

namespace KeywordExtractor
{
    [Serializable]
    [XmlInclude(typeof(IncludeScriptOperation))]
    public abstract class Operation : NotificationObject
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

        private string _parameter;
        public string Parameter
        {
            get { return this._parameter; }
            set
            {
                if (this._parameter != value)
                {
                    this._parameter = value;
                    this.RaisePropertyChanged("Parameter");
                }
            }
        }

        private OperationStatus _status;
        public OperationStatus Status
        {
            get { return this._status; }
            set
            {
                if (this._status != value)
                {
                    this._status = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }

        public Webflow Webflow { get; private set; }

        #endregion

        public Operation(Webflow webflow)
        {
            this.Webflow = webflow;
            this.Status = OperationStatus.NotStarted;
        }

        public abstract void Execute();
    }
}
