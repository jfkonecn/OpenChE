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
        public FunctionVisitableNode(string name) : this()
        {
            Name = name;
        }


        internal void VisitorOptions_IndexChanged(object sender, EventArgs e)
        {
            OnStateChanged();
        }

        public SelectableList<FunctionVisitor, IParameterContainerNode> VisitorOptions { get; internal set; }

        public FunctionTreeNode NextNode { get; set; }

        public override void Calculate()
        {
            VisitorOptions?.ItemAtSelectedIndex?.Calculate();
            NextNode?.Calculate();
        }

        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            base.BuildLists(settings, parameter);
            NextNode?.BuildLists(settings, parameter);
        }

        public override bool IsOutput(string parameterName)
        {
            FunctionVisitor visitor = VisitorOptions.ItemAtSelectedIndex;
            bool isOutput = visitor == null ? false : visitor.IsOutput(parameterName);
            isOutput = isOutput || (NextNode == null ? false : NextNode.IsOutput(parameterName));
            return isOutput;
        }

        protected override void OnParentChanged(ParentChangedEventArgs e)
        {
            base.OnParentChanged(e);
            Parent?.SettingAdded(VisitorOptions);
        }

        protected override void OnStateChanged()
        {
            base.OnStateChanged();            
            for (int i = 0; i < VisitorOptions.Count; i++)
            {
                VisitorOptions[i].DeactivateStates();
            }
            VisitorOptions.ItemAtSelectedIndex.ActivateStates();
        }
    }
}
