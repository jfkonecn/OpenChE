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

        public string OutputParameterName { get; protected set; }

        public string EquationExpression { get; protected set; }

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
            CurrentState = FunctionTreeNodeState.Inactive;
            foreach(Parameter para in Parameters)
            {
                para.CurrentState = ParameterState.Inactive;
            }
        }

        public override void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
            foreach (Parameter para in Parameters)
            {
                if (IsOutput(para.Name))
                {
                    para.CurrentState = ParameterState.Output;
                }
                else
                {
                    para.CurrentState = ParameterState.Input;
                }
            }
        }

        public override bool IsOutput(string parameterName)
        {
            return OutputParameterName.Equals(parameterName);
        }
    }
}
