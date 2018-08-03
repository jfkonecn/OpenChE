using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Component;
using EngineeringMath;
using EngineeringMath.Resources;


namespace BackendTesting
{
    [TestClass]
    public class ComponentEquation
    {
        [TestMethod]
        public void CustomEquation()
        {
            /*UnitCategory cat = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Area);
            string siName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
                usName = cat.GetUnitFullNameByUnitSystem(UnitSystem.Imperial.USCS);

            SIUnitParameter r = new SIUnitParameter("x", 0, double.MaxValue, LibraryResources.Length)
            {
                Value = 10
            };

            Equation equation = new Equation(someParameters, "r * r")
            {
                
            };

            Assert.AreEqual(100, equation.Evaluate());


            Assert.AreEqual(150, equation.Evaluate());*/


        }

    }
}
