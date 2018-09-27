using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public abstract class NumericParameterSetting : NodeBuilderParameterSetting
    {

        public NumericParameterSetting(string displayName, string varName, 
            double minBaseValue, double maxBaseValue) : base(displayName, varName)
        {
            MinBaseValue = minBaseValue;
            MaxBaseValue = maxBaseValue;
        }



        public double MinBaseValue { get; }
        public double MaxBaseValue { get; }
    }
}
