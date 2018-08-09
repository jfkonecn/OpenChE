using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IQueuingSortedListItem<P> : IChildItem<P>
        where P : class
    {

        /// <summary>
        /// where 0 is the highest
        /// </summary>
        uint Priority { get; }
    }
}
