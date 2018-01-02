using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.DataStructures
{
    // Taken from:
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
    public class CompositeGraphNode<T, S> : CompositeNode<T, S>
    {
        private List<int> costs;

        public CompositeGraphNode() : base() { }
        public CompositeGraphNode(T value) : base(value) { }
        public CompositeGraphNode(T value, CompositeNodeList<S, T> neighbors) : base(value, neighbors) { }

        new public CompositeNodeList<S, T> Neighbors
        {
            get
            {
                if (base.Neighbors == null)
                    base.Neighbors = new CompositeNodeList<S, T>();

                return base.Neighbors;
            }
        }

        public List<int> Costs
        {
            get
            {
                if (costs == null)
                    costs = new List<int>();

                return costs;
            }
        }
    }
}
