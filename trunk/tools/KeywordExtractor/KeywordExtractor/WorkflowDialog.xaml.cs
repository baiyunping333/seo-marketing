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
            this.DataContext = new WorkflowViewModel();
        }
    }
}
