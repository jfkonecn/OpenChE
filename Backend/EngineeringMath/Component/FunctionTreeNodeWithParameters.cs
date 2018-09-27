using EngineeringMath.Component.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNodeWithParameters : FunctionTreeNode
    {

        protected FunctionTreeNodeWithParameters() : base()
        {
            Parameters = new NotifyPropertySortedList<IParameter, IParameterContainerNode>(this);
            Parameters.ItemAdded += Parameters_ItemAdded;
            Parameters.ItemRemoved += Parameters_ItemRemoved;
            Parameters.ItemsCleared += Parameters_ItemsCleared;
        }


        private void Parameters_ItemsCleared(object sender, ItemEventArgs<IList<IParameter>> e)
        {
            ParameterRemoved(e.ModifiedItem);
        }

        private void Parameters_ItemRemoved(object sender, ItemEventArgs<IParameter> e)
        {
            ParameterRemoved(e.ModifiedItem);
        }

        public override IParameter FindParameter(string paraVarName)
        {
            if (this.Parameters.TryGetValue(paraVarName, out IParameter para))
            {
                return para;
            }
            return base.FindParameter(paraVarName);
        }

        private void Parameters_ItemAdded(object sender, ItemEventArgs<IParameter> e)
        {
            IParameter para = e.ModifiedItem;
            switch (CurrentState)
            {
                case FunctionTreeNodeState.Active:
                    if (IsOutput(para.VarName))
                    {
                        para.CurrentState = ParameterState.Output;
                    }
                    else
                    {
                        para.CurrentState = ParameterState.Input;
                    }
                    break;
                case FunctionTreeNodeState.Inactive:
                    para.CurrentState = ParameterState.Inactive;
                    break;
                default:
                    throw new NotImplementedException();
            }

            ParameterAdded(para);
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            foreach (IParameter para in this.Parameters)
            {
                parameter.Add(para);
            }
        }


        public override void DeactivateStates()
        {
            CurrentState = FunctionTreeNodeState.Inactive;

        }

        public override void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
        }


        protected override void OnStateChanged()
        {
            base.OnStateChanged();
            // TODO: parallelize
            if (CurrentState == FunctionTreeNodeState.Active)
            {
                foreach (IParameter para in Parameters)
                {
                    if (IsOutput(para.VarName))
                    {
                        para.CurrentState = ParameterState.Output;
                    }
                    else
                    {
                        para.CurrentState = ParameterState.Input;
                    }
                }
            }
            else if (CurrentState == FunctionTreeNodeState.Inactive)
            {
                foreach (IParameter para in Parameters)
                {
                    para.CurrentState = ParameterState.Inactive;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnParentChanged(ParentChangedEventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                foreach (IParameter para in Parameters)
                {
                    Parent.ParameterAdded(para);
                }
            }
        }
        public NotifyPropertySortedList<IParameter, IParameterContainerNode> Parameters { get; protected set; }
    }
}
