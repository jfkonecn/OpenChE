using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Stores data structures which include all unit types
    /// </summary>
    internal class StaticUnitProperties
    {
        /// <summary>
        /// quick reference lookup of all units
        /// </summary>
        internal readonly static Dictionary<Type, Dictionary<string, AbstractUnit>> AllUnits =
        new Dictionary<Type, Dictionary<string, AbstractUnit>>
        {
            { typeof(Density) , Density.StringToUnit },
            { typeof(Length) , Length.StringToUnit },
            { typeof(Mass) , Mass.StringToUnit },
            { typeof(Pressure) , Pressure.StringToUnit },
            { typeof(Temperature) , Temperature.StringToUnit },
            { typeof(Time) , Time.StringToUnit },
            { typeof(Unitless) , Unitless.StringToUnit },
            { typeof(Volume) , Volume.StringToUnit }
        };
    }
}
