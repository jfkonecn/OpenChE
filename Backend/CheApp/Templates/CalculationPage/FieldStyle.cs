using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EngineeringMath.Units;
using EngineeringMath.Calculations;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Object which binds with a Numeric Field Data Object
    /// </summary>
    public class FieldStyle : INotifyPropertyChanged
    {


        /// <summary>
        /// Binds data to UI
        /// </summary>
        public FieldStyle()
        {

        }

        /// <summary>
        /// The selected index of each picker
        /// </summary>
        public int[] SelectedIndex { get; set; }

        Color _BackgroundColor = Color.LightGray;
        /// <summary>
        /// Color of 
        /// </summary>
        public Color BackgroundColor {
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
