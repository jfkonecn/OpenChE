using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class PickerParameterSetting<T> : NodeBuilderParameterSetting
    {

        public PickerParameterSetting(string displayName, string varName, 
            params IPickerParameterOption<T>[] options) : base(displayName, varName)
        {
            Options = options;
        }


        public override IParameter BuildParameter()
        {
            return new PickerParameter<T>(DisplayName, VarName, Options);
        }

        public IPickerParameterOption<T>[] Options { get; }
    }
}
