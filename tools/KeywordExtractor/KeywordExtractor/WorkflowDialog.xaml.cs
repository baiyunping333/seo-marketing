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
        public WorkflowDialog()
        {
            InitializeComponent();

            //var vm = new WebflowViewModel();
            //this.DataContext = new WebflowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
