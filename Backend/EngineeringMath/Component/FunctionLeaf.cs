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


        public FunctionLeaf(string equationExpression, string outputParameterVarName, string name = "") : this()
        {
            EquationExpression = equationExpression;
            OutputParameterVarName = outputParameterVarName;
            Name = name;                
        }

        private string _Name = string.Empty;
        public override string Name
        {
            get
            {
                if (_Name == "")
                    return string.Format(LibraryResources.SolveFor, FindParameter(OutputParameterVarName).DisplayName);

                return _Name;

            }
            protected set
            {
                if (_Name.Equals(value))
                    return;
                _Name = value;
                OnPropertyChanged();
            }
        }
        public string OutputParameterVarName { get; set; }

        public string EquationExpression { get; set; }

        public override void Calculate()
        {
            INumericParameter para = (INumericParameter)FindParameter(OutputParameterVarName);
            para.BaseValue = FunctionEquation.Evaluate();
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

        public override ParameterState DetermineState(string parameterName)
        {
            return OutputParameterVarName.Equals(parameterName) ? ParameterState.Output : ParameterState.Input;
        }
    }
}
