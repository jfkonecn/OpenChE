using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class ItemEventArgs<T> : EventArgs 
        where T : class
    {
        public T ModifiedItem { get; protected set; }
        public ItemEventArgs(T modifiedItem)
        {
            ModifiedItem = modifiedItem;
        }
    }
}
