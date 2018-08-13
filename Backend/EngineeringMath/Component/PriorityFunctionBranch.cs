using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class PriorityFunctionBranch : FunctionTreeNode, IQueuingSortedListItem
    {
        protected PriorityFunctionBranch() : base()
        {

        }

        public PriorityFunctionBranch(string name, uint priority, FunctionTreeNode nextNode) : base(name)
        {
            NextNode = nextNode;
            Priority = priority;
        }
        public uint Priority { get; protected set; }

        public FunctionTreeNode NextNode { get; protected set; }

        public override void Calculate()
        {
            NextNode.Calculate();
        }


        public override void BuildLists(List<ISetting> settings, List<Parameter> parameter)
        {
            foreach (Parameter para in this.Parameters)
            {
                parameter.Add(para);
            }
            NextNode.BuildLists(settings, parameter);
        }

        public override void DeactivateStates()
        {
            CurrentState = FunctionTreeNodeState.Inactive;
            NextNode.DeactivateStates();
        }

        public override void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
            NextNode.ActivateStates();
        }

        public override bool IsOutput(string parameterName)
        {
            return NextNode.IsOutput(parameterName);
        }
    }
}
