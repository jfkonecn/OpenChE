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
            double upperLimit = double.MaxValue) : 
            this(ID, title, new NumericField(title, desiredUnits, isInput, lowerLimit, upperLimit), isInput)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="title"></param>
        /// <param name="field">The field attached to this object</param>
        /// <param name="isInput"></param>
        internal SimpleParameter(int ID, string title, NumericField field, bool isInput = true)
        {
            this.ID = ID;
            this.Title = title;
            this._Field = field;
            this.isInput = isInput;
            _Field.PropertyChanged += _Field_PropertyChanged;
            _Field.OnErrorEvent += OnError;
            _Field.OnResetEvent += OnReset;
            _Field.OnSuccessEvent += OnSuccess;
        }

        /// <summary>
        /// The value in the current units (intended to be binded with field)
        /// </summary>
        public double Value
        {
            get
            {
                return _Field.GetValue();
            }
            set
            {
                _Field.SetValue(value);
            }
        }


        private string _ValueStr = string.Empty;
        /// <summary>
        /// String version of value (intended to be binded with field)
        /// </summary>
        public string ValueStr
        {
            get
            {
                return _Field.ValueStr;
            }
            set
            {
                _Field.ValueStr = value;
            }
        }

        /// <summary>
        /// Stores the numeric information for this object
        /// </summary>
        private NumericField _Field;

        public bool isInput
        {
            get
            {
                return _Field.isInput;
            }
            set
            {
                _Field.isInput = value;

                if (!value)
                {
                    OnMadeOuput?.Invoke(this.ID);
                }
            }
        }

        public bool isOutput
        {
            get
            {
                return _Field.isOutput;
            }
            set
            {
                _Field.isOutput = value;
                if (value)
                {
                    OnMadeOuput?.Invoke(this.ID);
                }

            }
        }


        public string Placeholder
        {
            get
            {
                return _Field.Placeholder;
            }
            set
            {
                _Field.Placeholder = value;
            }
        }

        /// <summary>
        /// Stores data related to the units the user picker for this parameter (Intented to binded with a picker)
        /// </summary>
        public PickerSelection<AbstractUnit>[] UnitSelection
        {
            get
            {
                return _Field.UnitSelection;
            }

            set
            {
                _Field.UnitSelection = value;
            }

        }


        /// <summary>
        /// The lowest number the parameter is allowed to be
        /// </summary>
        public double LowerLimit { get { return _Field.LowerLimit; } }

        /// <summary>
        /// The highest number the paramter is allowed to be
        /// </summary>
        public double UpperLimit { get { return _Field.UpperLimit; } }


        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public virtual bool AllowUserInput
        {
            get
            {
                return isInput;
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
    }
}
