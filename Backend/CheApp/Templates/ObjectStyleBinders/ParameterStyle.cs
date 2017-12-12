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


        private Style _Style = (Style)Application.Current.Resources["gridStyleLevel2"];
        /// <summary>
        /// To be binded with an entry form object which stores the user input
        /// </summary>
        public Style Style
        {
            get
            {
                return _Style;
            }
            private set
            {
                _Style = value;
                OnPropertyChanged("GridStyle");
            }
        }




        private Style _TitleStyle = (Style)Application.Current.Resources["minorHeaderStyle"];
        /// <summary>
        /// This is the style for the parameter title
        /// </summary>
        public Style TitleStyle
        {
            get
            {
                return _TitleStyle;
            }
            private set
            {
                _TitleStyle = value;
                OnPropertyChanged("TitleStyle");
            }
        }



        Color _BackgroundColor = Color.LightGray;
        /// <summary>
        /// Color of 
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return _BackgroundColor;
            }
            set
            {
                _BackgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }
    }
}
