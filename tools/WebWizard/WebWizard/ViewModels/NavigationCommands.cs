using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace WebWizard.ViewModels
{
    public static class NavigationCommands
    {
        public static ICommand OpenNewTab
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    
                });
            }
        }
    }
}
