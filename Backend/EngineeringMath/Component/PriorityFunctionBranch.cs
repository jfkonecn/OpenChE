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
            Name = string.Empty;
        }

        public PriorityFunctionBranch(uint priority, FunctionTreeNode nextNode) : base()
        {
            NextNode = nextNode ?? throw new ArgumentNullException(nameof(nextNode));
            NextNode.Parent = this;
            Priority = priority;
            Name = Guid.NewGuid().ToString();
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

        public override ParameterState DetermineState(string paraVarName)
        {
            return NextNode.DetermineState(paraVarName);
        }
    }
}
