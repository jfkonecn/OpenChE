using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IQueuingSortedListItem
    {

        /// <summary>
        /// where 0 is the highest
        /// </summary>
        uint Priority { get; }
    }
}
