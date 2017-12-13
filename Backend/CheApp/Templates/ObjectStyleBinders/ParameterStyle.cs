using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using EngineeringMath.Calculations;
using EngineeringMath.Units;

namespace CheApp.Templates.ObjectStyleBinders
{
    /// <summary>
    /// Binds to a Parameter Object and contains properties which are intended to be binded to a UI
    /// </summary>
    public class ParameterStyle : BindableObject
    {
        // For Help:
        // https://developer.xamarin.com/api/type/Xamarin.Forms.BindableObject/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="para">Parameter this object is binding to</param>
        public ParameterStyle(Parameter para)
        {
            // Entry Object
            this.SetBinding(EntryTextProperty, new Binding("ValueStr", BindingMode.TwoWay));
            this.SetBinding(EntryIsEnabledProperty, new Binding("AllowUserInput", BindingMode.TwoWay));
            // Unit Pickers
            for(int i = 0; i < para.UnitSelection.Length; i++)
            {
                para.UnitSelection[i].SelectedIndex = 0;
            }

            // Substitute function picker
            para.SubFunctionSelection.SelectedIndex = 0;


            this.BindingContext = para;
        }

        public static readonly BindableProperty EntryTextProperty =
             BindableProperty.Create("EntryText", typeof(string),
                                      typeof(ParameterStyle),
                                      default(string));
        /// <summary>
        /// To be binded with an entry form object which stores the user input
        /// </summary>
        public string EntryText
        {
            get { return (string)GetValue(EntryTextProperty); }
            set
            {
                SetValue(EntryTextProperty, value);
                OnPropertyChanged("EntryText");
            }
        }


        public static readonly BindableProperty EntryIsEnabledProperty =
     BindableProperty.Create("EntryIsEnabled", typeof(bool),
                              typeof(ParameterStyle),
                              default(bool));

        /// <summary>
        /// To be binded with an entry form object which stores the user input
        /// </summary>
        public bool EntryIsEnabled
        {
            get { return (bool)GetValue(EntryIsEnabledProperty); }
            set
            {
                SetValue(EntryIsEnabledProperty, value);
                OnPropertyChanged("EntryIsEnabled");
            }
        }




        private Style _Style = (Style)Application.Current.Resources["neutralParameterStyle"];
        /// <summary>
        /// To be binded with an entry form object which stores the user input
        /// </summary>
        public Style Style
        {
            get
            {
                return _Style;
            }
            set
            {
                _Style = value;
                OnPropertyChanged("Style");
            }
        }
    }
}
