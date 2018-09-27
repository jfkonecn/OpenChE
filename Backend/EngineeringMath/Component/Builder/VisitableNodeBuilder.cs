using EngineeringMath.Resources.PVTTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public abstract class VisitableNodeBuilder
    {
        public FunctionVisitableNode Node { get; protected set; } = null;
        public void BuildNode(string name)
        {
            Node = new FunctionVisitableNode(name);
        }
        public abstract void BuildParameters();
        public abstract void BuildVisitorOptions();
    }
}
