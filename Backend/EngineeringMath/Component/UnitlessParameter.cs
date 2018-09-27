using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class UnitlessParameter : NumericParameter
    {
        public override string DisplayDetail => $"{string.Format("{0:G4}", BindValue)}";

        public override SelectableList<Unit, Category<Unit>> ParameterUnits
        {
            get
            {
                return null;
            }
            protected set
            {
                return;
            }
        }

        protected UnitlessParameter() : base()
        {

        }
        public UnitlessParameter(string displayName, string varName, 
            double minValue = double.MinValue, double maxValue = double.MaxValue) : base(displayName, varName, minValue, maxValue)
        {

        }

        protected override double BaseToBindValue(double value)
        {
            return value;
        }

        protected override double BindToBaseValue(double value)
        {
            return value;
        }
    }
}

