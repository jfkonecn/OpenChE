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
            Children = new QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode>(this);
        }

        internal FunctionQueueNode(Function fun, string name) : this(name)
        {
            ParentObject = fun;
        }

        public FunctionQueueNode(string name) : this()
        {
            Name = name;
        }

        private QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode> _Children;
        public QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode> Children
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

        public override void Invalidate()
        {
            throw new NotImplementedException();
        }
    }
}
