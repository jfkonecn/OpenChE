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
            AllEntries.Add(entry);
        }

        /// <summary>
        /// Gets ThermoEntry as this object's pressure and passed temperature
        /// <para>DO NOT get Saturated entries here! Use SatLiquidEntry and SatVaporEntry</para>
        /// </summary>
        /// <param name="temperature">Desired Temperture</param>
        /// <returns></returns>
        internal ThermoEntry GetThermoEntryAtTemperature(double temperature)
        {
            return ThermoEntry.Interpolation<ThermoEntry>.InterpolationThermoEntryFromList(
                temperature,
                AllEntries,
                delegate(ThermoEntry obj) 
                {
                    return obj.Temperature;
                },
                delegate (ThermoEntry obj)
                {
                    return obj;
                });
        }

        private List<ThermoEntry> AllEntries = new List<ThermoEntry>();

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
