using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Units;
using System;

namespace BackendTesting
{
    [TestClass]
    public class EngineeringUnits
    {
        [TestMethod]
        /// <summary>
        /// Checks if the Unit.Covert methods are correct
        /// </summary>
        public void ConversionTest()
        {
            double curValue = 1e6,
                actual = -10,
                delta = 0.1;

            // Area test
            EngineeringMath.Units.Area areaUnit = EngineeringMath.Units.Area.m2;
            actual = EngineeringMath.Units.Area.Convert(curValue, areaUnit, EngineeringMath.Units.Area.ft2);
            Assert.AreEqual(10763911, actual, 1);

            // Density test
            Density densityUnit = Density.kgm3;
            actual = Density.Convert(curValue, densityUnit, Density.lbsft3);
            Assert.AreEqual(1e6 * (1 / Math.Pow(3.28084,3)) * 2.20462, actual, delta);

            // Energy test
            Energy energyUnit = Energy.kJ;
            actual = Energy.Convert(curValue, energyUnit, Energy.Therm);
            Assert.AreEqual(9.48043428, actual, delta);
            actual = Energy.Convert(curValue, energyUnit, Energy.kCal);
            Assert.AreEqual(239006, actual, delta);
            actual = Energy.Convert(curValue, energyUnit, Energy.J);
            Assert.AreEqual(1e9, actual, delta);
            actual = Energy.Convert(curValue, energyUnit, Energy.BTU);
            Assert.AreEqual(947817, actual, delta);

            // Enthalpy test
            Enthalpy enthalpyUnit = Enthalpy.kJkg;
            actual = Enthalpy.Convert(curValue, enthalpyUnit, Enthalpy.BTUlbs);
            Assert.AreEqual(1e6 * (0.947817) * (1 / 2.20462), actual, delta);

            // Entropy test
            Entropy entropyUnit = Entropy.kJkgK;
            actual = Entropy.Convert(curValue, entropyUnit, Entropy.BTUlbsR);
            Assert.AreEqual(1e6 * (0.947817) * (1 / 2.20462) * (5.0 / 9.0), actual, delta);

            // Length test
            Length lengthUnit = Length.m;
            actual = Length.Convert(curValue, lengthUnit, Length.ft);
            Assert.AreEqual(3280840, actual, delta);
            actual = Length.Convert(curValue, lengthUnit, Length.inch);
            Assert.AreEqual(3280840 * 12, actual, delta);

            // Mass test
            Mass massUnit = Mass.g;
            actual = Mass.Convert(curValue, massUnit, Mass.kg);
            Assert.AreEqual(1000, actual, delta);
            actual = Mass.Convert(curValue, massUnit, Mass.lbsm);
            Assert.AreEqual(2204.62262, actual, delta);
            actual = Mass.Convert(curValue, massUnit, Mass.MetricTon);
            Assert.AreEqual(1, actual, delta);
            actual = Mass.Convert(curValue, massUnit, Mass.USTon);
            Assert.AreEqual(1.10231131, actual, delta);

            // Pressure test
            Pressure pressureUnit = Pressure.Pa;
            actual = Pressure.Convert(curValue, pressureUnit, Pressure.atm);
            Assert.AreEqual(9.86923267, actual, delta);
            actual = Pressure.Convert(curValue, pressureUnit, Pressure.bar);
            Assert.AreEqual(10, actual, delta);
            actual = Pressure.Convert(curValue, pressureUnit, Pressure.kPa);
            Assert.AreEqual(1000, actual, delta);
            actual = Pressure.Convert(curValue, pressureUnit, Pressure.psia);
            Assert.AreEqual(145.037738, actual, delta);

            // Specific Volume test
            SpecificVolume specificVolumeUnit = SpecificVolume.m3kg;
            actual = SpecificVolume.Convert(curValue, specificVolumeUnit, SpecificVolume.ft3lbs);
            Assert.AreEqual(1e6 * (Math.Pow(3.28084, 3)) * (1 / 2.20462), actual, delta);

            // Temperature test
            Temperature temperatureUnit = Temperature.C;
            actual = Temperature.Convert(curValue, temperatureUnit, Temperature.F);
            Assert.AreEqual(1800032, actual, delta);
            actual = Temperature.Convert(curValue, temperatureUnit, Temperature.K);
            Assert.AreEqual(1000273.15, actual, delta);
            actual = Temperature.Convert(curValue, temperatureUnit, Temperature.R);
            Assert.AreEqual(1800491.67, actual, delta);

            // Time test
            Time timeUnit = Time.sec;
            actual = Time.Convert(curValue, timeUnit, Time.min);
            Assert.AreEqual(16666.6667, actual, delta);
            actual = Time.Convert(curValue, timeUnit, Time.hr);
            Assert.AreEqual(277.777778, actual, delta);
            actual = Time.Convert(curValue, timeUnit, Time.day);
            Assert.AreEqual(11.5740741, actual, delta);

            // Volume test
            Volume volumeUnit = Volume.ml;
            actual = Volume.Convert(curValue, volumeUnit, Volume.ft3);
            Assert.AreEqual(35.3146667, actual, delta);
            actual = Volume.Convert(curValue, volumeUnit, Volume.liter);
            Assert.AreEqual(1000, actual, delta);
            actual = Volume.Convert(curValue, volumeUnit, Volume.m3);
            Assert.AreEqual(1, actual, delta);
            actual = Volume.Convert(curValue, volumeUnit, Volume.USGallon);
            Assert.AreEqual(264.172052, actual, delta);




        }
    }
}
