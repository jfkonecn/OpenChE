using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Component.CustomEventArgs;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Calculate function calls 
    /// </summary>
    public class FunctionVisitableNode : FunctionTreeNodeWithParameters
    {
        protected FunctionVisitableNode() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        protected FunctionVisitableNode(string name) : this()
        {
            Name = name;
        }

        public FunctionVisitableNode(string name, FunctionVisitor visitor) : this(name)
        {
            Visitor = visitor ?? throw new ArgumentNullException(nameof(visitor));
            Visitor.Parent = this;
        }

        public FunctionVisitor Visitor { get; protected set; }

        public FunctionTreeNode NextNode { get; set; }

        public override void Calculate()
        {
            Visitor?.Calculate();
            NextNode?.Calculate();
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            base.BuildLists(settings, parameter);
            NextNode?.BuildLists(settings, parameter);
        }

        public override ParameterState DetermineState(string paraVarName)
        {
            ParameterState state = Visitor == null ? ParameterState.Inactive : Visitor.DetermineState(paraVarName);
            if(state == ParameterState.Inactive && NextNode != null)
                state = NextNode.DetermineState(paraVarName);
            return state;
        }

        public override void ActivateStates()
        {
            base.ActivateStates();
            Visitor?.ActivateStates();
            NextNode?.ActivateStates();
        }

        public override void DeactivateStates()
        {
            base.DeactivateStates();
            Visitor?.DeactivateStates();
            NextNode?.DeactivateStates();
        }

    }
}
