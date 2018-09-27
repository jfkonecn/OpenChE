using EngineeringMath.Component.CustomEventArgs;
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
            FinishBuilding();
        }

        private void Children_IndexChanged(object sender, EventArgs e)
        {
            OnStateChanged();
        }

        private void Children_ItemAdded(object sender, ItemEventArgs<IParameterContainerNode> e)
        {
            IParameterContainerNode node = e.ModifiedItem;
            if (node.Equals(Children.ItemAtSelectedIndex))
            {
                node.ActivateStates();
            }
            else
            {
                node.DeactivateStates();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        internal FunctionBranch(IParameterContainerNode parent, string name) : this(name)
        {
            Parent = parent;
        }

        public FunctionBranch(string name) : base()
        {
            Name = name;
            FinishBuilding();
        }

        private void FinishBuilding()
        {
            Children = new SelectableList<IParameterContainerNode, IParameterContainerNode>(Name, this);
            Children.ItemAdded += Children_ItemAdded;
            Children.IndexChanged += Children_IndexChanged;
        }
        public SelectableList<IParameterContainerNode, IParameterContainerNode> Children { get; protected set; }

        public override void Calculate()
        {
            Children.ItemAtSelectedIndex.Calculate();
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            base.BuildLists(settings, parameter);
            settings.Add(Children);

            foreach(FunctionTreeNode node in Children)
            {
                node.BuildLists(settings, parameter);
            }
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            Children.CurrentState = SettingState.Inactive;
            // TODO: parallelize
            foreach (FunctionTreeNode node in Children)
            {
                node.DeactivateStates();
            }
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
            Children.CurrentState = SettingState.Active;
            // TODO: parallelize
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

        protected override void OnStateChanged()
        {
            base.OnStateChanged();
            // TODO: parallelize
            for (int i = 0; i < Children.Count; i++)
            {
                if (i == Children.SelectedIndex)
                {
                    Children[i].ActivateStates();
                }
                else
                {
                    Children[i].DeactivateStates();
                }
            }
        }

        protected override void OnParentChanged(ParentChangedEventArgs e)
        {
            base.OnParentChanged(e);
            Parent?.SettingAdded(Children);
        }


        public override bool IsOutput(string parameterVarName)
        {
            if (Children.ItemAtSelectedIndex == null)
                return false;
            return Children.ItemAtSelectedIndex.IsOutput(parameterVarName);
        }
    }
}
