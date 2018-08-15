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

        public override double GetBaseUnitValue(string paraName)
        {
            if (this.Parameters.TryGetValue(paraName, out Parameter para))
            {
                return para.BaseUnitValue;
            }
            return base.GetBaseUnitValue(paraName);
        }

        public override void SetBaseUnitValue(string paraName, double num)
        {
            if (this.Parameters.TryGetValue(paraName, out Parameter para))
            {
                para.BaseUnitValue = num;
                return;
            }
            base.SetBaseUnitValue(paraName, num);
        }

        private void Parameters_ItemAdded(object sender, ItemEventArgs<Parameter> e)
        {
            Parameter para = e.ModifiedItem;
            switch (CurrentState)
            {
                case FunctionTreeNodeState.Active:
                    if (IsOutput(para.Name))
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
            foreach (Parameter para in Parameters)
            {
                para.CurrentState = ParameterState.Inactive;
            }
        }

        public override void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
            foreach (Parameter para in Parameters)
            {
                if (IsOutput(para.Name))
                {
                    para.CurrentState = ParameterState.Output;
                }
                else
                {
                    para.CurrentState = ParameterState.Input;
                }
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
