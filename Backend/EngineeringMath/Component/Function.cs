using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows.Input;

namespace EngineeringMath.Component
{
    /// <summary>
    /// Performs an engineering calculation
    /// </summary>
    public class Function : NotifyPropertyChangedExtension, IParameterContainerNode
    {
        protected Function()
        {
            NextNode = new FunctionBranch(this, Name);
            NextNode.PropertyChanged += NextNode_PropertyChanged;
            Solve = new Command(
                SolveFunction,
                CanSolve
                );
        }

        private void NextNode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e == null)
                return;
            if (e.PropertyName.Equals(nameof(FunctionBranch.Children)))
            {
                OnPropertyChanged(nameof(Children));
            }
        }

        public Function(string name, string category) : this()
        {
            Name = name;
            Category = category;
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

        public double GetBaseUnitValue(string paraName)
        {
            throw new ParameterNotFoundException(paraName);
        }

        public void SetBaseUnitValue(string paraName, double num)
        {
            throw new ParameterNotFoundException(paraName);
        }
        #endregion

        #region Properties

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


        private string _Category;
        /// <summary>
        /// Category
        /// </summary>
        public string Category
        {
            get
            {
                return _Category;
            }
            set
            {
                _Category = value;
                OnPropertyChanged();
            }
        }

        private Command _Solve;
        /// <summary>
        /// Solves this function 
        /// </summary>
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

        FunctionBranch _NextNode;

        internal FunctionBranch NextNode
        {
            get { return _NextNode; }
            set
            {
                _NextNode = value;
                OnPropertyChanged();
            }
        }

        public NotifyPropertySortedList<FunctionTreeNode, IParameterContainerNode> Children
        {
            get
            {
                return NextNode.Children;
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
    }
}
