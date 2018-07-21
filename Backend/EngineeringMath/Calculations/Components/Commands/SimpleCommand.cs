using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace EngineeringMath.Calculations.Components.Commands
{
    public class SimpleCommand : ICommand
    {
        internal SimpleCommand(ExecuteDelegate command, CanExecuteDelegate canRunCommand)
        {
            Command = command;
            CanRunCommand = canRunCommand;
        }

        private ExecuteDelegate Command { get; set; }
        private CanExecuteDelegate CanRunCommand { get; set; }

        public event EventHandler CanExecuteChanged;


        public bool CanExecute(object parameter)
        {
            return CanRunCommand(parameter);
        }

        private void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public void Execute(object parameter)
        {
            Command(parameter);
        }


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
