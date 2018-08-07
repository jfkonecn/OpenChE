using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class QueuingSortedList<TKey, TValue, P> : NotifyPropertySortedList<TKey, TValue, P>
        where TKey : class
        where TValue : QueuingSortedListItem<TKey, P>
        where P : class
    {
        public QueuingSortedList(P parent) : base(parent)
        {

        }

        public QueuingSortedList(P parent, SortedList<TKey, TValue> list) : base(parent, list)
        {

        }

        public QueuingSortedList(NotifyPropertySortedList<TKey, TValue, P> list) : base(list)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TValue[] GetQueue()
        {
            return (from item in _List
                   select item.Value).OrderBy(x => x.Priority).ToArray();
        }
    }
}
