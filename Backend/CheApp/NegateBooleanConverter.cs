using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp
{
    // taken from: JoeManke
    // https://forums.xamarin.com/discussion/36714/how-to-in-a-binding-in-xaml

    /// <summary>
    /// Negates binded bools
    /// </summary>
    class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
