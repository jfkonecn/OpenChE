using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class SIParameterSetting : NumericParameterSetting
    {
        public SIParameterSetting(string displayName, string varName, string unitName, 
            double minBaseValue = double.MinValue, double maxBaseValue = double.MaxValue) 
            : base(displayName, varName, minBaseValue, maxBaseValue)
        {
            UnitName = unitName;
        }

        public override IParameter BuildParameter()
        {
            return new SIUnitParameter(DisplayName, VarName, UnitName, MinBaseValue, MaxBaseValue);
        }

        public string UnitName { get; }
    }
}
