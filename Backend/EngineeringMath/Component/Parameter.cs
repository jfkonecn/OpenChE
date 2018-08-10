using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class Parameter<T> : NotifyPropertyChangedExtension, IParameter
        where T : IComparable
    {
        protected Parameter()
        {

        }

        public Parameter(string name, double minBaseValue, double maxBaseValue)
        {
            MinBaseValue = minBaseValue;
            MaxBaseValue = maxBaseValue;
            Name = name;
        }


        protected abstract T BaseToBindValue(double value);

        protected abstract double BindToBaseValue(T value);


        private string _Name;
        public string Name
        {
            get { return _Name; }
            protected set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private double _MinBaseValue;
        /// <summary>
        /// In SI units
        /// </summary>
        public double MinBaseValue
        {
            get { return _MinBaseValue; }
            protected set
            {
                _MinBaseValue = value;
                OnPropertyChanged();
            }
        }

        private double _MaxBaseValue;
        /// <summary>
        /// In SI units
        /// </summary>
        public double MaxBaseValue
        {
            get { return _MaxBaseValue; }
            protected set
            {
                _MaxBaseValue = value;
                OnPropertyChanged();
            }
        }


        private double _BaseValue = double.NaN;
        /// <summary>
        /// Used to get value from this parameter for functions
        /// </summary>
        [XmlIgnore]
        public double BaseUnitValue
        {
            get { return _BaseValue; }
            set
            {
                double num = value;
                if (num > MaxBaseValue || num < MinBaseValue)
                {
                    num = double.NaN;
                }
                _BaseValue = num;
                // call _BindValue to prevent stack overflow
                _BindValue = BaseToBindValue(_BaseValue);
                OnPropertyChanged(nameof(BindValue));
            }
        }

        private T _BindValue = default(T);
        /// <summary>
        /// Value the user sees and may change
        /// </summary>
        [XmlIgnore]
        public T BindValue
        {
            get { return _BindValue; }
            set
            {
                T num = value;
                if (num.Equals(default(T)) || num.CompareTo(MaxBaseValue) > 0 || num.CompareTo(MinBindValue) < 0)
                {
                    num = default(T);
                }

                _BindValue = num;
                // call _BaseValue to prevent stack overflow
                _BaseValue = BindToBaseValue(_BindValue);
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        public T MinBindValue
        {
            get { return BaseToBindValue(MinBaseValue); }
        }

        [XmlIgnore]
        public T MaxBindValue
        {
            get { return BaseToBindValue(MaxBaseValue); }
        }


        [XmlIgnore]
        public IParameterContainerNode ParentObject { get; internal set; }



        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent
        {
            get
            {
                return this.ParentObject;
            }
            set
            {
                this.ParentObject = value;
            }
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
