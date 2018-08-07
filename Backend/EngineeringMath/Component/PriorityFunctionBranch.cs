using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class PriorityFunctionBranch : FunctionBranch, QueuingSortedListItem<string, IParameterContainerNode>
    {
        protected PriorityFunctionBranch() : base()
        {

        }

        public PriorityFunctionBranch(Function fun, uint priority) : base(fun)
        {
            Priority = priority;
        }
        public uint Priority { get; protected set; }
    }
}
