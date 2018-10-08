using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EngineeringMath.Component
{
    public class FunctionQueueNode : FunctionTreeNodeWithParameters
    {
        public FunctionQueueNode() : base()
        {
            Children = new QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode>(this);
            Name = string.Empty;
        }
        private QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode> _Children;
        public QueuingSortedList<PriorityFunctionBranch, IParameterContainerNode> Children
        {
            get
            {
                return _Children;
            }
            protected set
            {
                if ((value == null && _Children == null)
                    || (value != null && value.Equals(_Children)))
                    return;
                if(_Children != null)
                {
                    _Children.ItemAdded -= Children_ItemAdded;
                    _Children.ItemRemoved -= Children_ItemRemoved;
                    _Children.ItemsCleared -= Children_ItemsCleared;
                }
                    
                _Children = value;
                if (_Children != null)
                {
                    _Children.ItemAdded += Children_ItemAdded;
                    _Children.ItemRemoved += Children_ItemRemoved;
                    _Children.ItemsCleared += Children_ItemsCleared;
                }                   

            }
        }

        private void Children_ItemsCleared(object sender, ItemEventArgs<IList<PriorityFunctionBranch>> e)
        {
            if (CurrentState == FunctionTreeNodeState.Active)
            {
                ActivateStates();
            }
            else
            {
                DeactivateStates();
            }
        }

        private void Children_ItemRemoved(object sender, ItemEventArgs<PriorityFunctionBranch> e)
        {
            if(CurrentState == FunctionTreeNodeState.Active)
            {
                ActivateStates();
            }
            else
            {
                DeactivateStates();
            }
        }

        private void Children_ItemAdded(object sender, ItemEventArgs<PriorityFunctionBranch> e)
        {
            if (CurrentState == FunctionTreeNodeState.Active)
            {
                ActivateStates();
            }
            else
            {
                DeactivateStates();
            }
        }

        public override void Calculate()
        {
            // TODO: parallelize if the priority is the same
            foreach (PriorityFunctionBranch branch in Children.GetQueue())
            {
                branch.Calculate();
            }
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            foreach (IParameter para in this.Parameters)
            {
                parameter.Add(para);
            }
            // TODO: parallelize
            foreach (FunctionTreeNode node in Children)
            {
                node.BuildLists(settings, parameter);
            }
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            // TODO: parallelize
            foreach (FunctionTreeNode node in Children)
            {
                node.DeactivateStates();
            }
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
            // TODO: parallelize
            foreach (FunctionTreeNode node in Children)
            {
                node.ActivateStates();
            }
        }

        public override ParameterState DetermineState(string parameterName)
        {
            foreach (FunctionTreeNode node in Children.GetQueue())
            {
                // TODO: parallelize 
                ParameterState state = node.DetermineState(parameterName);
                if (state != ParameterState.Inactive)
                {
                    return state;
                }
            }
            return ParameterState.Inactive;
        }
    }
}
