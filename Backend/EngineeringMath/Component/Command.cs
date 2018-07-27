using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EngineeringMath.Component
{
    public class Command : ICommand
    {
        internal Command(ExecuteDelegate myCommand, CanExecuteDelegate canRunCommand)
        {
            MyCommand = myCommand;
            CanRunCommand = canRunCommand;
        }

        public bool CanExecute(object parameter)
        {
            return CanRunCommand(parameter);
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Execute(object parameter)
        {
            MyCommand(parameter);
        }
        private ExecuteDelegate MyCommand { get; set; }
        private CanExecuteDelegate CanRunCommand { get; set; }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Executed when command event occurs
        /// </summary>
        public delegate void ExecuteDelegate(object parameter);

        /// <summary>
        /// True when command when can be run
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public delegate bool CanExecuteDelegate(object parameter);

    }
}