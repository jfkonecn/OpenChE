using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EngineeringMath.DataStructures
{
    // Taken from:
    // https://msdn.microsoft.com/en-us/library/ms379572(v=vs.80).aspx
    public class CompositeNodeList<T, S> : Collection<CompositeNode<T, S>>
    {
        public CompositeNodeList() : base() { }

        public CompositeNodeList(int initialSize)
        {
            // Add the specified number of items
            for (int i = 0; i < initialSize; i++)
                base.Items.Add(default(CompositeNode<T, S>));
        }

        public CompositeNode<T, S> FindByValue(T value)
        {
            // search the list for the value
            foreach (CompositeNode<T, S> node in Items)
                if (node.Value.Equals(value))
                    return node;

            // if we reached here, we didn't find a matching node
            return null;
        }
    }
}
