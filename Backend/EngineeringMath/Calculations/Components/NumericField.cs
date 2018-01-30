using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Selectors;

namespace EngineeringMath.Calculations.Components
{
    internal class NumericField : AbstractComponent
    {


        /// <param name="ID">Id of the parameter</param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal NumericField(string title, AbstractUnit[] desiredUnits,
            bool isInput = true,
            double lowerLimit = double.MinValue,
            double upperLimit = double.MaxValue)
        {
            if (lowerLimit >= upperLimit)
            {
                throw new ArgumentException("Lower Limit is greater than or equal to upper limit!");
            }

            if (desiredUnits.Length > 2)
            {
                throw new ArgumentOutOfRangeException("Cannot have more than 2 elements in desiredUnits");
            }

            this.LowerLimit = lowerLimit;
            this.UpperLimit = upperLimit;
            // prevent pointer sharing
            this.DesiredUnits = desiredUnits.ToArray();

            UnitSelection = new PickerSelection<AbstractUnit>[desiredUnits.Length];
            for (int i = 0; i < desiredUnits.Length; i++)
            {
                UnitSelection[i] = new PickerSelection<AbstractUnit>(StaticUnitProperties.AllUnits[desiredUnits[i].GetType()]);
                UnitSelection[i].SelectedObject = desiredUnits[i];
            }


        }


        

        private double _Value = 0.0;

        /// <summary>
        /// Gets the value ValueStr represents
        /// </summary>
        public double GetValue()
        {
            OnReset();
            double temp;
            if (double.TryParse(_ValueStr, out temp))
            {

                temp = HelperFunctions.ConvertFrom(
                        temp,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
                if (temp < LowerLimit)
                {
                    OnError(new Exception(string.Format(LibraryResources.ValueBelowLowerLimit, LowerLimit)));
                }
                else if (temp > UpperLimit)
                {
                    OnError(new Exception(string.Format(LibraryResources.ValueAboveUpperLimit, UpperLimit)));
                }
                else
                {
                    _Value = temp;
                    OnPropertyChanged("Value");
                    OnSuccess();
                }


            }
            else
            {
                OnError(new Exception(string.Format(LibraryResources.NotANumber, LowerLimit)));
            }

            return _Value;
        }

        /// <summary>
        /// Sets the value of the parameter in the desired units and then sets the ValueStr
        /// <para>Current units are based on the state of SelectedStrings</para>
        /// </summary>
        /// <param name="value">The value in the current units</param>
        public void SetValue(double value)
        {
            OnReset();
            double temp = HelperFunctions.ConvertTo(
                        value,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
            if (temp < LowerLimit)
            {
                OnError(new Exception(string.Format(LibraryResources.ValueBelowLowerLimit, LowerLimit)));
            }
            else if (temp > UpperLimit)
            {
                OnError(new Exception(string.Format(LibraryResources.ValueAboveUpperLimit, UpperLimit)));
            }

            OnSuccess();
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
        /// Stores data related to the units the user picker for this parameter (Intented to binded with a picker)
        /// </summary>
        public PickerSelection<AbstractUnit>[] UnitSelection { get; set; }




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



    }
}
