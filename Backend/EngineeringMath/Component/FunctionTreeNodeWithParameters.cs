using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNodeWithParameters : FunctionTreeNode
    {

        protected FunctionTreeNodeWithParameters() : base()
        {
            Parameters = new NotifyPropertySortedList<Parameter, IParameterContainerNode>(this);
            Parameters.ItemAdded += Parameters_ItemAdded;
            Parameters.ItemRemoved += Parameters_ItemRemoved;
            Parameters.ItemsCleared += Parameters_ItemsCleared;
        }


        private void Parameters_ItemsCleared(object sender, ItemEventArgs<IList<Parameter>> e)
        {
            ParameterRemoved(e.ModifiedItem);
        }

        private void Parameters_ItemRemoved(object sender, ItemEventArgs<Parameter> e)
        {
            ParameterRemoved(e.ModifiedItem);
        }

        public override Parameter FindParameter(string paraVarName)
        {
            if (this.Parameters.TryGetValue(paraVarName, out Parameter para))
            {
                return para;
            }
            return base.FindParameter(paraVarName);
        }

        private void Parameters_ItemAdded(object sender, ItemEventArgs<Parameter> e)
        {
            Parameter para = e.ModifiedItem;
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
            if (CurrentState == FunctionTreeNodeState.Active)
            {
                foreach (Parameter para in Parameters)
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
                foreach (Parameter para in Parameters)
                {
                    para.CurrentState = ParameterState.Inactive;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
            if (Parent != null)
            {
                foreach (Parameter para in Parameters)
                {
                    Parent.ParameterAdded(para);
                }
            }
        }

        private NotifyPropertySortedList<Parameter, IParameterContainerNode> _Parameters;
        public NotifyPropertySortedList<Parameter, IParameterContainerNode> Parameters
        {
            get { return _Parameters; }
            set
            {
                _Parameters = value;
                OnPropertyChanged();
            }
        }
    }
}
