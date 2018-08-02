using EngineeringMath.Component;
using EngineeringMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace BackendTesting
{
    [TestClass]
    public class UnitConverter
    {

        [TestMethod]
        public void CustomUnit()
        {
            UnitCategory category = new UnitCategory("Temperature", true)
            {
                new Unit("Kelvin", "°K", 1.8, UnitSystem.Metric.SI, true),
                new Unit("Rankine","°R", UnitSystem.Imperial.USCS, true, true),
                new Unit("Fahrenheit", "°F",$"{Unit.CurUnitVar} + 459.67", $"{Unit.BaseUnitVar} - 459.67", UnitSystem.Imperial.BaselineSystem, true),
                new Unit("Celsius", "°C",$"({Unit.CurUnitVar} + 273.15) * 9/5", $"{Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.Metric.BaselineSystem, true)
            };
            Assert.AreEqual("Kelvin", category.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI));
            Assert.AreEqual("Rankine", category.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS));
            Assert.AreEqual("Rankine", category.BaseUnitFullName);
            double num = category.ConvertUnit("Celsius", "Fahrenheit", 100);
            Assert.AreEqual(212, num);
            num = category.ConvertUnit("Fahrenheit", "Rankine", num);
            Assert.AreEqual(671.67, num);
            num = category.ConvertUnit("Rankine", "Kelvin", num);
            Assert.AreEqual(373.15, num);
            num = category.ConvertUnit("Kelvin", "Celsius", num);
            Assert.AreEqual(100, num);

        }

        [TestMethod]
        public void TemperatureUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Temperature);
            double temp = cat.ConvertUnit(LibraryResources.Kelvin, LibraryResources.Fahrenheit, 373.15);
            Assert.AreEqual(212, temp);
            temp = cat.ConvertUnit(LibraryResources.Fahrenheit, LibraryResources.Celsius, temp);
            Assert.AreEqual(100, temp);
            temp = cat.ConvertUnit(LibraryResources.Celsius, LibraryResources.Rankine, temp);
            Assert.AreEqual(671.67, temp);
            temp = cat.ConvertUnit(LibraryResources.Rankine, LibraryResources.Kelvin, temp);
            Assert.AreEqual(373.15, temp);
        }

        [TestMethod]
        public void EnergyUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Energy);
            double temp = cat.ConvertUnit(LibraryResources.Joules, LibraryResources.Kilojoules, 125);
            Assert.AreEqual(0.125, temp);
            temp = cat.ConvertUnit(LibraryResources.Kilojoules, LibraryResources.Kilocalories, temp);
            Assert.AreEqual(0.0298757, temp, 0.00001);
            temp = cat.ConvertUnit(LibraryResources.Kilocalories, LibraryResources.BTU, temp);
            Assert.AreEqual(0.118477, temp, 0.0001);
            temp = cat.ConvertUnit(LibraryResources.BTU, LibraryResources.Therms, temp);
            Assert.AreEqual(1.1851e-6, temp, 1e-8);
            temp = cat.ConvertUnit(LibraryResources.Therms, LibraryResources.Joules, temp);
            Assert.AreEqual(125, temp);
        }

        [TestMethod]
        public void LengthUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Length);
            double temp = cat.ConvertUnit(LibraryResources.Meters, LibraryResources.Feet, 125);
            Assert.AreEqual(410.105, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.Feet, LibraryResources.Inches, temp);
            Assert.AreEqual(4921.26, temp, 0.01);
            temp = cat.ConvertUnit(LibraryResources.Inches, LibraryResources.Miles, temp);
            Assert.AreEqual(0.0776714, temp, 0.0001);
            temp = cat.ConvertUnit(LibraryResources.Miles, LibraryResources.Millimeters, temp);
            Assert.AreEqual(125000, temp);
            temp = cat.ConvertUnit(LibraryResources.Millimeters, LibraryResources.Centimeters, temp);
            Assert.AreEqual(12500, temp);
            temp = cat.ConvertUnit(LibraryResources.Centimeters, LibraryResources.Kilometers, temp);
            Assert.AreEqual(0.125, temp);
            temp = cat.ConvertUnit(LibraryResources.Kilometers, LibraryResources.Meters, temp);
            Assert.AreEqual(125, temp);
        }


        [TestMethod]
        public void MassUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Mass);
            double temp = cat.ConvertUnit(LibraryResources.Kilograms, LibraryResources.PoundsMass, 125);
            Assert.AreEqual(275.578, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.PoundsMass, LibraryResources.Grams, temp);
            Assert.AreEqual(125e3, temp);
            temp = cat.ConvertUnit(LibraryResources.Grams, LibraryResources.Milligrams, temp);
            Assert.AreEqual(125e6, temp);
            temp = cat.ConvertUnit(LibraryResources.Milligrams, LibraryResources.Micrograms, temp);
            Assert.AreEqual(125e9, temp);
            temp = cat.ConvertUnit(LibraryResources.Micrograms, LibraryResources.MetricTons, temp);
            Assert.AreEqual(0.125, temp);
            temp = cat.ConvertUnit(LibraryResources.MetricTons, LibraryResources.Ounces, temp);
            Assert.AreEqual(4409.25, temp, 0.01);
            temp = cat.ConvertUnit(LibraryResources.Ounces, LibraryResources.USTons, temp);
            Assert.AreEqual(0.137789, temp, 0.0001);
            temp = cat.ConvertUnit(LibraryResources.USTons, LibraryResources.Kilograms, temp);
            Assert.AreEqual(125, temp, 0.00001);
        }


        [TestMethod]
        public void AreaUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Area);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is m2 and USCS is ft2
            double temp = cat.ConvertUnit(siName, usName, 100);
            Assert.AreEqual(1076.39, temp, 1);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(100, temp, 1);
        }

        [TestMethod]
        public void DensityUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Density);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is kg/m3 and USCS is lbsm/ft3
            double temp = cat.ConvertUnit(siName, usName, 125);
            Assert.AreEqual(7.8035, temp, 0.001);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125, temp, 0.1);
        }



        [TestMethod]
        public void EnthalpyUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Enthalpy);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is J/kg and USCS is BTU/lbsm
            double temp = cat.ConvertUnit(siName, usName, 125000);
            Assert.AreEqual(125000 * 4.302104e-4, temp, 0.05);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125000, temp, 0.1);
        }

        [TestMethod]
        public void EntropyUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Entropy);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is J/kg and USCS is BTU/lbsm
            double temp = cat.ConvertUnit(siName, usName, 125000);
            Assert.AreEqual(125000 * 2.390057e-4, temp, 0.05);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125000, temp, 0.1);
        }

        [TestMethod]
        public void IsothermalCompressibilityUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.IsothermalCompressibility);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is 1/Pa and USCS is 1/psi
            double temp = cat.ConvertUnit(siName, usName, 125);
            Assert.AreEqual(861845, temp, 0.01);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125, temp, 0.1);
        }

        [TestMethod]
        public void SpecificVolumeUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.SpecificVolume);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is m3/kg and USCS is ft3/lbsm
            double temp = cat.ConvertUnit(siName, usName, 1/125.0);
            Assert.AreEqual(1/7.8035, temp, 0.001);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(1/125, temp, 0.1);
        }

        [TestMethod]
        public void PowerUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Power);
            double temp = cat.ConvertUnit(LibraryResources.Kilowatt, LibraryResources.Watt, 125);
            Assert.AreEqual(125e3, temp);
            temp = cat.ConvertUnit(LibraryResources.Watt, LibraryResources.Horsepower, temp);
            Assert.AreEqual(167.627761, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.Horsepower, LibraryResources.Kilowatt, temp);
            Assert.AreEqual(125, temp, 0.0001);
        }

        [TestMethod]
        public void PressureUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Pressure);
            double temp = cat.ConvertUnit(LibraryResources.Kilopascals, LibraryResources.Pascals, 125);
            Assert.AreEqual(125e3, temp);
            temp = cat.ConvertUnit(LibraryResources.Pascals, LibraryResources.Atmospheres, temp);
            Assert.AreEqual(1.23365, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.Atmospheres, LibraryResources.Bar, temp);
            Assert.AreEqual(1.25, temp);
            temp = cat.ConvertUnit(LibraryResources.Bar, LibraryResources.PoundsForcePerSqIn, temp);
            Assert.AreEqual(18.1297, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.PoundsForcePerSqIn, LibraryResources.Torr, temp);
            Assert.AreEqual(937.577, temp, 0.01);
            temp = cat.ConvertUnit(LibraryResources.Torr, LibraryResources.Kilopascals, temp);
            Assert.AreEqual(125, temp);
        }

        [TestMethod]
        public void TimesUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Time);
            double temp = cat.ConvertUnit(LibraryResources.Seconds, LibraryResources.Milliseconds, 125);
            Assert.AreEqual(125e3, temp);
            temp = cat.ConvertUnit(LibraryResources.Milliseconds, LibraryResources.Minutes, temp);
            Assert.AreEqual(125/60.0, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.Minutes, LibraryResources.Hours, temp);
            Assert.AreEqual(125/(3600.0), temp, 0.0001);
            temp = cat.ConvertUnit(LibraryResources.Hours, LibraryResources.Days, temp);
            Assert.AreEqual(125 / (3600.0 * 24), temp, 0.0001);
            temp = cat.ConvertUnit(LibraryResources.Days, LibraryResources.Seconds, temp);
            Assert.AreEqual(125, temp, 0.0001);
        }



        [TestMethod]
        public void VolumeUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Volume);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is m3 and USCS is ft3
            double temp = cat.ConvertUnit(siName, usName, 125);
            Assert.AreEqual(4414.33, temp, 0.05);
            temp = cat.ConvertUnit(usName, LibraryResources.Gallons, temp);
            Assert.AreEqual(33021.5, temp, 0.1);
            temp = cat.ConvertUnit(LibraryResources.Gallons, LibraryResources.Milliliters, temp);
            Assert.AreEqual(125e6, temp, 0.1);
            temp = cat.ConvertUnit(LibraryResources.Milliliters, LibraryResources.Liters, temp);
            Assert.AreEqual(125e3, temp, 0.1);
            temp = cat.ConvertUnit(LibraryResources.Liters, siName, temp);
            Assert.AreEqual(125, temp, 0.1);
        }

        [TestMethod]
        public void VolumeExpansivityUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.VolumeExpansivity);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is 1/K and USCS is 1/R
            double temp = cat.ConvertUnit(siName, usName, 125);
            Assert.AreEqual(125.0 * 5 / 9, temp, 0.001);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125, temp, 0.1);
        }

        [TestMethod]
        public void VolumetricFlowRateUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.VolumetricFlowRate);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                    usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);
            // SI is 1/K and USCS is 1/R
            double temp = cat.ConvertUnit(siName, usName, 125);
            Assert.AreEqual(125.0 * 35.3147, temp, 0.01);
            temp = cat.ConvertUnit(usName, siName, temp);
            Assert.AreEqual(125, temp, 0.1);
        }
    }
}
