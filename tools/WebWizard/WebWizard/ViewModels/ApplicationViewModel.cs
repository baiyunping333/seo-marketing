using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace WebWizard.ViewModels
{
    public class ApplicationViewModel
    {
        public static ApplicationViewModel Instance { get; private set; }

        public HomeTabViewModel HomeTab { get; set; }
        public ObservableCollection<WebTabViewModel> WebTabs { get; set; }
        public CompositeCollection AllTabs { get; set; }
        public ICommand OpenUrlCommand { get; private set; }

        static ApplicationViewModel()
        {
            Instance = new ApplicationViewModel();
        }

        private ApplicationViewModel()
        {
            this.HomeTab = HomeTabViewModel.Instance;
            this.HomeTab.IsSelected = true;
            this.WebTabs = new ObservableCollection<WebTabViewModel>();
            this.AllTabs = new CompositeCollection();
            this.AllTabs.Add(this.HomeTab);
            this.AllTabs.Add(new CollectionContainer { Collection = this.WebTabs });

            this.OpenUrlCommand = new DelegateCommand<string>((url) =>
            {
                var webTab = new WebTabViewModel(url);
                webTab.IsSelected = true;
                this.WebTabs.Add(webTab);
            });
        }
    }
}
