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


        public ButtonComponent(SimpleCommand.ExecuteDelegate execute, SimpleCommand.CanExecuteDelegate canExecute)
        {
            Command = new SimpleCommand(
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



        private SimpleCommand _Command;
        public SimpleCommand Command
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
    }
}
