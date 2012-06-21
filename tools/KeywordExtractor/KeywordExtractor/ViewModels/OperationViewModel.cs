using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace KeywordExtractor
{
    public class OperationViewModel : NotificationObject
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

        private string _type;
        public string Type
        {
            get { return this._type; }
            set
            {
                if (this._type != value)
                {
                    this._type = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        #endregion
    }
}
