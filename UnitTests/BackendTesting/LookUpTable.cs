using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Resources.LookupTables;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;

namespace BackendTesting
{
    [TestClass]
    public class LookUpTable
    {
        /// <summary>
        /// Tests the steam table 
        /// </summary>
        [TestMethod]
        public void SteamTableTest()
        {

            ThermoEntry entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(45, 0.01e6);
            Assert.AreEqual(entry.H, 188.44, 0.01, "H Temperture and Pressure. No Interpolation");
            Assert.AreEqual(entry.S, 0.63861, 0.01, "S Temperture and Pressure. No Interpolation");
            Assert.AreEqual(entry.V, 1.00992, 0.01, "V Temperture and Pressure. No Interpolation");
            Assert.AreEqual(entry.Temperature, 45, 0.01, "Temp Temperture and Pressure. No Interpolation");

            entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(45, 0);
            Assert.IsNull(entry, "Out of range pressure");

            entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(-50, 0.01e6);
            Assert.IsNull(entry, "Out of range temperature");

            entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(64.51, 0.2e6);
            Assert.AreEqual(entry.H, 270.20886, 0.01, "H Temperture and Pressure. Temperture Interpolation");
            Assert.AreEqual(entry.S, 0.8874397, 0.01, "S Temperture and Pressure. Temperture Interpolation");
            Assert.AreEqual(entry.V, 1.01952148, 0.01, "V Temperture and Pressure. Temperture Interpolation");
            Assert.AreEqual(entry.Temperature, 64.51, 0.01, "Temp Temperture and Pressure. Temperture Interpolation");

            entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(65, 0.211e6);
            Assert.AreEqual(entry.H, 272.271, 0.01, "H Temperture and Pressure. Pressure Interpolation");
            Assert.AreEqual(entry.S, 0.8935445, 0.01, "S Temperture and Pressure. Pressure Interpolation");
            Assert.AreEqual(entry.V, 1.0197845, 0.01, "V Temperture and Pressure. Pressure Interpolation");
            Assert.AreEqual(entry.Temperature, 65, 0.01, "Temp Temperture and Pressure. Pressure Interpolation");

            entry = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(64.51, 0.211e6);
            Assert.AreEqual(entry.H, 270.21986, 0.01, "H Temperture and Pressure. Temperture and Pressure Interpolation");
            Assert.AreEqual(entry.S, 0.8874342, 0.01, "S Temperture and Pressure. Temperture and Pressure Interpolation");
            Assert.AreEqual(entry.V, 1.01951598, 0.01, "V Temperture and Pressure. Temperture and Pressure Interpolation");
            Assert.AreEqual(entry.Temperature, 64.51, 0.01, "Temp Temperture and Pressure. Temperture and Pressure Interpolation");

            entry = SteamTable.Table.GetThermoEntrySatLiquidAtPressure(0.2e6);
            Assert.AreEqual(entry.H, 504.7, 0.01, "H Saturated Liquid. No Interpolation");
            Assert.AreEqual(entry.S, 1.5302, 0.01, "S Saturated Liquid. No Interpolation");
            Assert.AreEqual(entry.V, 1.06052, 0.01, "V Saturated Liquid. No Interpolation");
            Assert.AreEqual(entry.Temperature, 120.21, 0.01, "Temp Saturated Liquid. No Interpolation");

            entry = SteamTable.Table.GetThermoEntrySatVaporAtPressure(0.2e6);
            Assert.AreEqual(entry.H, 2706.2, 0.01, "H Saturated Vapor. No Interpolation");
            Assert.AreEqual(entry.S, 7.1269, 0.01, "S Saturated Vapor. No Interpolation");
            Assert.AreEqual(entry.V, 885.68, 0.01, "V Saturated Vapor. No Interpolation");
            Assert.AreEqual(entry.Temperature, 120.21, 0.01, "Temp Saturated Vapor. No Interpolation");

            entry = SteamTable.Table.GetThermoEntrySatLiquidAtPressure(0.211e6);
            Assert.AreEqual(entry.H, 511.8115, 0.01, "H Saturated Liquid. Interpolation");
            Assert.AreEqual(entry.S, 1.54813, 0.01, "S Saturated Liquid. Interpolation");
            Assert.AreEqual(entry.V, 1.062049, 0.01, "V Saturated Liquid. Interpolation");
            Assert.AreEqual(entry.Temperature, 121.882, 0.01, "Temp Saturated Liquid. Interpolation");

            entry = SteamTable.Table.GetThermoEntrySatVaporAtPressure(0.211e6);
            Assert.AreEqual(entry.H, 2708.62, 0.01, "H Saturated Vapor. Interpolation");
            Assert.AreEqual(entry.S, 7.10941, 0.01, "S Saturated Vapor. Interpolation");
            Assert.AreEqual(entry.V, 844.0945, 0.01, "V Saturated Vapor. Interpolation");
            Assert.AreEqual(entry.Temperature, 121.882, 0.01, "Temp Saturated Vapor. Interpolation");
        }
    }
}
