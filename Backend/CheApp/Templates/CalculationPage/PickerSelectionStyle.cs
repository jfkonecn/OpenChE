using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using EngineeringMath.Calculations;


namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Binds to a PickerSelection Object and contains properties which are intended to be binded to a picker
    /// </summary>
    public class PickerSelectionStyle<T> : BindableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">Selection this object is binding to</param>
        public PickerSelectionStyle(PickerSelection<T> obj)
        {
            this.SetBinding(ItemsSourceProperty, new Binding("PickerList", BindingMode.TwoWay));
            this.SetBinding(SelectedIndexProperty, new Binding("SelectedIndex", BindingMode.TwoWay));
            this.SetBinding(IsEnabledProperty, new Binding("IsEnabled", BindingMode.TwoWay));
            this.BindingContext = obj;
        }

        public static readonly BindableProperty ItemsSourceProperty =
     BindableProperty.Create("ItemsSource", typeof(List<string>),
                              typeof(ParameterStyle),
                              default(List<string>));
        /// <summary>
        /// To be binded with picker
        /// </summary>
        public List<string> ItemsSource
        {
            get { return (List<string>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
                OnPropertyChanged("ItemsSource");
            }
        }

        public static readonly BindableProperty SelectedIndexProperty =
BindableProperty.Create("SelectedIndex", typeof(int),
                      typeof(ParameterStyle),
                      default(int));
        /// <summary>
        /// To be binded with picker
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
                OnPropertyChanged("SelectedIndex");
            }
        }

        public static readonly BindableProperty IsEnabledProperty =
BindableProperty.Create("IsEnabled", typeof(bool),
              typeof(ParameterStyle),
              default(bool));
        /// <summary>
        /// To be binded with picker
        /// </summary>
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set
            {
                SetValue(IsEnabledProperty, value);
                OnPropertyChanged("IsEnabled");
            }
        }

        private Style _PickerStyle = (Style)Application.Current.Resources["pickerStyle"];
        /// <summary>
        /// This is the style for the picker
        /// </summary>
        public Style PickerStyle
        {
            get
            {
                return _PickerStyle;
            }
            private set
            {
                _PickerStyle = value;
                OnPropertyChanged("PickerStyle");
            }
        }

        /// <summary>
        /// Creates a new picker binded to this object
        /// </summary>
        /// <returns>Picker binded to this object</returns>
        public Picker CreatePicker()
        {
            Picker picker = new Picker();
            picker.SetBinding(Picker.ItemsSourceProperty, new Binding("ItemsSource"));
            picker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            picker.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
            picker.SetBinding(Picker.StyleProperty, new Binding("PickerStyle"));
            picker.BindingContext = this;
            return picker;
        }

    }
}

 