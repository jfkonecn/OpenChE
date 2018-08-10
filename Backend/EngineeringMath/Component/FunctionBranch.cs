using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionBranch : FunctionTreeNode
    {
        protected FunctionBranch() : base()
        {
            Children = new SelectableList<FunctionTreeNode, IParameterContainerNode>(this);
        }

        internal FunctionBranch(Function fun, string name) : this(name)
        {
            Parent = fun;
        }

        public FunctionBranch(string name) : this()
        {
            Name = name;
        }

        private SelectableList<FunctionTreeNode, IParameterContainerNode> _Children;
        public SelectableList<FunctionTreeNode, IParameterContainerNode> Children
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
            Children.ItemAtSelectedIndex.Calculate();
        }

        public override void Invalidate()
        {
            throw new NotImplementedException();
        }
    }
}
