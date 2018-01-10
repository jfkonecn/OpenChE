using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Resources.LookupTables.ThermoTableElements
{
    /// <summary>
    /// Specific Volume, Enthalpy and Entropy at a given temperature
    /// </summary>
    internal class ThermoEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">Temperture this entry is recorded at (in C)</param>
        /// <param name="v">Specific Volume (m3/kg)</param>
        /// <param name="h">Enthalpy (kJ/kg)</param>
        /// <param name="s">Entropy (kJ/(kg*K))</param>
        internal ThermoEntry(double temperature, double v, double h, double s)
        {
            Temperature = temperature;
            V = v;
            H = h;
            S = s;
        }

        /// <summary>Temperture this entry is recorded at (in C)</summary>
        internal readonly double Temperature;
        /// <summary>
        /// Specific Volume (m3/kg)
        /// </summary>
        internal readonly double V;
        /// <summary>
        /// Enthalpy (kJ/kg)
        /// </summary>
        internal readonly double H;
        /// <summary>
        /// Entropy (kJ/(kg*K))
        /// </summary>
        internal readonly double S;
    }
}
