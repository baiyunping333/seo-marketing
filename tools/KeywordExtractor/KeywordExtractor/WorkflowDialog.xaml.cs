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
        private Workflow wf;

        public WorkflowDialog()
        {
            InitializeComponent();
            wf = new Workflow
            {
                Name = "麦库记事",
                Url = "note.sdo.com"
            };
            //for (int i = 0; i < 20; i++)
            {
                wf.Operations.AddLast(new ExecuteScriptOperation(wf)
                {
                    Name = "登录"
                });
                wf.Operations.AddLast(new ExecuteScriptOperation(wf)
                {
                    Name = "新建笔记"
                });
                wf.Operations.AddLast(new ExecuteScriptOperation(wf)
                {
                    Name = "填写并保存笔记"
                });
                wf.Operations.AddLast(new ExecuteScriptOperation(wf)
                {
                    Name = "取回共享链接"
                });
            }

            this.DataContext = wf;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var op = new ExecuteScriptOperation(wf)
            {
                Name = "ccc"
            };

            wf.Operations.AddLast(op);
            dgOperations.ItemsSource = null;
            dgOperations.ItemsSource = wf.Operations;
            dgOperations.SelectedItem = op;
        }
    }
}
