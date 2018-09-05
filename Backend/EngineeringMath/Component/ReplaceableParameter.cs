using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EngineeringMath.Component
{
    public class ReplaceableParameter : NotifyPropertyChangedExtension, IParameter, IParameterContainerNode
    {
        public ReplaceableParameter(IParameter parameter, IEnumerable<IParameterContainerLeaf> leaves)
        {
            ReplacingParameter = parameter;
            InputBranch = new FunctionBranch(string.Format(LibraryResources.ReplaceParameterName, parameter.DisplayName))
            {
                Parent = this
            };
            InputBranch.Children.TopValue = new FunctionOutputMakerNode(LibraryResources.DontReplaceParameter, parameter.DisplayName); ;
            InputBranch.Children.IndexChanged += Children_IndexChanged;

            OutputBranch = new FunctionBranch(string.Format(LibraryResources.ReplaceParameterName, parameter.DisplayName))
            {
                Parent = this
            };
            OutputBranch.Children.TopValue = new FunctionOutputMakerNode(LibraryResources.DontReplaceParameter);
            OutputBranch.Children.IndexChanged += Children_IndexChanged;
            foreach (IParameterContainerLeaf leaf in leaves)
                AddReplacementNode(leaf);

            RefreshState();
        }

        private void Children_IndexChanged(object sender, EventArgs e)
        {
            // since the index has changed, the current state will be updated within the set statement
            CurrentState = CurrentState;
        }

        /// <summary>
        /// Adds a node which either uses ReplacingParameter as an input or an output.
        /// The node must contain all references to other parameters within itself. This node will not be able to see parameters in other nodes.
        /// To reference the ReplacingParameter in the EquationExpression or OutputParameterName simply use "#" (without quotes)
        /// </summary>
        /// <param name="node"></param>
        public void AddReplacementNode(IParameterContainerLeaf node)
        {
            string replacingStr = "#";

            if ((node.EquationExpression.Contains(replacingStr) && node.OutputParameterVarName.Equals(replacingStr)) ||
                (!node.EquationExpression.Contains(replacingStr) && !node.OutputParameterVarName.Equals(replacingStr))
                )
            {
                throw new ArgumentException("The expression must be an input XOR output");
            }

            if (node.EquationExpression.Contains(replacingStr))
            {
                node.EquationExpression = node.EquationExpression.Replace(replacingStr, ReplacingParameter.VarName);
                OutputBranch.Children.Add(node);
            }
            else
            {
                node.OutputParameterVarName = node.OutputParameterVarName.Replace(replacingStr, ReplacingParameter.VarName);
                InputBranch.Children.Add(node);
            }
        }

        /// <summary>
        /// Returns a FunctionNullNode branch if ReplacingParameter is not an input or output
        /// </summary>

        private IParameter _ReplacingParameter;
        protected IParameter ReplacingParameter
        {
            get
            {
                return _ReplacingParameter;
            }
            set
            {
                if ((value != null && value.Equals(_ReplacingParameter)) || (value == null && _ReplacingParameter == null))
                    return;
                if(_ReplacingParameter != null)
                {
                    _ReplacingParameter.StateChanged -= ReplacingParameter_StateChanged;
                    _ReplacingParameter.PropertyChanged -= ReplacingParameter_PropertyChanged;
                }
                    
                if (value != null)
                {
                    value.StateChanged += ReplacingParameter_StateChanged;
                    value.PropertyChanged += ReplacingParameter_PropertyChanged;
                }
                    

                _ReplacingParameter = value;
            }
        }

        private void ReplacingParameter_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        private void ReplacingParameter_StateChanged(object sender, EventArgs e)
        {
            RefreshState();
        }

        private void RefreshState()
        {
            switch (ReplacingParameter.CurrentState)
            {
                case ParameterState.Input:
                case ParameterState.ReplacedInput:
                    InputBranch.ActivateStates();
                    OutputBranch.DeactivateStates();
                    break;
                case ParameterState.Output:
                case ParameterState.ReplacedOutput:
                    InputBranch.DeactivateStates();
                    OutputBranch.ActivateStates();
                    break;
                case ParameterState.Inactive:
                default:
                    InputBranch.DeactivateStates();
                    OutputBranch.DeactivateStates();
                    break;
            }
        }

        public FunctionBranch InputBranch { get; }

        public FunctionBranch OutputBranch { get; }

        public event EventHandler<EventArgs> StateChanged
        {
            add
            {
                ReplacingParameter.StateChanged += value;
            }

            remove
            {
                ReplacingParameter.StateChanged -= value;
            }
        }

        public double BaseValue { get => ReplacingParameter.BaseValue; set => ReplacingParameter.BaseValue = value; }
        public double BindValue { get => ReplacingParameter.BindValue; set => ReplacingParameter.BindValue = value; }

        public string DisplayName => ReplacingParameter.DisplayName;

        public string DisplayDetail => ReplacingParameter.DisplayDetail;

        public string VarName => ReplacingParameter.VarName;

        public string Placeholder => ReplacingParameter.Placeholder;

        public double MinBaseValue => ReplacingParameter.MinBaseValue;

        public double MaxBaseValue => ReplacingParameter.MaxBaseValue;

        public double MinBindValue => ReplacingParameter.MinBindValue;

        public double MaxBindValue => ReplacingParameter.MaxBindValue;

        public ParameterState CurrentState {
            get => ReplacingParameter.CurrentState;
            set
            {
                switch (value)
                {
                    case ParameterState.Input:
                    case ParameterState.ReplacedInput:
                        ReplacingParameter.CurrentState = InputBranch.Children.SelectedIndex == 0 ? ParameterState.Input : ParameterState.ReplacedInput;
                        break;
                    case ParameterState.Output:
                    case ParameterState.ReplacedOutput:
                        ReplacingParameter.CurrentState = OutputBranch.Children.SelectedIndex == 0 ? ParameterState.Output : ParameterState.ReplacedOutput;
                        break;
                    case ParameterState.Inactive:
                    default:
                        ReplacingParameter.CurrentState = ParameterState.Inactive;
                        break;
                }
            }
        }
        public IParameterContainerNode Parent
        {
            get
            {
                return ReplacingParameter.Parent;
            }
            internal set
            {
                IParameterContainerNode parent = ReplacingParameter.Parent;
                ReplacingParameter.Parent = value;
                IChildItemDefaults.DefaultSetParent(ref parent, OnParentChanged, value, Parent_ParentChanged);
                
            }
        }
        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent { get => Parent; set => Parent = value; }

        private void Parent_ParentChanged(object sender, EventArgs e)
        {
            OnParentChanged();
        }

        protected void OnParentChanged()
        {
            if (Parent != null)
            {
                // leave in case we need to do something
            }
            ParentChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<EventArgs> ParentChanged;

        string IChildItem<IParameterContainerNode>.Key => ReplacingParameter.Key;

        string ISettingOption.Name => ReplacingParameter.DisplayName;

        SelectableList<Unit, Category<Unit>> IParameter.ParameterUnits => ReplacingParameter.ParameterUnits;

        IParameter IParameterContainerNode.FindParameter(string paraVarName)
        {
            if (ReplacingParameter.VarName.Equals(paraVarName))
                return ReplacingParameter;
            throw new Function.ParameterNotFoundException(paraVarName);
        }

        void IParameterContainerNode.Calculate()
        {
            if(InputBranch.CurrentState == FunctionTreeNodeState.Active)
            {
                InputBranch.Calculate();
            }
            else if (OutputBranch.CurrentState == FunctionTreeNodeState.Active)
            {
                OutputBranch.Calculate();
            }
            else
            {
                throw new Exception();
            }
        }

        void IParameterContainerNode.Reset()
        {
            if (Parent == null)
                throw new ArgumentNullException(nameof(Parent));
            RefreshState();
            Parent.Reset();
        }

        void IParameterContainerNode.SettingAdded(ISetting setting)
        {
            if (Parent == null)
                return;
            Parent.SettingAdded(setting);
        }

        void IParameterContainerNode.SettingRemoved(ISetting setting)
        {
            if (Parent == null)
                return;
            Parent.SettingRemoved(setting);
        }

        void IParameterContainerNode.SettingRemoved(IList<ISetting> settings)
        {
            if (Parent == null)
                return;
            Parent.SettingRemoved(settings);
        }

        void IParameterContainerNode.ParameterAdded(IParameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterAdded(parameter);
        }

        void IParameterContainerNode.ParameterRemoved(IParameter parameter)
        {
            if (Parent == null)
                return;
            Parent.ParameterRemoved(parameter);
        }

        void IParameterContainerNode.ParameterRemoved(IList<IParameter> parameters)
        {
            if (Parent == null)
                return;
            Parent.ParameterRemoved(parameters);
        }

        void IParameterContainerNode.ActivateStates()
        {
            throw new NotImplementedException();
        }

        void IParameterContainerNode.DeactivateStates()
        {
            throw new NotImplementedException();
        }

        bool IParameterContainerNode.IsOutput(string parameterVarName)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return ReplacingParameter.VarName;
        }
    }
}
