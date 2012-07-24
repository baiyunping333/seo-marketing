using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.ViewModel;

namespace SendEmail.Models
{
    public class ReceiveAccounts : NotificationObject
    {
        private string _id;
        public string ID
        {
            get { return this._id; }
            set
            {
                if (this._id != value)
                {
                    this._id = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }

        private string _userName;
        public string UserName
        {
            get { return this._userName; }
            set
            {
                if (this._userName != value)
                {
                    this._userName = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }

        
    }
}
