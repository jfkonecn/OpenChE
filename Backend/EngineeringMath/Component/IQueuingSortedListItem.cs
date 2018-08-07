using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface QueuingSortedListItem<TKey, P> : ISortedListItem<TKey, P>
        where TKey : class
        where P : class
    {

        /// <summary>
        /// where 0 is the highest
        /// </summary>
        uint Priority { get; }
    }
}
