using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Resources.PVTTables;


namespace BackendTesting
{
    [TestClass]
    public class SteamTableMethods
    {
        private static readonly SteamTable SteamTable = new SteamTable();
        [TestMethod]
        public void PressureAndTemperature()
        {

            PressureAndTemperature(Region.Liquid, 750, 78.309563916917e6, 1.0/500, 2102.069317626429e3, 2258.688445460262e3, 
                4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0);
        }

        public void PressureAndTemperature(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            double maxErr = 1e-3;
            IThermoEntry entry = SteamTable.GetThermoEntryAtTemperatureAndPressure(temperature, pressure);
            Assert.AreEqual(temperature, entry.Temperature, maxErr);
            Assert.AreEqual(pressure, entry.Pressure, maxErr);
            Assert.AreEqual(specificVolume, entry.SpecificVolume, maxErr);
            Assert.AreEqual(1.0 / specificVolume, entry.Density, maxErr);
            Assert.AreEqual(internalEnergy, entry.InternalEnergy, maxErr);
            Assert.AreEqual(enthalpy, entry.Enthalpy, maxErr);
            Assert.AreEqual(entropy, entry.Entropy, maxErr);
            Assert.AreEqual(isochoricHeatCapacity, entry.IsochoricHeatCapacity, maxErr);
            Assert.AreEqual(isobaricHeatCapacity, entry.IsobaricHeatCapacity, maxErr);
            Assert.AreEqual(speedOfSound, entry.SpeedOfSound, maxErr);
            Assert.AreEqual(vaporFraction, entry.VaporFraction, maxErr);
            Assert.AreEqual(liquidFraction, entry.LiquidFraction, maxErr);
            Assert.AreEqual(solidFraction, entry.SolidFraction, maxErr);
        }
    }
}
