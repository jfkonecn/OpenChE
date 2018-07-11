using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Selectors;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EngineeringMath.Calculations.Components.Parameter
{
    public class SimpleParameter : AbstractComponent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title">Title of the field</param>
        /// <param name="ID">Id of the parameter</param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal SimpleParameter(int ID, string title, AbstractUnit[] desiredUnits,
            bool isInput = true,
            double lowerLimit = double.MinValue,
            double upperLimit = double.MaxValue)
        {
            this.ID = ID;
            this.Title = title;
            this.IsInput = isInput;
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

            DesiredUnits = new ObservableCollection<AbstractUnit>();
            foreach (AbstractUnit unit in desiredUnits)
            {
                this.DesiredUnits.Add(unit);
            }

            UnitSelection = new ObservableCollection<SimplePicker<AbstractUnit>>();
            for (int i = 0; i < desiredUnits.Length; i++)
            {
                UnitSelection.Add(new SimplePicker<AbstractUnit>(StaticUnitProperties.AllUnits[desiredUnits[i].GetType()])
                {
                    SelectedObject = desiredUnits[i]
                });
                UnitSelection[i].OnSelectedIndexChanged += UnitPicker_OnSelectedIndexChanged(i);
            }
        }

        /// <summary>
        /// The value in the current units (intended to be binded with field)
        /// </summary>
        public double Value
        {
            get
            {
                return GetValue();
            }
            set
            {
                SetValue(value);
                OnPropertyChanged();
            }
        }




        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public virtual bool AllowUserInput
        {
            get
            {
                return IsInput;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">ID of the Parameter which was just changed</param>
        public delegate void MadeOuputHandler(int ID);

        /// <summary>
        /// Called when this parameter is made to be an output
        /// </summary>
        public event MadeOuputHandler OnMadeOuput;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx">Index of UnitSelection</param>
        /// <returns></returns>
        private SimplePicker<AbstractUnit>.SelectedIndexChangedHandler UnitPicker_OnSelectedIndexChanged(int idx)
        {
            return delegate (AbstractComponent sender, EventArgs e)
            {
                OnReset();

                Placeholder = string.Format(LibraryResources.ParameterValidRange,
                    EffectiveLowerLimitString,
                    EffectiveUpperLimitString);

                if (this.IsOutput && double.TryParse(ValueStr, out double num))
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
                            DesiredUnits.ToArray(),
                            oldUnits);

                    SetValue(num);
                }
            };


        }

        private double _Value = 0.0;

        /// <summary>
        /// Gets the value ValueStr represents in the desired units
        /// </summary>
        private double GetValue()
        {
            OnReset();
            if (double.TryParse(ValueStr, out double temp))
            {

                temp = HelperFunctions.ConvertFrom(
                        temp,
                        DesiredUnits.ToArray(),
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
        private void SetValue(double value)
        {
            OnReset();
            double temp = HelperFunctions.ConvertTo(
                        value,
                        DesiredUnits.ToArray(),
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
            if (value < LowerLimit)
            {
                OnError(new Exception(string.Format(LibraryResources.ValueBelowLowerLimit, EffectiveLowerLimitString)));
            }
            else if (value > UpperLimit)
            {
                OnError(new Exception(string.Format(LibraryResources.ValueAboveUpperLimit, EffectiveUpperLimitString)));
            }
            else
            {
                OnSuccess();
            }

            _Value = temp;
            ValueStr = string.Format("{0:G4}", temp);
        }

        private string _ValueStr = string.Empty;
        /// <summary>
        /// String version of value (intended to be binded with field)
        /// </summary>
        public string ValueStr {
            get
            {
                return _ValueStr;
            }
            set
            {
                _ValueStr = value;
                OnPropertyChanged();
            }
        }

        bool _IsInput;
        public bool IsInput
        {
            get
            {
                return _IsInput;
            }
            set
            {
                _IsInput = value;
                if (!value)
                {
                    OnMadeOuput?.Invoke(this.ID);
                }
                OnPropertyChanged();
                OnPropertyChanged("IsOutput");
                OnPropertyChanged("AllowUserInput");
                OnPropertyChanged("Placeholder");
            }
        }

        public bool IsOutput
        {
            get
            {
                // should always be the opposite of isInput
                return !this.IsInput;
            }
            set
            {
                // property change event handled in the isInput parameter
                this.IsInput = !value;
            }
        }

        private string _Placeholder;
        public string Placeholder
        {
            get
            {
                if (IsOutput)
                {
                    return null;
                }
                return _Placeholder;
            }
            set
            {
                _Placeholder = value;
            }
        }


        /// <summary>
        /// Stores data related to the units the user picker for this parameter (Intented to binded with a picker)
        /// </summary>
        public ObservableCollection<SimplePicker<AbstractUnit>> UnitSelection { get; private set; }




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
                        DesiredUnits.ToArray(),
                        UnitSelection.Select(x => x.SelectedObject).ToArray());

                    if (double.IsNegativeInfinity(num))
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
                return string.Format("{0:G4}", num);
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
                        DesiredUnits.ToArray(),
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
        public ObservableCollection<AbstractUnit> DesiredUnits { get; private set; }


    }
}
