using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameterContainerLeaf : IParameterContainerNode
    {
        string EquationExpression { get; }
    }
}
