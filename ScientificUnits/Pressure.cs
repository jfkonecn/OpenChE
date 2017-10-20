using System;
using System.Collections.Generic;
using System.Text;

namespace ScientificUnits
{
    /// <summary>
    /// Type of Pressure Unit
    /// </summary>
    public class Pressure
    {
        /// <summary>
        /// Contains all the different types of pressure units
        /// </summary>
        readonly static Unit[] allUnits = {
            new Unit("Pa", 1.0),
            new Unit("kPa", 1000.0)
        };
    }
}
