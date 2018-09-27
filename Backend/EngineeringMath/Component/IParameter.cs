using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameter : INotifyPropertyChanged, IChildItem<IParameterContainerNode>
    {

        string DisplayName { get; }

        string DisplayDetail { get; }

        string VarName { get; }

        ParameterState CurrentState { get; set; }
        event EventHandler<EventArgs> StateChanged;

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
