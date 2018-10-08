using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class FunctionSetterLeaf : FunctionVisitableNodeLeaf
    {
        protected FunctionSetterLeaf() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="outputParameterVarName"></param>
        /// <param name="setter">changes the setting of the passed parameter, which will be the parameter with the matching outputVarName</param>
        public FunctionSetterLeaf(string name, string outputParameterVarName, Action<IParameter> setter) : base()
        {
            Name = name;
            OutputParameterVarName = outputParameterVarName;
            Setter = setter;
            Visitor = new FunctionVisitor(string.Empty, Calculate, DetermineParameterState)
            {
                Parent = this
            };
        }

        private void Calculate(IParameterContainerNode node)
        {
            Setter(node.FindParameter(OutputParameterVarName));
        }

        private ParameterState DetermineParameterState(IParameterContainerNode node, string varName)
        {
            return varName == OutputParameterVarName ? ParameterState.Output : ParameterState.Inactive;
        }

        public string OutputParameterVarName { get; set; }
        protected Action<IParameter> Setter { get; set; }

    }
}
