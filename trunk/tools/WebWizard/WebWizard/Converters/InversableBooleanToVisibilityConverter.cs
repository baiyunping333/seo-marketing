using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace WebWizard.Converters
{
    public class InversableBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool inverse = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out inverse);
            }

            bool val = inverse? !(bool)value : (bool)value;

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool inverse = false;
            if (parameter != null)
            {
                bool.TryParse(parameter.ToString(), out inverse);
            }

            Visibility val = (Visibility)value;
            if (val == Visibility.Visible)
            {
                return inverse ? false : true;
            }
            else
            {
                return inverse ? true : false;
            }
        }
    }
}
