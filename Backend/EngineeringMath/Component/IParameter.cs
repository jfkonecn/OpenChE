using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameter : IChildItem<IParameterContainerNode>
    {
        string Name { get; }
        double BaseUnitValue { get; set; }

        double MinBaseValue { get; }

        double MaxBaseValue { get; }
    }
}
