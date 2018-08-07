using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionBranch : FunctionTreeNode
    {
        protected FunctionBranch()
        {
            Children = new SelectableList<string, FunctionTreeNode, IParameterContainerNode>(this);
        }

        public FunctionBranch(Function fun) : this()
        {
            ParentObject = fun;
        }

        private SelectableList<string, FunctionTreeNode, IParameterContainerNode> _Children;
        public SelectableList<string, FunctionTreeNode, IParameterContainerNode> Children
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
    }
}
