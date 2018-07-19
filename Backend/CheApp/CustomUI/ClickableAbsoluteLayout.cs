using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace CheApp.CustomUI
{
    public class ClickableAbsoluteLayout : AbsoluteLayout
    {
        public ClickableAbsoluteLayout()
        {
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    Clicked?.Invoke(this, EventArgs.Empty);

                    if(this.Command != null)
                    {
                        if (Command.CanExecute(CommandParameter))
                        {
                            Command.Execute(CommandParameter);
                        }
                    }
                })
            });
        }
        public event EventHandler Clicked;

        public static readonly BindableProperty CommandProperty = 
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ClickableAbsoluteLayout), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ClickableAbsoluteLayout), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

    }
}
