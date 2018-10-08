using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public abstract class VisitableNodeBuilder
    {
        public FunctionVisitableNodeLeaf Node { get; protected set; } = null;
        public abstract void BuildNode(string name);
        public abstract void BuildParameters();
        /// <summary>
        /// Lookup for parameter settings
        /// </summary>
        public abstract IEnumerable<NodeBuilderParameterSetting> ParameterSettings { get; }

        /// <summary>
        /// Builds all non-auto buildParameters and returns the result
        /// </summary>
        /// <returns>Keys are the varNames of the parameters</returns>
        public virtual Dictionary<string, IParameter> BuildNonautoBuildParameters()
        {
            Dictionary<string, IParameter> dic = new Dictionary<string, IParameter>();
            foreach (NodeBuilderParameterSetting setting in ParameterSettings)
            {
                if(!setting.AutoBuildParameter && setting.UseThisParameter)
                    dic.Add(setting.VarName, setting.BuildParameter());
            }
            return dic;
        }
    }
}
