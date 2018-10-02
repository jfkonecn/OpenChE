using EngineeringMath.Component.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionVisitor : FunctionTreeNodeWithParameters
    {
        protected FunctionVisitor()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="calculateAct">called when calcuate method is called</param>
        /// <param name="isOutputFun">called when isOuputFun is called</param>
        public FunctionVisitor(string name, 
            Action<IParameterContainerNode> calculateAct, 
            Func<IParameterContainerNode, string, ParameterState> determineStateFun)
        {
            Name = name ?? string.Empty;
            CalculateAct = calculateAct ?? throw new ArgumentNullException(nameof(calculateAct));
            DetermineStateFun = determineStateFun ?? throw new ArgumentNullException(nameof(determineStateFun));
        }


        protected Action<IParameterContainerNode> CalculateAct { get; }
        protected Func<IParameterContainerNode, string, ParameterState> DetermineStateFun { get; }



        public override void Calculate()
        {
            CalculateAct(this);
        }

        public override ParameterState DetermineState(string varName)
        {
            return DetermineStateFun(this, varName);
        }

    }
}
