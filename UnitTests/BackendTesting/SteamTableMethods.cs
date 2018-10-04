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
        private static readonly SteamTable SteamTable = SteamTable.Table;
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
            
            IThermoEntry entry = SteamTable.GetThermoEntryAtTemperatureAndPressure(temperature, pressure);
            CheckEntryValues(entry, region, temperature,
            pressure, specificVolume, internalEnergy, enthalpy,
            entropy, isochoricHeatCapacity, isobaricHeatCapacity,
            speedOfSound,
            vaporFraction, liquidFraction, solidFraction);
        }

        [TestMethod]
        public void SatPressure()
        {
            SatPressure(SaturationRegion.Liquid, 393.361545936488,
            0.2e6, 0.00106051840643552, 504471.741847973, 504683.84552926,
            1530.0982011075, 3666.99397284121, 4246.73524917536,
            1520.69128792808,
            0, 1, 0);
            SatPressure(SaturationRegion.Vapor, 393.361545936488,
            0.2e6, 0.885735065081644, 2529094.32835793, 2706241.34137425,
            7126.8563914686, 1615.96336473298, 2175.22318865273,
            481.883535821489,
            1, 0, 0);

        }

        public void SatPressure(SaturationRegion region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            IThermoEntry entry = SteamTable.GetThermoEntryAtSatPressure(pressure, region);
            CheckEntryValues(entry, region, temperature,
            pressure, specificVolume, internalEnergy, enthalpy,
            entropy, isochoricHeatCapacity, isobaricHeatCapacity,
            speedOfSound,
            vaporFraction, liquidFraction, solidFraction);
        }



        [TestMethod]
        public void SatTemperature()
        {
            SatPressure(SaturationRegion.Liquid, 393.361545936488,
            0.2e6, 0.00106051840643552, 504471.741847973, 504683.84552926,
            1530.0982011075, 3666.99397284121, 4246.73524917536,
            1520.69128792808,
            0, 1, 0);
            SatPressure(SaturationRegion.Vapor, 393.361545936488,
            0.2e6, 0.885735065081644, 2529094.32835793, 2706241.34137425,
            7126.8563914686, 1615.96336473298, 2175.22318865273,
            481.883535821489,
            1, 0, 0);

        }


        public void SatTemperature(SaturationRegion region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            IThermoEntry entry = SteamTable.GetThermoEntryAtSatTemp(temperature, region);
            CheckEntryValues(entry, region, temperature,
            pressure, specificVolume, internalEnergy, enthalpy,
            entropy, isochoricHeatCapacity, isobaricHeatCapacity,
            speedOfSound,
            vaporFraction, liquidFraction, solidFraction);
        }


        [TestMethod]
        public void EnthalpyAndPressure()
        {
            EnthalpyAndPressure(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0 / 500, 2102.069317626429e3, 2258.688445460262e3,
    4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0);

            EnthalpyAndPressure(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3,
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0, 1, 0);
            EnthalpyAndPressure(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0);

            EnthalpyAndPressure(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0);

            EnthalpyAndPressure(Region.LiquidVapor, 318.957548207023, 10e3, 11.8087122249855, 1999135.82661328,
                2117222.94886314, 6.6858e3, 1966.28009225455, 2377.86300751001, 655.005141924186,
                0.804912447078132, 0.195087552921867, 0);
        }

        public void EnthalpyAndPressure(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            IThermoEntry entry = SteamTable.GetThermoEntryAtEnthalpyAndPressure(enthalpy, pressure);
            CheckEntryValues(entry, region, temperature,
            pressure, specificVolume, internalEnergy, enthalpy,
            entropy, isochoricHeatCapacity, isobaricHeatCapacity,
            speedOfSound,
            vaporFraction, liquidFraction, solidFraction);
        }


        [TestMethod]
        public void EntropyAndPressure()
        {
            EntropyAndPressure(Region.SupercriticalFluid, 750, 78.309563916917e6, 1.0 / 500, 2102.069317626429e3, 2258.688445460262e3,
4.469719056217e3, 2.71701677121e3, 6.341653594791e3, 760.696040876798, 0, 0, 0);

            EntropyAndPressure(Region.Liquid, 473.15, 40e6, 0.001122406088, 825.228016170348e3,
                870.124259682489e3, 2.275752861241e3, 3.292858637199e3, 4.315767590903e3, 1457.418351596083, 0, 1, 0);
            EntropyAndPressure(Region.SupercriticalFluid, 2000, 30e6, 0.03113852187, 5637.070382521894e3, 6571.226038618478e3,
                8.536405231138e3, 2.395894362358e3, 2.885698818781e3, 1067.369478777425, 0, 0, 0);

            EntropyAndPressure(Region.Gas, 823.15, 14e6, 0.024763222774, 3114.302136294585e3, 3460.987255128561e3,
                6.564768889364e3, 1.892708832325e3, 2.666558503968e3, 666.050616844223, 0, 0, 0);

            EntropyAndPressure(Region.LiquidVapor, 318.957548207023, 10e3, 11.8087122249855, 1999135.82661328,
                2117222.94886314, 6.6858e3, 1966.28009225455, 2377.86300751001, 655.005141924186, 
                0.804912447078132, 0.195087552921867, 0);
        }

        public void EntropyAndPressure(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            IThermoEntry entry = SteamTable.GetThermoEntryAtEntropyAndPressure(entropy, pressure);
            CheckEntryValues(entry, region, temperature,
            pressure, specificVolume, internalEnergy, enthalpy,
            entropy, isochoricHeatCapacity, isobaricHeatCapacity,
            speedOfSound,
            vaporMassFraction, liquidMassFraction, solidMassFraction);
        }

        private void CheckEntryValues(IThermoEntry entry, SaturationRegion region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            Region temp;
            switch (region)
            {
                case SaturationRegion.Vapor:
                    temp = Region.Vapor;
                    break;
                case SaturationRegion.Liquid:
                    temp = Region.Liquid;
                    break;
                case SaturationRegion.Solid:
                    temp = Region.Solid;
                    break;
                default:
                    throw new Exception("I don't know this region");
            }

            CheckEntryValues(entry, temp, temperature,
                pressure, specificVolume, internalEnergy, enthalpy,
                entropy, isochoricHeatCapacity, isobaricHeatCapacity,
                speedOfSound,
                vaporFraction, liquidFraction, solidFraction);
        }

        private void CheckEntryValues(IThermoEntry entry, Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction, double liquidFraction, double solidFraction)
        {
            double maxErr = 1e-3;
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
            Assert.AreEqual(vaporFraction, entry.VaporMassFraction, maxErr);
            Assert.AreEqual(liquidFraction, entry.LiquidMassFraction, maxErr);
            Assert.AreEqual(solidFraction, entry.SolidMassFraction, maxErr);
        }
    }
}
