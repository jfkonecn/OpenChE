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
    public abstract class Function : NotifyPropertyChangedExtension
    {
        protected Function()
        {
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
            Calculate();
            OnSolved();
        }

        /// <summary>
        /// Function called by the solve command
        /// </summary>
        protected virtual bool CanSolve(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Performs the actual calculation for this function object
        /// </summary>
        protected abstract void Calculate();


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
        #endregion

        #region Properties

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            internal set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _CatName;
        /// <summary>
        /// Category Name
        /// </summary>
        public string CatName
        {
            get
            {
                return _CatName;
            }
            internal set
            {
                _CatName = value;
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
