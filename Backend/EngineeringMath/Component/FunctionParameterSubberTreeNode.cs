using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Allows parameter to be replaced by different input and output parameters. 
    /// The parameter being replaced is still used in calculations
    /// </summary>
    public class FunctionParameterSubberTreeNode : FunctionTreeNode
    {
        protected FunctionParameterSubberTreeNode() : base()
        {
            ParentChanged += FunctionParameterSubberTreeNode_ParentChanged;
        }

        private void FunctionParameterSubberTreeNode_ParentChanged(object sender, EventArgs e)
        {
            MainBranch.Parent = ParentObject;
        }

        /// <summary>
        /// Use # in the equation expression to represent the Default Parameter
        /// </summary>
        /// <param name="replacingParameter"></param>
        /// <param name="replacmentFunctions"></param>
        public FunctionParameterSubberTreeNode(string name, IParameter replacingParameter, params IParameterContainerLeaf[] replacmentFunctions) : this()
        {
            ReplacingParameter = replacingParameter;            
            Name = name;


            InputBranch = new FunctionBranch(string.Format(LibraryResources.ReplaceParameterName, ReplacingParameter.DisplayName))
            {

            };
            InputBranch.Children.TopValue = new FunctionOutputMakerNode(LibraryResources.DontReplaceParameter, ReplacingParameter.DisplayName); ;

            OutputBranch = new FunctionBranch(string.Format(LibraryResources.ReplaceParameterName, ReplacingParameter.DisplayName))
            {

            };
            OutputBranch.Children.TopValue = new FunctionOutputMakerNode(LibraryResources.DontReplaceParameter);



            MainBranch = new FunctionBranch(ParentObject, string.Empty)
            {
                Parameters =
                {
                    ReplacingParameter
                },
                Children =
                {
                    InputBranch,
                    OutputBranch
                }
            };


            foreach (IParameterContainerLeaf node in replacmentFunctions)
            {
                AddLeafNode(node);
            }
        }


        /// <summary>
        /// Adds a node which either uses ReplacingParameter as an input or an output.
        /// The node must contain all references to other parameters within itself. This node will not be able to see parameters in other nodes.
        /// To reference the ReplacingParameter in the EquationExpression or OutputParameterName simply use "#" (without quotes)
        /// </summary>
        /// <param name="node"></param>
        public void AddLeafNode(IParameterContainerLeaf node)
        {
            string replacingStr = "#";

            if ((node.EquationExpression.Contains(replacingStr) && node.OutputParameterVarName.Equals(replacingStr)) ||
                (!node.EquationExpression.Contains(replacingStr) && !node.OutputParameterVarName.Equals(replacingStr))
                )
            {
                throw new ArgumentException("The expression must be an input XOR output");
            }

            if (node.EquationExpression.Contains(replacingStr))
            {
                node.EquationExpression = node.EquationExpression.Replace(replacingStr, ReplacingParameter.VarName);
            }
            else
            {
                node.OutputParameterVarName = node.OutputParameterVarName.Replace(replacingStr, ReplacingParameter.VarName);
            }
        }


        public FunctionTreeNode NextNode { get; set; }

        public string Category { get; protected set; }

        private IParameter _ReplacingParameter;
        public IParameter ReplacingParameter
        {
            get
            {
                return _ReplacingParameter;
            }
            protected set
            {
                if (_ReplacingParameter.Equals(value))
                    return;
                if (!_ReplacingParameter.Equals(null))
                    ParameterRemoved(_ReplacingParameter);
                _ReplacingParameter = value;
                if (!value.Equals(null))
                    ParameterAdded(_ReplacingParameter);
            }
        }


        /// <summary>
        /// Contains both input and output branches
        /// </summary>
        protected FunctionBranch MainBranch { get; set; }


        /// <summary>
        /// Branch which contains functions which use the ReplacingParameter as an input
        /// </summary>
        protected FunctionBranch InputBranch { get; set; }

        /// <summary>
        /// Branch which contains functions which use the ReplacingParameter as an output
        /// </summary>
        protected FunctionBranch OutputBranch { get; set; }

        /// <summary>
        /// Returns a FunctionNullNode branch if ReplacingParameter is not an input or output
        /// </summary>
        [XmlIgnore]
        public FunctionTreeNode ActiveNode
        {
            get
            {
                switch (ReplacingParameter.CurrentState)
                {
                    case ParameterState.Input:
                        return InputBranch;
                    case ParameterState.Output:
                        return OutputBranch;
                    case ParameterState.Inactive:
                    default:
                        return new FunctionNullNode(string.Empty);
                }
            }
        }




        public override void ActivateStates()
        {
            base.ActivateStates();
            MainBranch.ActivateStates();
            NextNode.ActivateStates();
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            MainBranch.BuildLists(settings, parameter);
            NextNode.BuildLists(settings, parameter);
        }

        public override void Calculate()
        {
            ActiveNode.Calculate();
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            MainBranch.DeactivateStates();
            NextNode.DeactivateStates();
        }

        public override IParameter FindParameter(string paraVarName)
        {
            if (ReplacingParameter.VarName.Equals(paraVarName))
                return ReplacingParameter;
            return ParentObject.FindParameter(paraVarName);
        }

        public override bool IsOutput(string parameterName)
        {
            return NextNode.IsOutput(parameterName);
        }
    }
}
