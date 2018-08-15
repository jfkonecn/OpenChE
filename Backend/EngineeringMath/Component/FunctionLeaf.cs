using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionLeaf : FunctionTreeNodeWithParameters, IParameterContainerLeaf
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

        public string OutputParameterName { get; set; }

        public string EquationExpression { get; set; }

        public override void Calculate()
        {
            SetBaseUnitValue(OutputParameterName, FunctionEquation.Evaluate());
        }

        public override void BuildLists(List<ISetting> settings, List<Parameter> parameter)
        {
            foreach(Parameter para in this.Parameters)
            {
                parameter.Add(para);
            }
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
        }

        public override bool IsOutput(string parameterName)
        {
            return OutputParameterName.Equals(parameterName);
        }
    }
}
