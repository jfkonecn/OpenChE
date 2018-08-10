using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class SIUnitParameter : Parameter<double>
    {
        protected SIUnitParameter() : base()
        {
            
        }
        public SIUnitParameter(string name, string unitCategoryName, double minSIValue = double.MinValue, double maxSIValue = double.MaxValue) : base(name, minSIValue, maxSIValue)
        {

            UnitCategoryName = unitCategoryName;
        }

        protected override double BaseToBindValue(double value)
        {
            return UnitCategory.ConverterFromSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }

        protected override double BindToBaseValue(double value)
        {
            return UnitCategory.ConverterToSIUnit(ParameterUnits.ItemAtSelectedIndex.FullName, value);
        }


        [XmlIgnore]
        private ISIParameterUnitCategory UnitCategory { get; set; }


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
                UnitCategory = MathManager.AllUnits.GetUnitCategoryByName(_UnitCategoryName);
                ParameterUnits = new SelectableList<Unit, UnitCategory>(UnitCategory.Children);
                OnPropertyChanged();
            }
        }

    }
}
