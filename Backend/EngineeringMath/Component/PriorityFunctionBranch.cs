using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class PriorityFunctionBranch : FunctionTreeNode, IQueuingSortedListItem<IParameterContainerNode>
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

        public override void Invalidate()
        {
            throw new NotImplementedException();
        }
    }
}
