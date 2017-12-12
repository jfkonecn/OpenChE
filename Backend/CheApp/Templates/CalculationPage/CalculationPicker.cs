using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.ObjectStyleBinders;
using EngineeringMath.Calculations;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// A picker which binds to PickerSelectionStyle Objects
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CalculationPicker<T> : Picker
    {
        public CalculationPicker(PickerSelectionStyle<T> obj)
        {
            this.SetBinding(Picker.ItemsSourceProperty, new Binding("ItemsSource"));
            this.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            this.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
            this.SetBinding(Picker.StyleProperty, new Binding("PickerStyle"));
            this.SetBinding(Picker.TitleProperty, new Binding("Title"));
            this.BindingContext = obj;
        }

        public CalculationPicker(PickerSelection<T> obj, out PickerSelectionStyle<T> style, string title = null) : this(style = new PickerSelectionStyle<T>(obj, title))
        {
            
        }
    }
}
