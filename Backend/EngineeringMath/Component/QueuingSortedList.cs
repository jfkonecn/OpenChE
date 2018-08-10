using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class QueuingSortedList<TValue, P> : NotifyPropertySortedList<TValue, P>
        where TValue : ChildItem<P>, IQueuingSortedListItem
        where P : class
    {
        public QueuingSortedList(P parent) : base(parent)
        {

        }

        public QueuingSortedList(P parent, SortedList<string, TValue> list) : base(parent, list)
        {

        }

        public QueuingSortedList(NotifyPropertySortedList<TValue, P> list) : base(list)
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
