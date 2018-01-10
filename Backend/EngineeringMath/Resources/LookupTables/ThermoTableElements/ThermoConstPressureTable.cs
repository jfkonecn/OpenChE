using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources.LookupTables;

namespace EngineeringMath.Resources.LookupTables.ThermoTableElements
{
    /// <summary>
    /// Collection of thermo entries @ constant pressure
    /// </summary>
    internal class ThermoConstPressureTable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pressure">The pressure all Thermo Entries in this object are recorded at (in Pa)</param>
        /// <param name="satTemp">The saturation temperature all Thermo Entries in this object are recorded at (in C)</param>
        /// <param name="satLiquidEntry">Saturation Liquid Thermo Entry</param>
        /// /// <param name="satVaporEntry">Saturation Vapor Thermo Entry</param>
        internal ThermoConstPressureTable(double pressure, double satTemp, ThermoEntry satLiquidEntry, ThermoEntry satVaporEntry)
        {
            Pressure = pressure;
            SatTemp = satTemp;
            SatLiquidEntry = satLiquidEntry;
            SatVaporEntry = satVaporEntry;
        }

        /// <summary>
        /// Adds ThermoEntry as this object's pressure 
        /// <para>DO NOT add Saturated entries here! Use SatLiquidEntry and SatVaporEntry</para>
        /// </summary>
        /// <param name="entry">The entry to be added to this class</param>
        internal void AddThermoEntry(ThermoEntry entry)
        {
            AllEntries.Add(entry.Temperature, entry);
        }

        /// <summary>
        /// Gets ThermoEntry as this object's pressure and passed temperature
        /// <para>DO NOT get Saturated entries here! Use SatLiquidEntry and SatVaporEntry</para>
        /// </summary>
        /// <param name="temperature">Desired Temperture</param>
        /// <returns></returns>
        internal ThermoEntry GetThermoEntryAtTemperature(double temperature)
        {
            // to be returned
            ThermoEntry returnEntry;
            // check if temperature already is in the dictionary
            if (AllEntries.TryGetValue(temperature, out returnEntry))
            {
                return returnEntry;
            }
            else
            {
                // else interpolate
                // Interpolated Specific Volume
                double interV,
                    // Interpolated Enthalpy
                    interH,
                    // Interpolated Entropy
                    interS,
                    lowTemp = double.MinValue,
                    highTemp = double.MaxValue;

                // find the two closest entries in
                ThermoEntry lowTempEntry = null, 
                    highTempEntry = null;

                foreach(ThermoEntry entry in AllEntries.Values)
                {
                    if(entry.Temperature > lowTemp && entry.Temperature < temperature)
                    {
                        lowTemp = entry.Temperature;
                    }
                    else if(entry.Temperature < highTemp && entry.Temperature > temperature)
                    {
                        highTemp = entry.Temperature;
                    }
                }

                if (AllEntries.TryGetValue(lowTemp, out lowTempEntry) && AllEntries.TryGetValue(highTemp, out highTempEntry))
                {
                    interV = Interpolation.LinearInterpolation(temperature, 
                        lowTempEntry.Temperature, lowTempEntry.V, 
                        highTempEntry.Temperature, highTempEntry.V);

                    interH = Interpolation.LinearInterpolation(temperature,
                        lowTempEntry.Temperature, lowTempEntry.H,
                        highTempEntry.Temperature, highTempEntry.H);

                    interS = Interpolation.LinearInterpolation(temperature,
                        lowTempEntry.Temperature, lowTempEntry.S,
                        highTempEntry.Temperature, highTempEntry.S);

                    return new ThermoEntry(temperature, interV, interH, interS);
                }
                else
                {
                    throw new Exception("Temperature is out of range!");
                }
            }
        }

        private Dictionary<double, ThermoEntry> AllEntries = new Dictionary<double, ThermoEntry>();

        /// <summary>
        /// Thermo Entry for the saturated liquid at this object's pressure
        /// </summary>
        internal readonly ThermoEntry SatLiquidEntry;

        /// <summary>
        /// Thermo Entry for the saturated vapor at this object's pressure
        /// </summary>
        internal readonly ThermoEntry SatVaporEntry;


        /// <summary>
        /// The pressure all Thermo Entries in this object are recorded at (in Pa)
        /// </summary>
        internal readonly double Pressure;

        /// <summary>
        /// The saturation temperature all Thermo Entries in this object are recorded at (in C)
        /// </summary>
        internal readonly double SatTemp;

    }
}
