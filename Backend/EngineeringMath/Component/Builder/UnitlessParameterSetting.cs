using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class UnitlessParameterSetting : NumericParameterSetting
    {
        public UnitlessParameterSetting(string displayName, string varName, 
            double minBaseValue = double.MinValue, double maxBaseValue = double.MaxValue) :
            base(displayName, varName, minBaseValue, maxBaseValue)
        {

        }

        public override IParameter BuildParameter()
        {
            return new UnitlessParameter(DisplayName, VarName, MinBaseValue, MaxBaseValue);
        }
    }
}
