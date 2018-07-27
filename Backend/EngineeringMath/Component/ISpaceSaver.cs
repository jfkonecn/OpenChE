using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface ISpaceSaver
    {
        /// <summary>
        /// To save space when this function is not in use we remove any parameters which are not needed to recreate this function
        /// </summary>
        void Nullify();
    }
}
