using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EngineeringMath.Component
{
    public class FunctionQueueNode : FunctionTreeNode
    {
        protected FunctionQueueNode() : base()
        {
            Children = new QueuingSortedList<string, PriorityFunctionBranch, IParameterContainerNode>(this);
        }

        public FunctionQueueNode(Function fun) : this()
        {
            ParentObject = fun;
        }

        private QueuingSortedList<string, PriorityFunctionBranch, IParameterContainerNode> _Children;
        public QueuingSortedList<string, PriorityFunctionBranch, IParameterContainerNode> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }

        public override void Calculate()
        {
            foreach (PriorityFunctionBranch branch in Children.GetQueue())
            {
                branch.Calculate();
            }
        }
    }
}
