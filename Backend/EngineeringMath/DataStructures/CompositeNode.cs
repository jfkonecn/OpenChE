using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.DataStructures
{
    // Taken from:
    // https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
    /// <summary>
    /// Node which of object T which can only connect to objects of type S
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="S"></typeparam>
    public class CompositeNode<T, S>
    {
        // Private member-variables
        private T data;
        private CompositeNodeList<S, T> neighbors = null;

        public CompositeNode() { }
        public CompositeNode(T data) : this(data, null) { }
        public CompositeNode(T data, CompositeNodeList<S, T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        protected CompositeNodeList<S, T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }
    }
}
