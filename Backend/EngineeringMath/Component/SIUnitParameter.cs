using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class SIUnitParameter : Parameter
    {
        protected SIUnitParameter() : base()
        {

        }
        public SIUnitParameter(string name, string unitCategoryName, double minSIValue = double.MinValue, double maxSIValue = double.MaxValue) : base(name)
        {
            MinSIValue = minSIValue;
            MaxSIValue = maxSIValue;
            UnitCategoryName = unitCategoryName;
        }

        private double SIValueToValue(double value)
        {
            return GetUnitCategory().ConvertUnit(GetSIUnitName(), ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }

        private double ValueToSIValue(double value)
        {
            return GetUnitCategory().ConvertUnit(ParameterUnits.ItemAtSelectedIndex.FullName, GetSIUnitName(), value);
        }

        private SelectableList<Unit, UnitCategory> _ParameterUnits;
        [XmlIgnore]
        public SelectableList<Unit, UnitCategory> ParameterUnits
        {
            get { return _ParameterUnits; }
            set
            {
                _ParameterUnits = value;
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        private Func<string> GetSIUnitName;

        [XmlIgnore]
        private Func<UnitCategory> GetUnitCategory;


        private double _Value = double.NaN;
        [XmlIgnore]
        public double Value
        {
            get { return _Value; }
            set
            {
                double num = value;
                if (num > MaxValue || num < MinValue)
                {
                    num = double.NaN;
                }

                _Value = num;
                // call _SIValue to prevent stack overflow
                _SIValue = ValueToSIValue(_Value);
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        public double MinValue
        {
            get { return SIValueToValue(MinSIValue); }
        }

        [XmlIgnore]
        public double MaxValue
        {
            get { return SIValueToValue(MaxSIValue); }
        }

        private double _SIValue = double.NaN;

        /// <summary>
        /// The value of this parameter in SI units
        /// </summary>
        [XmlIgnore]
        public double SIValue
        {
            get { return _SIValue; }
            private set
            {
                _SIValue = value;
                // call _Value to prevent stack overflow
                _Value = SIValueToValue(_SIValue);
                OnPropertyChanged(nameof(Value));
            }
        }


        private double _MinSIValue = double.MinValue;
        /// <summary>
        /// In SI units
        /// </summary>
        public double MinSIValue
        {
            get { return _MinSIValue; }
            private set
            {
                _MinSIValue = value;
                OnPropertyChanged();
            }
        }

        private double _MaxSIValue;
        /// <summary>
        /// In SI units
        /// </summary>
        public double MaxSIValue
        {
            get { return _MaxSIValue; }
            private set
            {
                _MaxSIValue = value;
                OnPropertyChanged();
            }
        }
        private string _UnitCategoryName;
        public string UnitCategoryName
        {
            get
            {
                return _UnitCategoryName;
            }
            set
            {
                _UnitCategoryName = value;
                UnitCategory temp = MathManager.AllUnits.GetUnitCategoryByName(_UnitCategoryName);
                // pointer share so that you can unit data is current
                GetUnitCategory = () => { return temp; };                
                GetSIUnitName = () => { return GetUnitCategory().GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI); };
                ParameterUnits = new SelectableList<Unit, UnitCategory>(temp.Children);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Bind to SIValue!!
        /// </summary>
        public override double BaseUnitValue
        {
            get
            {
                return SIValue;
            }
            set
            {
                SIValue = value;
            }
        }
    }
}
