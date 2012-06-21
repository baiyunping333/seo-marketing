using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Reflection;
using Microsoft.Win32;

namespace KeywordExtractor
{
    [Serializable]
    public class WorkflowViewModel : NotificationObject
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

        private OperationViewModel _selectedOperation;
        public OperationViewModel SelectedOperation
        {
            get { return this._selectedOperation; }
            set
            {
                if (this._selectedOperation != value)
                {
                    this._selectedOperation = value;
                    this.RaisePropertyChanged("SelectedOperation", "HasSelectedOperation");
                    this.RefreshCommandStatus();
                }
            }
        }

        public bool HasSelectedOperation
        {
            get { return this.SelectedOperation != null; }
        }

        public ObservableCollection<OperationViewModel> Operations { get; set; }

        #endregion

        #region Commands
        [XmlIgnore]
        public DelegateCommand CreateOperationCommand { get; set; }
        [XmlIgnore]
        public DelegateCommand DeleteOperationCommand { get; set; }
        [XmlIgnore]
        public DelegateCommand MoveUpOperationCommand { get; set; }
        [XmlIgnore]
        public DelegateCommand MoveDownOperationCommand { get; set; }
        [XmlIgnore]
        public DelegateCommand SaveCommand { get; set; }
        #endregion

        #region Constructors
        public WorkflowViewModel()
        {
            this.Operations = new ObservableCollection<OperationViewModel>();
            this.CreateOperationCommand = new DelegateCommand(() =>
            {
                var op = new OperationViewModel();
                this.Operations.Add(op);
                this.SelectedOperation = op;
            });

            this.DeleteOperationCommand = new DelegateCommand(() =>
            {
                this.Operations.Remove(this.SelectedOperation);
                this.SelectedOperation = null;
            }, () =>
            {
                return this.HasSelectedOperation;
            });

            this.MoveUpOperationCommand = new DelegateCommand(() =>
            {
                int currentIndex = this.Operations.IndexOf(this.SelectedOperation);
                this.Operations.Move(currentIndex, currentIndex - 1);
                this.RefreshCommandStatus();
            }, () =>
            {
                if (this.SelectedOperation != null)
                {
                    int currentIndex = this.Operations.IndexOf(this.SelectedOperation);
                    int newIndex = currentIndex - 1;
                    if (newIndex >= 0)
                    {
                        return true;
                    }
                }
                return false;
            });

            this.MoveDownOperationCommand = new DelegateCommand(() =>
            {
                int currentIndex = this.Operations.IndexOf(this.SelectedOperation);
                this.Operations.Move(currentIndex, currentIndex + 1);
                this.RefreshCommandStatus();
            }, () =>
            {
                if (this.SelectedOperation != null)
                {
                    int currentIndex = this.Operations.IndexOf(this.SelectedOperation);
                    int newIndex = currentIndex + 1;
                    if (newIndex < this.Operations.Count)
                    {
                        return true;
                    }
                }
                return false;
            });

            this.SaveCommand = new DelegateCommand(() => this.Save());
        }
        #endregion

        #region Public Methods

        public void Save()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "xml";
            dlg.Filter = "XML file|.xml";
            if (dlg.ShowDialog() == true)
            {
                using (var file = File.CreateText(dlg.FileName))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(WorkflowViewModel));
                    ser.Serialize(file, this);
                    file.Close();
                }
            }
        }

        public void Load()
        {

        }

        #endregion

        #region Private Methods

        private void RefreshCommandStatus()
        {
            this.DeleteOperationCommand.RaiseCanExecuteChanged();
            this.MoveUpOperationCommand.RaiseCanExecuteChanged();
            this.MoveDownOperationCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
