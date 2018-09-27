using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public class VisitableNodeDirector
    {
        public PVTTableNodeBuilder NodeBuilder { get; set; }

        public FunctionVisitableNode Node { get { return NodeBuilder?.Node; } }

        public void BuildNode(string name)
        {
            if (NodeBuilder == null)
                return;
            if(Node != null)
                Node.VisitorOptions.IndexChanged -= Node.VisitorOptions_IndexChanged;
            NodeBuilder.BuildNode(name);
            NodeBuilder.BuildParameters();
            NodeBuilder.BuildVisitorOptions();
            Node.VisitorOptions.IndexChanged += Node.VisitorOptions_IndexChanged;
        }
    }
}
