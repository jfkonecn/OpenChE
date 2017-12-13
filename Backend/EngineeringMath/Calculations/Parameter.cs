using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using System.ComponentModel;
using System.Diagnostics;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations
{
    public class Parameter : INotifyPropertyChanged
    {
        private static readonly string TAG = "Parameter:";
        /// <param name="title">Title of the field</param>
        /// <param name="ID">Id of the parameter</param>
        /// <param name="subFunctions">The functions which this parameter may be replaced by the string key is the title which is intended to be stored in a picker
        /// <para>EX: An area parameter can replaced by a function which calculates the area of a circle</para>
        /// </param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal Parameter( int ID, string title, AbstractUnit[] desiredUnits,
            Dictionary<string, FunctionFactory.FactoryData> subFunctions,
            bool isInput = true,              
            double lowerLimit = double.MinValue, 
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
            
            // Build SubFunctionSelection
            // add a default selection
            Dictionary<string, FunctionFactory.FactoryData> temp = new Dictionary<string, FunctionFactory.FactoryData>
                {
                    { "Direct Input", null }
                };
            if (subFunctions != null)
            {
                foreach (KeyValuePair<string, FunctionFactory.FactoryData> ele in subFunctions)
                    temp.Add(ele.Key, ele.Value);
            }
            this.SubFunctionSelection = new PickerSelection<FunctionFactory.FactoryData>(temp);
            this.SubFunctionSelection.OnSelectedIndexChanged += SubFunctionSelection_OnSelectedIndexChanged;

            this.ID = ID;
            this.LowerLimit = lowerLimit;
            this.UpperLimit = upperLimit;
            // prevent pointer sharing
            this.DesiredUnits = desiredUnits.ToArray();
            // don't use setter because this will call an event which is currently null
            _isInput = isInput;
            this.Title = title;

            UnitSelection = new PickerSelection<AbstractUnit>[desiredUnits.Length];
            for(int i = 0; i < desiredUnits.Length; i++)
            {
                UnitSelection[i] = new PickerSelection<AbstractUnit>(StaticUnitProperties.AllUnits[desiredUnits[i].GetType()]);
            }
            
           
        }

        /// <summary>
        /// 
        /// </summary>
        private void SubFunctionSelection_OnSelectedIndexChanged()
        {
            // free the memory being used
            _SubFunction = null;

            bool enable = true;
            if (SubFunctionSelection.SelectedObject != null)
            {
                enable = false;
            }
            foreach (PickerSelection<AbstractUnit> obj in UnitSelection)
            {
                obj.IsEnabled = enable;
            }
            AllowUserInputChanged();
        }

        /// <summary>
        /// Contains all of the functions which will be allowed to substituted out all the fields within the function (intended to binded with a picker
        /// </summary>
        public PickerSelection<FunctionFactory.FactoryData> SubFunctionSelection;


        private Function _SubFunction;
        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public Function SubFunction
        {
            get
            {
                if(SubFunctionSelection.SelectedObject == null)
                {
                    _SubFunction = null;
                }
                else
                {
                    // Do not rebuild if right function is already in place
                    if (_SubFunction == null || !SubFunctionSelection.SelectedObject.FunType.Equals(_SubFunction.GetType()))
                    {
                        Debug.WriteLine($"{TAG} Building a new SubFunction");
                        _SubFunction = FunctionFactory.BuildFunction(SubFunctionSelection.SelectedObject.FunType);


                        // double check that that the parameter is the type of unit as the desired output of the subfunction
                        Parameter outputPara = _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID];
                        for (int i = 0; i < outputPara.UnitSelection.Length; i++)
                        {
                            if(this.UnitSelection[i].GetType() != outputPara.UnitSelection[i].GetType())
                            {
                                throw new Exception("Output parameter in subfunction does not match this parameter's units");
                            }
                        }


                        for (int i = 0; i < _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].UnitSelection.Length; i++)
                        {
                            
                            _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].UnitSelection[i].SelectedObject = 
                                this.UnitSelection[i].SelectedObject;
                        }

                        _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].ValueStr = this.ValueStr;
                        _SubFunction.Title = this.Title;

                        // dont allow user to be able to change the output function
                        _SubFunction.OutputSelection.SelectedObject = this;
                        _SubFunction.OutputSelection.IsEnabled = false;

                        _SubFunction.OnSolve += delegate()
                        {
                            for (int i = 0; i < _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].UnitSelection.Length; i++)
                            {
                                this.UnitSelection[i].SelectedObject = 
                                _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].UnitSelection[i].SelectedObject;
                            }
                            
                            this.ValueStr = _SubFunction.FieldDic[SubFunctionSelection.SelectedObject.OuputID].ValueStr;
                        };


                    }
                    else
                    {
                        Debug.WriteLine($"{TAG} Keep old SubFunction");
                    }
                }

                return _SubFunction;
            }
        }
        public int ID { get; private set; }

        private double _Value = 0.0;

        /// <summary>
        /// Gets the value ValueStr represents
        /// </summary>
        public double GetValue()
        {
            // Reset error message
            ErrorMessage = null;

            double temp;
            if(double.TryParse(_ValueStr, out temp))
            {

                temp = HelperFunctions.ConvertFrom(
                        temp,
                        DesiredUnits,
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
                if (temp < LowerLimit)
                {
                    ErrorMessage = string.Format(LibraryResources.ValueBelowLowerLimit, LowerLimit);
                }
                else if (temp > UpperLimit)
                {
                    ErrorMessage = string.Format(LibraryResources.ValueAboveUpperLimit, UpperLimit);
                }
                else
                {
                    _Value = temp;
                    OnPropertyChanged("Value");
                }


            }
            else
            {
                ErrorMessage = string.Format(LibraryResources.NotANumber, LowerLimit);
            }



            return _Value;
        }


        string _ErrorMessage;
        /// <summary>
        /// String intended to be shown to the user when a bad input is given
        /// <para>A null ErrorMessage value implies that there is no error</para>
        /// </summary>
        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            internal set
            {
                _ErrorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
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
                        UnitSelection.Select(x => x.SelectedObject).ToArray());
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
        /// Stores data related to the units the user picker for this parameter (Intented to binded with a picker)
        /// </summary>
        public PickerSelection<AbstractUnit>[] UnitSelection { get; set; }


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

                if (!value)
                {
                    SubFunctionSelection.SelectedObject = null;
                    OnMadeOuput(this.ID);
                }

                SubFunctionSelection.IsEnabled = AllowUserInput;

                AllowUserInputChanged();
            }
        }

        /// <summary>
        /// calls all property change events when the allowed user input may have changed
        /// </summary>
        private void AllowUserInputChanged()
        {
            // This could change the state of allowing user inputs
            OnPropertyChanged("AllowUserInput");
            OnPropertyChanged("AllowSubFunctionClick");
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

        /// <summary>
        /// True when the this parameter should take a user input
        /// <para>It is false when this parameter is the output or when another function is calculating the this parameter's value</para>
        /// </summary>
        public bool AllowUserInput
        {
            get
            {
                if(this.SubFunctionSelection.SelectedObject == null)
                {
                    return isInput;
                }
                // another function is calculating the value of this parameter
                return false;
            }
        }


        /// <summary>
        /// True when user has selected a subfunction
        /// </summary>
        public bool AllowSubFunctionClick
        {
            get
            {
                return this.SubFunctionSelection.SelectedObject != null;
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
