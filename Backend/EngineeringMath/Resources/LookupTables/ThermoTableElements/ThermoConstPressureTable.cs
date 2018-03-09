using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            if(satTemp != satLiquidEntry.Temperature || satTemp != satVaporEntry.Temperature)
            {
                throw new Exception("ThermoConstPressureTable: Something went wrong");
            }
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
            AllEntries.OrderBy(x => x.Temperature);
        }

        /// <summary>
        /// Gets ThermoEntry as this object's pressure and passed temperature. Null when no entry found.
        /// <para>DO NOT get Saturated entries here! Use SatLiquidEntry and SatVaporEntry</para>
        /// </summary>
        /// <param name="temperature">Desired Temperture</param>
        /// <returns></returns>
        internal ThermoEntry GetThermoEntryAtTemperature(double temperature)
        {
            return ThermoEntry.Interpolation<ThermoEntry>.InterpolationThermoEntryFromList(
                temperature,
                AllEntries,
                delegate(ThermoEntry entry) 
                {
                    return entry.Temperature;
                },
                delegate (ThermoEntry obj)
                {
                    return obj;
                },
                delegate (ThermoEntry entry, ThermoEntry.Phase phase)
                {
                    if (phase.Equals(ThermoEntry.Phase.liquid))
                    {
                        return SatLiquidEntry;
                    }
                    else if (phase.Equals(ThermoEntry.Phase.vapor))
                    {
                        return SatVaporEntry;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                });
        }

        /// <summary>
        /// Where the key is the temperature of the entry
        /// </summary>
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

        /// <summary>
        /// Returns the smallest temperature in this object
        /// </summary>
        internal double MinTemperature
        {
            get
            {
                return AllEntries.Min(x => x.Temperature);
            }
        }

        /// <summary>
        /// Returns the largest temperature in this object
        /// </summary>
        internal double MaxTemperature
        {
            get
            {
                return AllEntries.Max(x => x.Temperature);
            }
        }

        internal ThermoEntry this[int i]
        {
            get
            {
                if(i < 0 || i >= Count)
                {
                    throw new Exception("i cannot be less than zero or greater than or equal to the count!");
                }

                if (AllEntries[0].Temperature > SatTemp || AllEntries[AllEntries.Count - 1].Temperature < SatTemp)      
                {
                    throw new Exception("There SatTemp must have entries above and below it!");
                }
                else if (i < AllEntries.Count)
                {
                    if(AllEntries[i].Temperature > SatTemp)
                    {
                        if (AllEntries[i - 1].Temperature < SatTemp)
                        {
                            return SatLiquidEntry;
                        }
                        else if (AllEntries[i - 2].Temperature < SatTemp)
                        {
                            return SatVaporEntry;
                        }
                        else
                        {
                            i -= 2;
                        }
                    }
                }
                else
                {
                    i -= 2;
                }


                return AllEntries[i];
            }
        }

        internal int Count
        {
            get
            {
                // add 2 to account for sat liquid and sat vapor
                return AllEntries.Count + 2;
            }
        }

    }
}
