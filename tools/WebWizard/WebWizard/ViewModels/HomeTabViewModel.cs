using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WebWizard.ViewModels
{
    public class HomeTabViewModel : TabViewModel
    {
        public static HomeTabViewModel Instance { get; private set; }

        static HomeTabViewModel()
        {
            Instance = new HomeTabViewModel();
        }

        private HomeTabViewModel()
        {
            this.Title = "Home";
            this.CanClose = false;
            this.Icon = new BitmapImage(new Uri("/WebWizard;component/Assets/icons/glyphicons_009_magic.png", UriKind.RelativeOrAbsolute));
        }
    }
}
