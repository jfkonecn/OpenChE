using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;
using static EngineeringMath.Component.Function;

namespace EngineeringMath.Component
{
    public class FunctionLeaf : FunctionTreeNodeWithParameters, IParameterContainerNode
    {
        protected FunctionLeaf() : base()
        {

        }

        


        public FunctionLeaf(string equationExpression, string outputParameterVarName, string name = "") : this()
        {
            OutputParameterVarName = outputParameterVarName;
            FunctionEquation = new Equation(equationExpression);
            Name = name;                
        }

        protected Equation FunctionEquation { get; set; }

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

        public string EquationExpression
        {
            get
            {
                return FunctionEquation.EquationExpression;
            }
            set
            {
                FunctionEquation.EquationExpression = value;
            }
        }

        public override void Calculate()
        {
            INumericParameter para = (INumericParameter)FindParameter(OutputParameterVarName);
            para.BaseValue = FunctionEquation.Evaluate((x) => { return ((INumericParameter)FindParameter(x)).BaseValue; });
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
            if (OutputParameterVarName.Equals(parameterName))
                return ParameterState.Output;
            if (FunctionEquation.IsInput(parameterName))
                return ParameterState.Input;
            return ParameterState.Inactive;
        }
    }
}
