using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionLeaf : FunctionTreeNode, IParameterContainerLeaf
    {
        protected FunctionLeaf() : base()
        {
            FunctionEquation = new Equation(this);
        }

        [XmlIgnore]
        private Equation FunctionEquation { get; set;}


        public FunctionLeaf(string equationExpression) : this()
        {
            EquationExpression = equationExpression;
        }


        public string EquationExpression { get; protected set; }

        public override void Calculate()
        {
            FunctionEquation.Evaluate();
        }
    }
}
