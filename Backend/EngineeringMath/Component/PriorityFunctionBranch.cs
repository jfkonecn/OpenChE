using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class PriorityFunctionBranch : FunctionTreeNode, IQueuingSortedListItem
    {
        protected PriorityFunctionBranch() : base()
        {
            NextNode.Parent = this;
        }

        public PriorityFunctionBranch(string name, uint priority, FunctionTreeNode nextNode) : base()
        {
            Name = name;
            NextNode = nextNode;
            NextNode.Parent = this;
            Priority = priority;
        }
        public uint Priority { get; protected set; }

        public FunctionTreeNode NextNode { get; protected set; }

        public override void Calculate()
        {
            NextNode.Calculate();
        }


        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            NextNode.BuildLists(settings, parameter);
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            NextNode.DeactivateStates();
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
            NextNode.ActivateStates();
        }

        public override bool IsOutput(string parameterName)
        {
            return NextNode.IsOutput(parameterName);
        }
    }
}
