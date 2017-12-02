using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using System.ComponentModel;

namespace EngineeringMath.Calculations
{
    public class Parameter : INotifyPropertyChanged
    {
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
            
            if(subFunctions != null)
            {
                // add a default selection
                Dictionary< string, FunctionFactory.FactoryData > temp = new Dictionary<string, FunctionFactory.FactoryData>
                {
                    { "Direct Input", null }
                };
                foreach (KeyValuePair<string, FunctionFactory.FactoryData> ele in subFunctions)
                    temp.Add(ele.Key, ele.Value);

                this.SubFunctionSelection = new PickerSelection<FunctionFactory.FactoryData>(temp);
            }

            
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
        /// Contains all of the functions which will be allowed to substituted out all the fields within the function (intended to binded with a picker
        /// </summary>
        public PickerSelection<FunctionFactory.FactoryData> SubFunctionSelection;

        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public Function ReplaceFunction { get; set; }
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
            UnitSelection.Select(x => x.SelectedObject).ToArray());
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
                    OnMadeOuput(this.ID);
                }

                // This could change the state of allowing user inputs
                OnPropertyChanged("AllowUserInput");
                OnPropertyChanged("DontAllowUserInput");
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

        /// <summary>
        /// True when the this parameter should take a user input
        /// <para>It is false when this parameter is the output or when another function is calculating the this parameter's value</para>
        /// </summary>
        public bool AllowUserInput
        {
            get
            {
                if(this.ReplaceFunction == null)
                {
                    return isInput;
                }
                // another function is calculating the value of this parameter
                return false;
            }
        }

        /// <summary>
        /// True when user input should not be allowed
        /// <para>This bool is always the opposite of AllowUserInput</para>
        /// </summary>
        public bool DontAllowUserInput
        {
            get
            {
                return !AllowUserInput;
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
