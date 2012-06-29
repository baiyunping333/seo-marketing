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
    public class WorkflowViewModel : ViewModelBase<ScriptingWebflow>
    {
        #region Properties
        private Operation _selectedOperation;
        public Operation SelectedOperation
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

        private string[] _operationTypes = new string[] { "引用脚本", "执行脚本", "等待页面加载" };
        public string[] OperationTypes
        {
            get { return _operationTypes; }
        }
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
            var wf = new ScriptingWebflow();

            this.Model = wf;

            this.CreateOperationCommand = new DelegateCommand(() =>
            {
                var op = new ExecuteScriptOperation(wf);
                this.Model.Operations.Add(op);
                this.SelectedOperation = op;
            });

            this.DeleteOperationCommand = new DelegateCommand(() =>
            {
                this.Model.Operations.Remove(this.SelectedOperation);
                this.SelectedOperation = null;
            }, () =>
            {
                return this.HasSelectedOperation;
            });

            this.MoveUpOperationCommand = new DelegateCommand(() =>
            {
                int currentIndex = this.Model.Operations.IndexOf(this.SelectedOperation);
                this.Model.Operations.Move(currentIndex, currentIndex - 1);
                this.RefreshCommandStatus();
            }, () =>
            {
                if (this.SelectedOperation != null)
                {
                    int currentIndex = this.Model.Operations.IndexOf(this.SelectedOperation);
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
                int currentIndex = this.Model.Operations.IndexOf(this.SelectedOperation);
                this.Model.Operations.Move(currentIndex, currentIndex + 1);
                this.RefreshCommandStatus();
            }, () =>
            {
                if (this.SelectedOperation != null)
                {
                    int currentIndex = this.Model.Operations.IndexOf(this.SelectedOperation);
                    int newIndex = currentIndex + 1;
                    if (newIndex < this.Model.Operations.Count)
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
                    XmlSerializer ser = new XmlSerializer(typeof(Webflow));
                    ser.Serialize(file, this.Model);
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
