using System;

namespace ScientificUnits
{
    /// <summary>
    /// The basic interface for all units
    /// </summary>
    internal class Unit
    {


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">name of the unit</param>
        /// <param name="toStandard">        
        /// Conversion factor which takes this unit and makes it the standard
        /// Used as CurrentUnit * ToStandard = StandardUnit
        /// <para>For Example: </para>
        /// <para>If the standard length unit, chosen by the length class, is m and this unit is km,
        /// then ToStandard would have a value of 1000.</para>
        /// <para>If the standard length unit is m and this unit is m,
        /// then ToStandard would have a value of 1.</para></param>
        internal Unit(string name, double toStandard)
        {
            this.name = new String(name.ToCharArray());

            this.toStandard = toStandard;
        }
        
        
        /// <summary>
        /// name of the unit
        /// </summary>
        private string name;
        /// <summary>
        /// name of the unit
        /// </summary>
        internal string Name
        {
            get
            {
                return name;
            }
        }

        private double toStandard;

        /// <summary>
        /// Conversion factor which takes this unit and makes it the standard
        /// Used as CurrentUnit * ToStandard = StandardUnit
        /// For Example: 
        /// If the standard length unit, chosen by the length class, is m and this unit is km,
        /// then ToStandard would have a value of 1000.
        /// If the standard length unit is m and this unit is m, 
        /// then ToStandard would have a value of 1.
        /// </summary>
        internal double ToStandard
        {
            get
            {
                return toStandard;
            }
        }
        
    }
}
