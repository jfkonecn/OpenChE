using EngineeringMath.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BackendTesting
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class UnitConverter
    {
        [TestMethod]
        public void CustomUnit()
        {
            UnitCategory category = new UnitCategory("Temperature", true)
            {
                new Unit("Kelvin", "°K","curUnit * 9/5","baseUnit * 5/9", UnitSystem.UnitSystemType.SI, false, true),
                new Unit("Rankine","°R","","", UnitSystem.UnitSystemType.USCS, true, true),
                new Unit("Fahrenheit", "°F","curUnit + 459.67", "baseUnit - 459.67", UnitSystem.UnitSystemType.None, false, true),
                new Unit("Celsius", "°C","(curUnit + 273.15) * 9/5", "baseUnit * 5/9 - 273.15", UnitSystem.UnitSystemType.None, false, true)
            };
            Assert.AreEqual("Kelvin", category.GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemType.SI));
            Assert.AreEqual("Rankine", category.GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemType.USCS));
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
    }
}
