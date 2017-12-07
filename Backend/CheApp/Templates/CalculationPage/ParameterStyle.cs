﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using EngineeringMath.Calculations;

namespace CheApp.Templates.CalculationPage
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

        private Style _EntryStyle = (Style)Application.Current.Resources["numericEntryStyle"];
        /// <summary>
        /// To be binded with an entry form object which stores the user input
        /// </summary>
        public Style EntryStyle
        {
            get
            {
                return _EntryStyle;
            }
            private set
            {
                _EntryStyle = value;
                OnPropertyChanged("EntryStyle");
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

        private Style _PickerStyle = (Style)Application.Current.Resources["pickerStyle"];
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


        private Style _LabelStyle = (Style)Application.Current.Resources["entryLabelStyle"];
        public Style LabelStyle
        {
            get
            {
                return _LabelStyle;
            }
            private set
            {
                _LabelStyle = value;
                OnPropertyChanged("LabelStyle");
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

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
