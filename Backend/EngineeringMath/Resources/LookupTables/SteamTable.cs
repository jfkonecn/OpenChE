using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;

namespace EngineeringMath.Resources.LookupTables
{
    /// <summary>
    /// Source: https://www.nist.gov/sites/default/files/documents/srd/NISTIR5078-Tab3.pdf
    /// </summary>
    public class SteamTable
    {
        public static readonly SteamTable Table = new SteamTable();
        private SteamTable()
        {
            Assembly assembly = Assembly.Load(new AssemblyName("EngineeringMath"));
            string resourceName = "EngineeringMath.Resources.LookupTables.SteamTable.csv";

            

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {

                    string[] firstLine = reader.ReadLine().Split(',');
                    // skip next line
                    reader.ReadLine().Split(',');
                    CreateTableElementsList(
                        firstLine, 
                        reader.ReadLine().Split(','), 
                        reader.ReadLine().Split(','));
                    while (!reader.EndOfStream)
                    {
                        string[] line = reader.ReadLine().Split(',');
                        int i = 0, j = 0;
                        while (i < TableElements.Count)
                        {
                            j = 6 * i;
                            TableElements[i].AddThermoEntry(CreateEntry(line, j));
                            i++;
                        }
                    }
                }
            }            
        }

        /// <summary>
        /// Creates all elements in this table
        /// <para>Assumes pressure units in the string array are in MPa and temperature units are in C</para>
        /// </summary>
        /// <param name="firstLine">First line of the csv file assumed to have the pressure and saturated temperature</param>
        /// <param name="thirdLine">Third line of the csv file assumed to be saturated liquid thermo entry</param>
        /// <param name="fourthLine">Fourth line of the csv file assumed to be saturated vapor thermo entry</param>
        private void CreateTableElementsList(string[] firstLine, string[] thirdLine, string[] fourthLine)
        {
            int i = 0;

            while(i < firstLine.Length)
            {
                // assume pressure is in units of MPa
                double pressure = double.Parse(firstLine[i + 1]) * 1e6,                
                    // assume temperature is in units of C
                    satTemp = double.Parse(firstLine[i + 3]);

                ThermoEntry satLiq = CreateEntry(thirdLine, i, satTemp),
                    satVap = CreateEntry(fourthLine, i, satTemp);

                TableElements.Add(new ThermoConstPressureTable(pressure, satTemp, satLiq, satVap));
                i += 6;
            }
        }

        /// <summary>
        /// Creates a thermo entry based on a passed line of the csv file the current index being examined
        /// </summary>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <param name="temperature">in C</param>
        /// <returns></returns>
        private ThermoEntry CreateEntry(string[] line, int i, double temperature)
        {
            return new ThermoEntry(temperature,
                    // Specific Volume Assume m3/kg
                    double.Parse(line[i + 1]),
                    // Enthalpy Assume kJ/kg 
                    double.Parse(line[i + 3]),
                    // Entropy Assume kJ/(kg * K)
                    double.Parse(line[i + 4]));
        }

        /// <summary>
        /// Creates a thermo entry based on a passed line of the csv file the current index being examined
        /// </summary>
        /// <param name="line"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private ThermoEntry CreateEntry(string[] line, int i)
        {
            return CreateEntry(line, i, double.Parse(line[i]));
        }

        /// <summary>
        /// Gets ThermoEntry at passed pressure and passed temperature. Null when no entry found.
        /// </summary>
        /// <param name="temperature">Desired Temperture (C)</param>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntryAtTemperatureAndPressure(double temperature, double pressure)
        {
            return ThermoEntry.Interpolation<ThermoConstPressureTable>.InterpolationThermoEntryFromList(
                pressure,
                TableElements,
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.Pressure;
                },
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.GetThermoEntryAtTemperature(temperature);
                });
        }


        /// <summary>
        /// Gets ThermoEntry for saturated liquid at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntrySatLiquidAtPressure(double pressure)
        {
            return ThermoEntry.Interpolation<ThermoConstPressureTable>.InterpolationThermoEntryFromList(
                pressure,
                TableElements,
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.Pressure;
                },
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.SatLiquidEntry;
                });
        }


        /// <summary>
        /// Gets ThermoEntry for saturated vapor at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        public ThermoEntry GetThermoEntrySatVaporAtPressure(double pressure)
        {
            return ThermoEntry.Interpolation<ThermoConstPressureTable>.InterpolationThermoEntryFromList(
                pressure,
                TableElements,
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.Pressure;
                },
                delegate (ThermoConstPressureTable obj)
                {
                    return obj.SatVaporEntry;
                });
        }


        private List<ThermoConstPressureTable> TableElements = new List<ThermoConstPressureTable>();
    }
}
