using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class SIUnitParameter : Parameter, ISpaceSaver 
    {

        public SIUnitParameter(string name, double minSIValue, double maxSIValue) : base(name)
        {
            MinSIValue = minSIValue;
            MaxSIValue = maxSIValue;
        }

        private double SIValueToValue(double value)
        {
            // TODO: Make unit converter
            return value;
        }

        private double ValueToSIValue(double value)
        {
            // TODO: Make unit converter
            return value;
        }




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

        

        public void Nullify()
        {
            throw new NotImplementedException();
        }
    }
}
