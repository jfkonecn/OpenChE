using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Node which does nothing. Use when you want to add an option which does not do any calculations
    /// </summary>
    public class FunctionNullNode : FunctionTreeNode
    {
        protected FunctionNullNode() : base()
        {

        }

        public FunctionNullNode(string name) : this()
        {
            Name = name;
        }

        public override void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            return;
        }

        public override void Calculate()
        {
            return;
        }

        public override void DeactivateStates()
        {
            CurrentState = FunctionTreeNodeState.Inactive;
        }

        public override IParameter FindParameter(string paraName)
        {
            throw new NotSupportedException();
        }

        public override bool IsOutput(string parameterName)
        {
            throw new NotSupportedException();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
