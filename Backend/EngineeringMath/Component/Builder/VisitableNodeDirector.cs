using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class VisitableNodeDirector
    {
        public VisitableNodeBuilder NodeBuilder { get; set; }

        public FunctionVisitableNodeLeaf Node { get { return NodeBuilder?.Node; } }

        public void BuildNode(string name)
        {
            if (NodeBuilder == null)
                return;
            NodeBuilder.BuildNode(name);
            NodeBuilder.BuildParameters();
        }
    }
}
