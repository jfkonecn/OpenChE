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

            PressureAndTemperature(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0/500, 2102.069317626429e3, 2258.688445460262e3, 
                4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0);

            PressureAndTemperature(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3, 
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0,1,0);
            PressureAndTemperature(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0);

            PressureAndTemperature(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0);
        }

        public void PressureAndTemperature(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            double maxErr = 1e-3;
            IThermoEntry entry = SteamTable.GetThermoEntryAtTemperatureAndPressure(temperature, pressure);
            Assert.AreEqual(region, entry.Region);
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
