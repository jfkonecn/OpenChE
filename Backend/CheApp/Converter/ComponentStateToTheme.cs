using EngineeringMath.Calculations.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Converter
{
    class ComponentStateToTheme : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((AbstractComponent.ComponentState)value)
            {
                case AbstractComponent.ComponentState.Success:
                    return (Style)Application.Current.Resources["Parameter.Success"];
                case AbstractComponent.ComponentState.Error:
                    return (Style)Application.Current.Resources["Parameter.Danger"];
                case AbstractComponent.ComponentState.Reset:
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // one way only!
            throw new NotImplementedException();
        }
    }
}
