using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    /// <summary>
    /// A parameter which represents a number input/output
    /// </summary>
    public interface INumericParameter : IParameter
    {

        double BaseValue { get; set; }
        double BindValue { get; set; }        
        string Placeholder { get; }
        double MinBaseValue { get; }
        double MaxBaseValue { get; }
        double MinBindValue { get; }
        double MaxBindValue { get; }
        /// <summary>
        /// Can be null
        /// </summary>
        SelectableList<Unit, Category<Unit>> ParameterUnits { get; }
    }

    public abstract class NumericParameter : NotifyPropertyChangedExtension, INumericParameter
    {
        protected NumericParameter()
        {

        }

        public NumericParameter(string displayName, string varName, double minBaseValue, double maxBaseValue)
        {
            MinBaseValue = minBaseValue;
            MaxBaseValue = maxBaseValue;
            DisplayName = displayName;
            VarName = varName;
            _BaseValue = MinBaseValue <= 0 ? 0 : MinBaseValue;
        }


        protected abstract double BaseToBindValue(double value);

        protected abstract double BindToBaseValue(double value);



        private double _BaseValue = double.NaN;
        /// <summary>
        /// Used to get value from this parameter for functions
        /// </summary>
        [XmlIgnore]
        public double BaseValue
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
                OnPropertyChanged(nameof(DisplayDetail));
            }
        }


        public string Placeholder
        {
            get
            {
                return string.Format(LibraryResources.Placeholder, MinBindValue, MaxBindValue);
            }
        }

        private double _BindValue;
        /// <summary>
        /// Value the user sees and may change
        /// </summary>
        [XmlIgnore]
        public double BindValue
        {
            get { return _BindValue; }
            set
            {
                double num = value;
                if (num > MaxBindValue || num < MinBindValue)
                {
                    num = double.NaN;
                }

                _BindValue = num;
                // call _BaseValue to prevent stack overflow
                _BaseValue = BindToBaseValue(_BindValue);
                OnPropertyChanged(nameof(DisplayDetail));
                OnPropertyChanged();
            }
        }


        [XmlIgnore]
        public double MinBindValue
        {
            get { return BaseToBindValue(MinBaseValue); }
        }

        [XmlIgnore]
        public double MaxBindValue
        {
            get { return BaseToBindValue(MaxBaseValue); }
        }



        public override string ToString()
        {
            return VarName;
        }

        private string _DisplayName;
        /// <summary>
        /// Name used in UI
        /// </summary>
        public string DisplayName
        {
            get { return _DisplayName; }
            protected set
            {
                _DisplayName = value;
                OnPropertyChanged(nameof(DisplayDetail));
                OnPropertyChanged();
            }
        }

        private string _VarName;
        /// <summary>
        /// Name used for calculations
        /// </summary>
        public string VarName
        {
            get { return _VarName; }
            protected set
            {
                _VarName = value;
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


        /// <summary>
        /// Provides greater detail about this parameter
        /// </summary>
        public abstract string DisplayDetail { get; }

        private ParameterState _CurrentState = ParameterState.Inactive;

        public event EventHandler<EventArgs> StateChanged;
        private void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public ParameterState CurrentState
        {
            get
            {
                return _CurrentState;
            }
            set
            {
                if (CurrentState == value)
                    return;
                _CurrentState = value;
                OnStateChanged();
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        private IParameterContainerNode _Parent;


        public IParameterContainerNode Parent
        {
            get
            {
                return _Parent;
            }
            internal set
            {
                IChildItemDefaults.DefaultSetParent(ref _Parent, OnParentChanged, value, Parent_ParentChanged);
            }
        }
        protected virtual void OnParentChanged(ParentChangedEventArgs e)
        {
            ParentChanged?.Invoke(this, e);
        }
        private void Parent_ParentChanged(object sender, ParentChangedEventArgs e)
        {
            OnParentChanged(e);
        }
        public event EventHandler<ParentChangedEventArgs> ParentChanged;

        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent { get => Parent; set => Parent = value; }

        string IChildItem<IParameterContainerNode>.Key => VarName;
        [XmlIgnore]
        public abstract SelectableList<Unit, Category<Unit>> ParameterUnits { get; protected set; }

    }
}
