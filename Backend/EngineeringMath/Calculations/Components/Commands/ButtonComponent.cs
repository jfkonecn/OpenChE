using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EngineeringMath.Calculations.Components.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ButtonComponent : AbstractComponent
    {


        public ButtonComponent(ExecuteDelegate execute, CanExecuteDelegate canExecute)
        {
            Command = new ButtonCommand(
                (object parameter) =>
                {
                    try
                    {
                        OnReset();
                        execute(parameter);
                    }
                    catch(Exception e)
                    {
                        OnError(e);
                    }
                    OnSuccess();                    
                }
            , canExecute);
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

        private ButtonCommand _Command;
        public ButtonCommand Command
        {
            get
            {
                return _Command;
            }
            private set
            {
                _Command = value;
                OnPropertyChanged();
            }
        }

        public class ButtonCommand : ICommand
        {

            internal ButtonCommand(ExecuteDelegate command, CanExecuteDelegate canRunCommand)
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

            public void Execute(object parameter)
            {
                Command(parameter);
            }
        }
    }
}
