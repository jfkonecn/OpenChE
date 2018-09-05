using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameter : INotifyPropertyChanged, IChildItem<IParameterContainerNode>
    {
        double BaseValue { get; set; }

        double BindValue { get; set; }

        string DisplayName { get; }

        string DisplayDetail { get; }

        string VarName { get; }
        string Placeholder { get; }
        double MinBaseValue { get; }
        double MaxBaseValue { get; }
        double MinBindValue { get; }
        double MaxBindValue { get; }
        ParameterState CurrentState { get; set; }
        event EventHandler<EventArgs> StateChanged;
        /// <summary>
        /// Can be null
        /// </summary>
        SelectableList<Unit, Category<Unit>> ParameterUnits { get; }
    }
    public enum ParameterState
    {
        Input,
        Output,
        Inactive,
        ReplacedInput,
        ReplacedOutput
    }
}
