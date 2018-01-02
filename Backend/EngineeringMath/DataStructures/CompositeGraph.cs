using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.DataStructures
{
    // Mostly taken from:
    // https://msdn.microsoft.com/en-us/library/ms379574(v=vs.80).aspx
    /// <summary>
    /// Creates a graph with two different objects as node
    /// <para>T - S connections are valid T - T and S - S connections are not</para>
    /// </summary>
    class CompositeGraph<T, S>
    {
        /// <summary>
        /// Contains the T object nodes
        /// </summary>
        private CompositeNodeList<T, S> nodeSetT;
        /// <summary>
        /// Contains the S object nodes
        /// </summary>
        private CompositeNodeList<S, T> nodeSetS;

        public CompositeGraph() : this(null, null) { }
        public CompositeGraph(CompositeNodeList<T, S> nodeSetT, CompositeNodeList<S, T> nodeSetS)
        {
            if (nodeSetT == null)
                this.nodeSetT = new CompositeNodeList<T, S>();
            else
                this.nodeSetT = nodeSetT;

            if (nodeSetS == null)
                this.nodeSetS = new CompositeNodeList<S, T>();
            else
                this.nodeSetS = nodeSetS;
        }

        public void AddNode(CompositeGraphNode<T, S> node)
        {
            // adds a node to the graph
            nodeSetT.Add(node);
        }

        public void AddNode(CompositeGraphNode<S, T> node)
        {
            // adds a node to the graph
            nodeSetS.Add(node);
        }

        public void AddNode(T value)
        {
            // adds a node to the graph
            AddNode(new CompositeGraphNode<T, S>(value));
        }

        public void AddNode(S value)
        {
            // adds a node to the graph
            AddNode(new CompositeGraphNode<S, T>(value));
        }

        public void AddDirectedEdge(CompositeGraphNode<T, S> from, CompositeGraphNode<S, T> to, int cost = int.MaxValue)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddDirectedEdge(CompositeGraphNode<S, T> from, CompositeGraphNode<T, S> to, int cost = int.MaxValue)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddUndirectedEdge(CompositeGraphNode<S, T> from, CompositeGraphNode<T, S> to, int cost)
        {
            AddUndirectedEdge(to, from, cost);
        }

        public void AddUndirectedEdge(CompositeGraphNode<T, S> from, CompositeGraphNode<S, T> to, int cost)
        {
            AddDirectedEdge(to, from, cost);
            AddDirectedEdge(from, to, cost);
        }

        public bool Contains(T value)
        {
            return nodeSetT.FindByValue(value) != null;
        }

        public bool Contains(S value)
        {
            return nodeSetS.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            // first remove the node from the nodeset
            CompositeGraphNode<T, S> nodeToRemove = (CompositeGraphNode<T, S>)nodeSetT.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSetT.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (CompositeGraphNode<S, T> gnode in nodeSetS)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }


        public bool Remove(S value)
        {
            // first remove the node from the nodeset
            CompositeGraphNode<S, T> nodeToRemove = (CompositeGraphNode<S, T>)nodeSetS.FindByValue(value);
            if (nodeToRemove == null)
                // node wasn't found
                return false;

            // otherwise, the node was found
            nodeSetS.Remove(nodeToRemove);

            // enumerate through each node in the nodeSet, removing edges to this node
            foreach (CompositeGraphNode<T, S> gnode in nodeSetT)
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    // remove the reference to the node and associated cost
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }

            return true;
        }


        public CompositeNodeList<T,S> TNodes
        {
            get
            {
                return nodeSetT;
            }
        }

        public CompositeNodeList<S, T> SNodes
        {
            get
            {
                return nodeSetS;
            }
        }

        public int Count
        {
            get { return nodeSetT.Count + nodeSetS.Count; }
        }
    }
}
