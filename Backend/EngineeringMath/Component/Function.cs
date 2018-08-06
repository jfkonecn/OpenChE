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
            NextNode = new FunctionBranch(this);
            Command Solve = new Command(
                SolveFunction,
                CanSolve
                );
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
            throw new NotImplementedException();
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
            internal set
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

        public FunctionBranch NextNode
        {
            get { return _NextNode; }
            set
            {
                _NextNode = value;
                OnPropertyChanged();
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
    }
}
