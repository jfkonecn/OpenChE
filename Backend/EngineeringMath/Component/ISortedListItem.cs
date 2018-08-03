using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">key used to sort list</typeparam>
    /// <typeparam name="P">Parent type</typeparam>
    public interface ISortedListItem<TKey, P> : IChildItem<P> 
        where TKey : class
        where P : class
    {
        TKey Key
        {
            get;
        } 
    }
}
