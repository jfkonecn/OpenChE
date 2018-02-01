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
        /// <param name="desiredUnits">Units desired when used for internal calculations</param>
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
                UnitSelection[i].OnSelectedIndexChanged += UnitPicker_OnSelectedIndexChanged(i);
            }


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">Index of UnitSelection</param>
        /// <returns></returns>
        private PickerSelection<AbstractUnit>.SelectedIndexChangedHandler UnitPicker_OnSelectedIndexChanged(int idx)
        {
            return delegate ()
            {
                OnReset();
                double num;
                if (this.isInput)
                {
                    Placeholder = string.Format(LibraryResources.ParameterValidRange, 
                        EffectiveLowerLimitString, 
                        EffectiveUpperLimitString);
                }
                else if (double.TryParse(ValueStr, out num))
                {
                    // we don't use _Field.GetValue() because it will give us the value 
                    // in terms of the currently selected units

                    // get the old units
                    AbstractUnit[] oldUnits = UnitSelection.Select(x => x.SelectedObject).ToArray();
                    oldUnits[idx] = UnitSelection[idx].PreviouslySelectedObject;

                    for (int i = 0; i < oldUnits.Length; i++)
                    {
                        if (oldUnits[i] == null)
                        {
                            // we can't convert!
                            return;
                        }
                    }

                    num = HelperFunctions.ConvertFrom(
                            num,
                            DesiredUnits,
                            oldUnits);

                    SetValue(num);
                }
            };


        }

        private double _Value = 0.0;

        /// <summary>
        /// Gets the value ValueStr represents in the desired units
        /// </summary>
        public double GetValue()
        {
            OnReset();
            double temp;
            if (double.TryParse(ValueStr, out temp))
            {

                temp = HelperFunctions.ConvertFrom(
                        temp,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
                // temp will be in the desired units
                if (temp < LowerLimit)
                {
                    OnError(new Exception(string.Format(LibraryResources.ValueBelowLowerLimit, EffectiveLowerLimitString)));
                }
                else if (temp > UpperLimit)
                {
                    OnError(new Exception(string.Format(LibraryResources.ValueAboveUpperLimit, EffectiveUpperLimitString)));
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
                OnError(new Exception(string.Format(LibraryResources.NotANumber)));
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
                OnError(new Exception(string.Format(LibraryResources.ValueBelowLowerLimit, EffectiveLowerLimitString)));
            }
            else if (temp > UpperLimit)
            {
                OnError(new Exception(string.Format(LibraryResources.ValueAboveUpperLimit, EffectiveUpperLimitString)));
            }
            else
            {
                OnSuccess();
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
                OnPropertyChanged("AllowUserInput");
                OnPropertyChanged("Placeholder");
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

        private string _Placeholder;
        public string Placeholder
        {
            get
            {
                if (isOutput)
                {
                    return null;
                }
                return _Placeholder;
            }
            set
            {
                OnPropertyChanged("Placeholder");
                _Placeholder = value;
            }
        }


        /// <summary>
        /// Stores data related to the units the user picker for this parameter (Intented to binded with a picker)
        /// </summary>
        public PickerSelection<AbstractUnit>[] UnitSelection { get; set; }




        /// <summary>
        /// The lowest number the parameter is allowed to be in the desired units
        /// </summary>
        public double LowerLimit { get; private set; }

        /// <summary>
        /// The highest number, as a formated string, the parameter is allowed to be in the currently selected units
        /// </summary>
        public string EffectiveLowerLimitString
        {
            get
            {
                double num;
                try
                {
                    num = HelperFunctions.ConvertTo(
                        LowerLimit,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());

                    if(double.IsNegativeInfinity(num))
                    {
                        num = double.MinValue;
                    }
                }
                catch (OverflowException)
                {
                    num = double.MinValue;
                }
                catch (ArgumentNullException)
                {
                    // UnitSelection was null so just say zero for now
                    num = 0;
                }
                return string.Format("{0:G2}", num);
            }
        }



        /// <summary>
        /// The highest number the paramter is allowed to be in the desired units
        /// </summary>
        public double UpperLimit { get; private set; }

        /// <summary>
        /// The highest number the parameter is allowed to be in the currently selected units
        /// </summary>
        public string EffectiveUpperLimitString
        {
            get
            {
                double num;
                try
                {
                    num = HelperFunctions.ConvertTo(
                        UpperLimit,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
                    if (double.IsPositiveInfinity(num))
                    {
                        num = double.MaxValue;
                    }
                }
                catch (OverflowException)
                {
                    num = double.MaxValue;
                }
                catch (ArgumentNullException)
                {
                    // UnitSelection was null so just say zero for now
                    num = 0;
                }
                return string.Format("{0:G2}", num);
            }
        }


        /// <summary>
        /// The unit this parameter represents
        /// </summary>
        public AbstractUnit[] DesiredUnits { get; private set; }



    }
}
