﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using System.ComponentModel;

namespace EngineeringMath.GenericObject
{
    public class Parameter : INotifyPropertyChanged
    {
        /// <param name="title">Title of the field</param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal Parameter( int ID, string title, AbstractUnit[] desiredUnits, 
            bool isInput = true, double lowerLimit = double.MinValue, 
            double upperLimit = double.MaxValue)
        {
            if(lowerLimit >= upperLimit)
            {
                throw new ArgumentException("Lower Limit is greater than or equal to upper limit!");
            }

            if(desiredUnits.Length > 2)
            {
                throw new ArgumentOutOfRangeException("Cannot have more than 2 elements in desiredUnits");
            }
            this.ID = ID;
            this.LowerLimit = lowerLimit;
            this.UpperLimit = upperLimit;
            // prevent pointer sharing
            this.DesiredUnits = desiredUnits.ToArray();
            this.isInput = isInput;
            this.Title = title;

            this.SelectedIndex = new int[desiredUnits.Length];

            _PickerStrings = new List<string>[desiredUnits.Length];
            for(int i = 0; i < desiredUnits.Length; i++)
            {
                _PickerStrings[i] = new List<string>(StaticUnitProperties.AllUnits[desiredUnits[i].GetType()].Keys);
            }
            
           
        }

        public int ID { get; private set; }

        private double _Value = 0.0;

        /// <summary>
        /// Gets the value ValueStr represents
        /// </summary>
        public double GetValue()
        {
            double temp;
            double.TryParse(_ValueStr, out temp);

            temp = HelperFunctions.ConvertFrom(
            temp,
            DesiredUnits,
            SelectedStrings);
            if (temp < LowerLimit)
            {
                throw new ArgumentOutOfRangeException("Value below lower limit!");
            }
            else if (temp > UpperLimit)
            {
                throw new ArgumentOutOfRangeException("Value above upper limit!");
            }
            _Value = temp;
            OnPropertyChanged("Value");
            return _Value;
        }



        /// <summary>
        /// Sets the value of the parameter in the desired units and then sets the ValueStr
        /// <para>Current units are based on the state of SelectedStrings</para>
        /// </summary>
        /// <param name="value">The value in the current units</param>
        public void SetValue(double value)
        {
            double temp = HelperFunctions.ConvertTo(
                        value,
                        DesiredUnits,
                        SelectedStrings);
            if(temp < LowerLimit)
            {
                throw new ArgumentOutOfRangeException("Value below lower limit!");
            }
            else if(temp > UpperLimit)
            {
                throw new ArgumentOutOfRangeException("Value above upper limit!");
            }


            _Value = temp;
            ValueStr = string.Format("{0:G4}", temp);
            OnPropertyChanged("Value");
        }


        private string _ValueStr = string.Empty;
        /// <summary>
        /// String version of value (intended to be binded with field)
        /// </summary>
        public string ValueStr
        {
            get
            {
                return _ValueStr;
            }
            set
            {
                _ValueStr = value;
                OnPropertyChanged("ValueStr");

            }
        }


        /// <summary>
        /// The selected indexes of the pickers of the current units (Intented to binded with a picker)
        /// </summary>
        public int[] SelectedIndex { get; set; }

        /// <summary>
        /// Contains all strings which will be stored in the pickers (Intented to binded with a picker)
        /// </summary>
        public List<string>[] _PickerStrings;

        /// <summary>
        /// Strings selected by the user at the pickers
        /// </summary>
        protected string[] SelectedStrings
        {
            get
            {
                string[] temp = new string[SelectedIndex.Length];

                for(int i = 0; i < SelectedIndex.Length; i++)
                {
                    temp[i] = _PickerStrings[i][SelectedIndex[i]];
                }

                return temp;
            }
        }

        /// <summary>
        /// Title of the field
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// The lowest number the parameter is allowed to be
        /// </summary>
        public double LowerLimit { get; private set; }

        /// <summary>
        /// The highest number the paramter is allowed to be
        /// </summary>
        public double UpperLimit { get; private set; }


        /// <summary>
        /// The unit this parameter represents
        /// </summary>
        public AbstractUnit[] DesiredUnits { get; private set; }


        bool _isInput;
        public bool isInput
        {
            get
            {
                return _isInput;
            }
            set
            {
                _isInput = value;
                OnPropertyChanged("isInput");
                OnPropertyChanged("isOutput");
            }
        }

        public bool isOutput
        {
            get
            {
                // should always be the opposite of isInput
                return !this.isInput;
            }
            set
            {
                // property change event handled in the isInput parameter
                this.isInput = !value;                
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
