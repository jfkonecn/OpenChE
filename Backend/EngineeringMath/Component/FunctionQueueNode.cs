using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EngineeringMath.Component
{
    public class FunctionQueueNode : FunctionTreeNodeWithParameters
    {
        protected FunctionQueueNode() : base()
        {
            Children = new QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode>(this);
        }

        internal FunctionQueueNode(Function fun, string name) : this(name)
        {
            Parent = fun;
        }

        public FunctionQueueNode(string name) : this()
        {
            Name = name;
        }
        public QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode> Children { get; protected set; }

        public override void Calculate()
        {
            foreach (PriorityFunctionBranch branch in Children.GetQueue())
            {
                branch.Calculate();
            }
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            foreach (IParameter para in this.Parameters)
            {
                parameter.Add(para);
            }
            foreach (FunctionTreeNode node in Children)
            {
                node.BuildLists(settings, parameter);
            }
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            foreach (FunctionTreeNode node in Children)
            {
                node.DeactivateStates();
            }
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
            foreach (FunctionTreeNode node in Children)
            {
                node.ActivateStates();
            }
        }

        public override bool IsOutput(string parameterName)
        {
            foreach (FunctionTreeNode node in Children)
            {
                if (node.IsOutput(parameterName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
