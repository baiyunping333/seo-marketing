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
using System.Windows.Shapes;

namespace KeywordExtractor
{
    /// <summary>
    /// Interaction logic for WorkflowDialog.xaml
    /// </summary>
    public partial class WorkflowDialog : Window
    {
        private WorkflowViewModel wf;

        public WorkflowDialog()
        {
            InitializeComponent();
            wf = new WorkflowViewModel
            {
                Name = "麦库记事",
                Url = "note.sdo.com"
            };

            this.DataContext = wf;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var op = new OperationViewModel()
            {
                Name = "ccc"
            };

            wf.Operations.Add(op);
            //dgOperations.ItemsSource = null;
            //dgOperations.ItemsSource = wf.Operations;
            dgOperations.SelectedItem = op;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            wf.Save();
        }
    }
}
