using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public abstract class NodeBuilderParameterSetting
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="buildParameter">true if you want the builder to create the parameter for you</param>
        public NodeBuilderParameterSetting(string displayName, string varName)
        {
            DisplayName = displayName;
            VarName = varName;
        }

        public string DisplayName { get; set; }
        public string VarName { get; set; }
        /// <summary>
        /// True if this parameter will be built by the builder
        /// </summary>
        public bool AutoBuildParameter { get; set; } = true;
        /// <summary>
        /// True if this parameter will be used in calculations
        /// </summary>
        public bool UseThisParameter { get; set; } = true;
        public abstract IParameter BuildParameter();

    }
}
