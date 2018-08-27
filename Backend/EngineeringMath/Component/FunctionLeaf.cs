using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Resources;

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


        public FunctionLeaf(string equationExpression, string outputParameterVarName) : this()
        {
            EquationExpression = equationExpression;
            OutputParameterVarName = outputParameterVarName;
        }


        public override string Name
        {
            get
            {
                return string.Format(LibraryResources.SolveFor, FindParameter(OutputParameterVarName).DisplayName);
            }
            protected set
            {
                throw new NotSupportedException();
            }

        }
        public string OutputParameterVarName { get; set; }

        public string EquationExpression { get; set; }

        public override void Calculate()
        {
            IParameter para = FindParameter(OutputParameterVarName);
            para.BaseUnitValue = FunctionEquation.Evaluate();
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            foreach(IParameter para in this.Parameters)
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
            return OutputParameterVarName.Equals(parameterName);
        }
    }
}
