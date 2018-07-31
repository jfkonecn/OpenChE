using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public static class UnitSystem
    {
        public enum UnitSystemBaseUnit
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
