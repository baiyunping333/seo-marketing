using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WebWizard.ViewModels
{
    public class MainWindowViewModel
    {
        public HomeTabViewModel HomeTab { get; set; }
        public ObservableCollection<WebTabViewModel> WebTabs { get; set; }
        public CompositeCollection AllTabs { get; set; }

        public MainWindowViewModel()
        {
            this.HomeTab = new HomeTabViewModel();
            this.WebTabs = new ObservableCollection<WebTabViewModel>();
            this.AllTabs = new CompositeCollection();
            this.AllTabs.Add(this.HomeTab);
            this.AllTabs.Add(new CollectionContainer { Collection = this.WebTabs });

            this.WebTabs.Add(new WebTabViewModel());
        }
    }
}
