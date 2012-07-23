using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using SendEmail.Models;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;
using SendEmail.Core;
using System.IO;
using System.Windows;

namespace SendEmail.ViewModels
{
    public class MainWindowViewModel : ViewModelBase<Account>
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public ObservableCollection<ReceiveAccounts> ReceiveAccounts { get; set; }

        public ICommand ImportAccountCommand { get; set; }
        public ICommand WebBrowserInit { get; set; }

        public MainWindowViewModel()
        {
            this.Accounts = new ObservableCollection<Account>();
            this.ImportAccountCommand = new DelegateCommand<string>((isLeft) =>
            {
                //string isleftTab = isLeft as string;
                MessageBox.Show(isLeft);
                if (isLeft == "left")
                {
                    OpenFileDialog dlg = new OpenFileDialog();

                    if (dlg.ShowDialog() == true)
                    {
                        string fname = dlg.FileName;
                        string[] infos = new string[2];


                        //文本操作===>读Txt start====
                        StreamReader sReader = File.OpenText(fname);
                        string str;
                        int i = 0;
                        while ((str = sReader.ReadLine()) != null)
                        {
                            ++i;
                            infos = str.Split('|');
                            this.Accounts.Add(new Account { ID = i.ToString(), UserName = infos[0], Password = infos[1] });

                        }
                    }
                }
                else {

                    OpenFileDialog dlg = new OpenFileDialog();

                    if (dlg.ShowDialog() == true)
                    {
                        string fname = dlg.FileName;
                        string[] infos = new string[2];


                        //文本操作===>读Txt start====
                        StreamReader sReader = File.OpenText(fname);
                        string str;
                        int i = 0;
                        while ((str = sReader.ReadLine()) != null)
                        {
                            ++i;
                            this.ReceiveAccounts.Add(new ReceiveAccounts { ID = i.ToString(), UserName = str });

                        }
                    }
                    
                }
                
            });

            this.WebBrowserInit = new DelegateCommand(() =>
            {

            });

            /*
            //Sample Data
            this.Accounts.Add(new Account { UserName = "afei001", Password = "test001" });
            this.Accounts.Add(new Account { UserName = "afei002", Password = "test002" });
            this.Accounts.Add(new Account { UserName = "afei003", Password = "test003" });
             * */

        }
    }
}
