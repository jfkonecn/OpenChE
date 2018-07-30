using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Component;

namespace BackendTesting
{
    [TestClass]
    public class ComponentEquation
    {
        [TestMethod]
        public void StringTester()
        {
            SIUnitParameter x = new SIUnitParameter("x", 0, double.MaxValue)
            {
                Value = 10
            };
            SIUnitParameter y = new SIUnitParameter("y", 0, double.MaxValue)
            {
                Value = 10
            };
            ParameterList someParameters = new ParameterList()
            {
                x,
                y
            };
            Equation equation = new Equation("x * y")
            {
                Parameters = someParameters
            };

            Assert.AreEqual(100, equation.Evaluate());


            x.Value = 5;
            y.Value = 30;
            Assert.AreEqual(150, equation.Evaluate());
        }

    }
}
