using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameter : INotifyPropertyChanged, IChildItem<IParameterContainerNode>
    {
        double BaseUnitValue { get; set; }

        double BindValue { get; set; }

        string DisplayName { get; }

        string DisplayDetail { get; }

        string VarName { get; }

        double MinBaseValue { get; }
        double MaxBaseValue { get; }
        ParameterState CurrentState { get; set; }
    }
    public enum ParameterState
    {
        Input,
        Output,
        Inactive
    }
}
