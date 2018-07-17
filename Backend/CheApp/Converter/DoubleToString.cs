using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Converter
{
    public class DoubleToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0:G4}", (double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(double.TryParse((string)value, out double result))
            {
                return result;
            }
            else
            {
                return double.NaN;
            }
        }
    }
}
