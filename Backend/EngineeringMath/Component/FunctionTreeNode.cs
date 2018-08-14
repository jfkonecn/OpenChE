using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNode : ChildItem<IParameterContainerNode>, IParameterContainerNode
    {
        protected FunctionTreeNode()
        {

        }



        protected FunctionTreeNode(string name) : this()
        {
            Name = name;
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        protected IParameterContainerNode ParentObject { get; set; }



        public override IParameterContainerNode Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                if (value.Equals(ParentObject))
                    return;


                if (ParentObject is FunctionTreeNode node)
                {
                    node.ParentChanged -= Parent_ParentChanged;
                }

                node = value as FunctionTreeNode;
                if (node != null)
                {
                    node.ParentChanged += Parent_ParentChanged;
                }

                this.ParentObject = value;
                OnParentChanged();
            }
        }


        private void Parent_ParentChanged(object sender, EventArgs e)
        {
            OnParentChanged();
        }




        public abstract double GetBaseUnitValue(string paraName);

        public abstract void SetBaseUnitValue(string paraName, double num);

        public abstract void Calculate();

        /// <summary>
        /// Notifies the parent that settings and parameters must be updates
        /// </summary>
        public void Reset()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent));
            Parent.Reset();
        }

        /// <summary>
        /// Makes all states along branch stateless
        /// </summary>
        public abstract void DeactivateStates();


        /// <summary>
        /// Makes all states along the ACTIVE branch the correct state i.e. making a parameter input or output
        /// </summary>
        public abstract void ActivateStates();

        public abstract void BuildLists(List<ISetting> settings, List<Parameter> parameter);

        public FunctionTreeNodeState CurrentState { get; internal set; } = FunctionTreeNodeState.Inactive;
        public override string ToString()
        {
            return Name;
        }

        public void SettingAdded(ISetting setting)
        {
            if (Parent == null)
                return;
            Parent.SettingAdded(setting);
        }
        public void SettingRemoved(ISetting setting)
        {
            if (Parent == null)
                return;
            Parent.SettingRemoved(setting);
        }
        public void SettingRemoved(IList<ISetting> settings)
        {
            if (Parent == null)
                return;
            Parent.SettingRemoved(settings);
        }

        public void ParameterAdded(Parameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterAdded(parameter);
        }
        public void ParameterRemoved(Parameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterRemoved(parameter);
        }
        public void ParameterRemoved(IList<Parameter> parameters)
        {
            if (Parent == null)
                return;
            Parent.ParameterRemoved(parameters);
        }

        /// <summary>
        /// Searches each active leaf node to determine if the parameter name is being used as an output
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public abstract bool IsOutput(string parameterName);


        protected virtual void OnParentChanged()
        {
            ParentChanged?.Invoke(this, EventArgs.Empty);          
        }

        public EventHandler<EventArgs> ParentChanged;

    }
    public enum FunctionTreeNodeState
    {
        Active,
        Inactive
    }
}
