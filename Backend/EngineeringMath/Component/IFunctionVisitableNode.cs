using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IFunctionVisitableNode : IParameterContainerNode
    {
        FunctionVisitor Visitor { get; set; }
    }
}
