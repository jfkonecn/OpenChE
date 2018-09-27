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
            Func<IParameterContainerNode, string, bool> isOutputFun)
        {
            Name = name ?? string.Empty;
            CalculateAct = calculateAct ?? throw new ArgumentNullException(nameof(calculateAct));
            IsOutputFun = isOutputFun ?? throw new ArgumentNullException(nameof(isOutputFun));
        }


        protected Action<IParameterContainerNode> CalculateAct { get; }
        protected Func<IParameterContainerNode, string, bool> IsOutputFun { get; }



        public override void Calculate()
        {
            CalculateAct(this);
        }

        public override bool IsOutput(string varName)
        {
            return IsOutputFun(this, varName);
        }

    }
}
