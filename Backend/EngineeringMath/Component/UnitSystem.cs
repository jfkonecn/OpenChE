using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class UnitSystem
    {
        public enum UnitSystemType
        {
            /// <summary>
            /// International System of Units 
            /// </summary>
            SI,
            /// <summary>
            /// United States customary system
            /// </summary>
            USCS,
            /// <summary>
            /// No system
            /// </summary>
            None
        }
    }
}
