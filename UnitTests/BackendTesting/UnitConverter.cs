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
                new Unit("Kelvin", "°K","curUnit * 9/5","baseUnit * 5/9", UnitSystem.UnitSystemBaseUnit.SI, false, true),
                new Unit("Rankine","°R","","", UnitSystem.UnitSystemBaseUnit.USCS, true, true),
                new Unit("Fahrenheit", "°F","curUnit + 459.67", "baseUnit - 459.67", UnitSystem.UnitSystemBaseUnit.None, false, true),
                new Unit("Celsius", "°C","(curUnit + 273.15) * 9/5", "baseUnit * 5/9 - 273.15", UnitSystem.UnitSystemBaseUnit.None, false, true)
            };
            Assert.AreEqual("Kelvin", category.GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemBaseUnit.SI));
            Assert.AreEqual("Rankine", category.GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemBaseUnit.USCS));
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
        public void AreaUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Area);
            double temp = cat.ConvertUnit(LibraryResources.MetersSquared, LibraryResources.FeetSquared, 100);
            Assert.AreEqual(1076.39, temp, 1);
            temp = cat.ConvertUnit(LibraryResources.FeetSquared, LibraryResources.MetersSquared, temp);
            Assert.AreEqual(100, temp, 1);
        }

        [TestMethod]
        public void DensityUnits()
        {
            UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Density);
            double temp = cat.ConvertUnit(LibraryResources.KgPerMeterCubed, LibraryResources.LbsmPerFeetCubed, 125);
            Assert.AreEqual(7.8035, temp, 0.001);
            temp = cat.ConvertUnit(LibraryResources.LbsmPerFeetCubed, LibraryResources.KgPerMeterCubed, temp);
            Assert.AreEqual(125, temp, 0.1);
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
        public void EnthalpyUnits()
        {
            
        }
    }
}
