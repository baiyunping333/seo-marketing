using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SendEmail.ViewModels
{
    public class ViewModelBase<T>
    {
        public T Model { get; set; }
    }
}
