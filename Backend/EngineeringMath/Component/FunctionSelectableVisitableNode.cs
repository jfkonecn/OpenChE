using EngineeringMath.Component.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class FunctionSelectableVisitableNode : FunctionVisitableNodeLeaf
    {
        protected FunctionSelectableVisitableNode() : base()
        {

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public FunctionSelectableVisitableNode(string name) : base(name)
        {
            
        }




        internal void VisitorOptions_IndexChanged(object sender, EventArgs e)
        {
            Visitor = VisitorOptions?.ItemAtSelectedIndex;
            OnStateChanged();            
        }

        private SelectableList<FunctionVisitor, IParameterContainerNode> _VisitorOptions;
        public SelectableList<FunctionVisitor, IParameterContainerNode> VisitorOptions
        {
            get
            {
                return _VisitorOptions;
            }
            internal set
            {
                if ((value != null && value.Equals(_VisitorOptions)) ||
                    (_VisitorOptions != null && _VisitorOptions.Equals(value)) ||
                    (value == null && _VisitorOptions == null))
                    return;
                if (_VisitorOptions != null)
                    _VisitorOptions.IndexChanged -= VisitorOptions_IndexChanged;
                _VisitorOptions = value;
                if (_VisitorOptions != null)
                {
                    _VisitorOptions.IndexChanged += VisitorOptions_IndexChanged;
                    VisitorOptions.SelectedIndex = 0;
                }
                    
            }
        }


        public override void BuildLists(List<ISetting> settings, List<IParameter> parameter)
        {
            base.BuildLists(settings, parameter);
            settings.Add(VisitorOptions);
        }

        protected override void OnParentChanged(ParentChangedEventArgs e)
        {
            base.OnParentChanged(e);
            Parent?.SettingAdded(VisitorOptions);
        }

        protected override void OnStateChanged()
        {
            for (int i = 0; i < VisitorOptions.Count; i++)
            {
                VisitorOptions[i].DeactivateStates();
            }
            base.OnStateChanged();
        }
    }
}
