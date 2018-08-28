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
        public SIUnitParameter(string displayName, string varName, string unitCategoryName, 
            double minSIValue = double.MinValue, double maxSIValue = double.MaxValue) 
            : base(displayName, varName, minSIValue, maxSIValue)
        {
            UnitCategoryName = unitCategoryName;
        }

        protected override double BaseToBindValue(double value)
        {
            if(ParameterUnits.ItemAtSelectedIndex == null)
                return value;
            return UnitCategory.ConverterFromSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }

        protected override double BindToBaseValue(double value)
        {
            if (ParameterUnits.ItemAtSelectedIndex == null)
                return value;
            return UnitCategory.ConverterToSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }


        [XmlIgnore]
        private UnitCategory UnitCategory { get; set; }


        private SelectableList<Unit, Category<Unit>> _ParameterUnits;
        [XmlIgnore]
        public SelectableList<Unit, Category<Unit>> ParameterUnits
        {
            get { return _ParameterUnits; }
            set
            {
                if (_ParameterUnits != null)
                    _ParameterUnits.IndexChanged -= _ParameterUnits_IndexChanged;
                _ParameterUnits = value;
                _ParameterUnits.IndexChanged += _ParameterUnits_IndexChanged;
                OnPropertyChanged(nameof(DisplayDetail));
                OnPropertyChanged();
            }
        }

        private void _ParameterUnits_IndexChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(MinBindValue));
            OnPropertyChanged(nameof(MaxBindValue));
            OnPropertyChanged(nameof(Placeholder));
            OnPropertyChanged(nameof(DisplayDetail));
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
                UnitCategory = MathManager.AllUnits.GetCategoryByName(_UnitCategoryName);
                ParameterUnits = new SelectableList<Unit, Category<Unit>>(VarName, UnitCategory.Children);
                OnPropertyChanged();
            }
        }

        public override string DisplayDetail
        {
            get
            {
                return $"{string.Format("{0:G4}",BindValue)} {ParameterUnits.ItemAtSelectedIndex?.Symbol}";
            }
        }
    }
}
