using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionBranch : FunctionTreeNodeWithParameters
    {
        protected FunctionBranch() : base()
        {
            Children = new SelectableList<FunctionTreeNode, IParameterContainerNode>(Name, this);
            Children.ItemAdded += Children_ItemAdded;
        }

        private void Children_ItemAdded(object sender, ItemEventArgs<FunctionTreeNode> e)
        {
            FunctionTreeNode node = e.ModifiedItem;
            if (node.Equals(Children.ItemAtSelectedIndex))
            {
                node.ActivateStates();
            }
            else
            {
                node.DeactivateStates();
            }
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

        public override void BuildLists(List<ISetting> settings, List<Parameter> parameter)
        {
            foreach (Parameter para in this.Parameters)
            {
                parameter.Add(para);
            }
            settings.Add(Children);

            foreach(FunctionTreeNode node in Children)
            {
                node.BuildLists(settings, parameter);
            }
        }

        public override void DeactivateStates()
        {
            Children.CurrentState = SettingState.Inactive;
            CurrentState = FunctionTreeNodeState.Inactive;
            foreach (FunctionTreeNode node in Children)
            {
                node.DeactivateStates();
            }
        }

        public override void ActivateStates()
        {
            Children.CurrentState = SettingState.Active;
            foreach (FunctionTreeNode node in Children)
            {
                if (Children.ItemAtSelectedIndex.Equals(node))
                {
                    node.ActivateStates();
                }
                else
                {
                    node.DeactivateStates();
                }                
            }
        }

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            if (Parent == null)
                return;
            Parent.SettingAdded(Children);
        }


        public override bool IsOutput(string parameterName)
        {
            return Children.ItemAtSelectedIndex.IsOutput(parameterName);
        }
    }
}
