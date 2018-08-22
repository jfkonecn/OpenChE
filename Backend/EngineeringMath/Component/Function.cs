using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows.Input;
using System.Xml.Serialization;
using System.Linq;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Performs an engineering calculation
    /// </summary>
    public class Function : ChildItem<Category<Function>>, IParameterContainerNode, ICategoryItem
    {
        protected Function()
        {
            AllParameters = new List<Parameter>();
            AllSettings = new List<ISetting>();

            Solve = new Command(
                SolveFunction,
                CanSolve
                );
        }

        private void NextNode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e == null)
                return;
        }

        public Function(string fullName) : this()
        {
            FullName = fullName;
        }


        #region Methods

        

        /// <summary>
        /// Function called by the solve command
        /// </summary>
        protected virtual void SolveFunction(object parameter)
        {
            OnSolving();
            this.Calculate();
            OnSolved();
        }

        /// <summary>
        /// Function called by the solve command
        /// </summary>
        protected virtual bool CanSolve(object parameter)
        {
            return true;
        }


        protected void OnSolved()
        {
            Solved?.Invoke(this, EventArgs.Empty);
        }

        protected void OnSolving()
        {
            Solving?.Invoke(this, EventArgs.Empty);
        }

        protected void OnError(Exception e)
        {
            HadError?.Invoke(this, e);
        }

        protected void OnReset()
        {
            WasReset?.Invoke(this, EventArgs.Empty);
        }

        public void Calculate()
        {
            NextNode.Calculate();
        }

        public Parameter FindParameter(string paraName)
        {
            throw new ParameterNotFoundException(paraName);
        }

        public void Reset()
        {
            AllSettings = new List<ISetting>();
            AllParameters = new List<Parameter>();
            NextNode.BuildLists(AllSettings, AllParameters);
        }

        public void SettingAdded(ISetting setting)
        {
            AllSettings.Add(setting);
        }
        public void SettingRemoved(ISetting setting)
        {
            AllSettings.Remove(setting);
        }
        public void SettingRemoved(IList<ISetting> settings)
        {
            foreach(ISetting setting in settings)
            {
                AllSettings.Remove(setting);
            }
        }

        public void ParameterAdded(Parameter parameter)
        {
            AllParameters.Add(parameter);
        }
        public void ParameterRemoved(Parameter parameter)
        {
            AllParameters.Remove(parameter);
        }
        public void ParameterRemoved(IList<Parameter> parameters)
        {
            foreach (Parameter parameter in parameters)
            {
                AllParameters.Remove(parameter);
            }
        }
        #endregion

        #region Properties

        public List<Parameter> InputParameters
        {
            get
            {
                return (from para in AllParameters
                       where para.CurrentState == ParameterState.Input
                       orderby para.ToString()
                       select para)
                       .ToList();
            }
        }

        public List<Parameter> OutputParameters
        {
            get
            {
                return (from para in AllParameters
                        where para.CurrentState == ParameterState.Output
                        orderby para.ToString()
                        select para)
                       .ToList();
            }
        }


        public List<ISetting> ActiveSettings
        {
            get
            {
                return (from setting in AllSettings
                        where setting.CurrentState == SettingState.Active
                        orderby setting.ToString()
                        select setting)
                       .ToList();
            }
        }

        private string _FullName;

        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
                OnPropertyChanged();
            }
        }



        private Command _Solve;
        /// <summary>
        /// Solves this function 
        /// </summary>
        [XmlIgnore]
        public Command Solve
        {
            get
            {
                return _Solve;
            }
            private set
            {
                _Solve = value;
                OnPropertyChanged();
            }
        }

        private List<ISetting> _AllSettings;
        [XmlIgnore]
        public List<ISetting> AllSettings
        {
            get
            {
                return _AllSettings;
            }
            protected set
            {
                _AllSettings = value;
                OnPropertyChanged();
            }
        }


        private List<Parameter> _AllParameters;
        [XmlIgnore]
        internal List<Parameter> AllParameters
        {
            get
            {
                return _AllParameters;
            }
            set
            {
                _AllParameters = value;
                OnPropertyChanged();
            }
        }


        private Command _Save;
        /// <summary>
        /// Saves this functions
        /// </summary>
        public Command Save
        {
            get
            {
                return _Save;
            }
            private set
            {
                _Save = value;
                OnPropertyChanged();
            }
        }

        FunctionTreeNode _NextNode;

        public FunctionTreeNode NextNode
        {
            get { return _NextNode; }
            set
            {
                if (_NextNode != null && _NextNode.Equals(value))
                    return;
                if(_NextNode != null)
                {
                    _NextNode.PropertyChanged -= NextNode_PropertyChanged;
                    _NextNode.Parent = null;
                }                    

                _NextNode = value;
                if(_NextNode != null)
                {
                    _NextNode.CurrentState = FunctionTreeNodeState.Active;
                    _NextNode.PropertyChanged += NextNode_PropertyChanged;
                    _NextNode.Parent = this;
                }
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        private Category<Function> ParentObject { get; set; }


        public override Category<Function> Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }

        #endregion

        #region Events
        public event EventHandler Solved;

        public event EventHandler Solving;

        public event ErrorEventHandler HadError;

        public delegate void ErrorEventHandler(object sender, Exception e);

        public event EventHandler WasReset;
        #endregion

        public class ParameterNotFoundException : ArgumentException
        {
            public ParameterNotFoundException(string ParameterName) : base(string.Empty, paramName: ParameterName)
            {

            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
