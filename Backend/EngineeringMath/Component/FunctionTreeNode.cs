using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNode : NotifyPropertyChangedExtension, IParameterContainerNode
    {
        protected FunctionTreeNode()
        {

        }


        private string _Name;

        public virtual string Name
        {
            get { return _Name; }
            protected set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        protected IParameterContainerNode _Parent;



        public IParameterContainerNode Parent
        {
            get
            {
                return _Parent;
            }
            internal set
            {
                IChildItemDefaults.DefaultSetParent(ref _Parent, OnParentChanged, value, Parent_ParentChanged);                
            }
        }

        private void Parent_StateChanged(object sender, EventArgs e)
        {
            OnStateChanged();
        }

        protected virtual void OnParentChanged()
        {
            ParentChanged?.Invoke(this, EventArgs.Empty);
        }
        private void Parent_ParentChanged(object sender, EventArgs e)
        {
            OnParentChanged();
        }
        public event EventHandler<EventArgs> ParentChanged;






        public virtual IParameter FindParameter(string paraVarName)
        {
            return Parent.FindParameter(paraVarName);
        }

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
        public virtual void DeactivateStates()
        {
            CurrentState = FunctionTreeNodeState.Inactive;
        }


        /// <summary>
        /// Makes all states along the ACTIVE branch the correct state i.e. making a parameter input or output
        /// </summary>
        public virtual void ActivateStates()
        {
            CurrentState = FunctionTreeNodeState.Active;
        }

        public abstract void BuildLists(List<ISetting> settings, List<IParameter> parameter);

        private FunctionTreeNodeState _CurrentState = FunctionTreeNodeState.Inactive;
        public FunctionTreeNodeState CurrentState
        {
            get
            {
                return _CurrentState;
            }
            internal set
            {
                _CurrentState = value;
                OnStateChanged();
            }
        }

        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent { get => Parent; set => Parent = value; }

        string IChildItem<IParameterContainerNode>.Key => Name;

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

        public void ParameterAdded(IParameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterAdded(parameter);
        }
        public void ParameterRemoved(IParameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterRemoved(parameter);
        }
        public void ParameterRemoved(IList<IParameter> parameters)
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





        protected virtual void OnStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> StateChanged;

    }
    public enum FunctionTreeNodeState
    {
        Active,
        Inactive
    }
}
