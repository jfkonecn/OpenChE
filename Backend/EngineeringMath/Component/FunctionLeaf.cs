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


        public FunctionLeaf(string name, string equationExpression, string outputParameterName) : this()
        {
            Name = name;
            EquationExpression = equationExpression;
            OutputParameterName = outputParameterName;
        }

        public string OutputParameterName { get; protected set; }

        public string EquationExpression { get; protected set; }

        public override void Calculate()
        {
            SetBaseUnitValue(OutputParameterName, FunctionEquation.Evaluate());
        }

        public override void Invalidate()
        {
            throw new NotImplementedException();
        }
    }
}
