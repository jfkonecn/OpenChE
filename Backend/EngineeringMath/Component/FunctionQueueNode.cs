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

        internal FunctionQueueNode(Function fun, string name) : this(name)
        {
            ParentObject = fun;
        }

        public FunctionQueueNode(string name) : this()
        {
            Name = name;
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
