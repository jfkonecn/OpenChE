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
        public CalculationPicker(PickerSelection<T> obj, string title = null)
        {
            this.SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
            this.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            this.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
            this.BindingContext = obj;
        }
    }
}
