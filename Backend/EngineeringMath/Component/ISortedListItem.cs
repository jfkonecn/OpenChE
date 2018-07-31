using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface ISortedListItem<TKey>
    {
        TKey Key
        {
            get;
        } 
    }
}
