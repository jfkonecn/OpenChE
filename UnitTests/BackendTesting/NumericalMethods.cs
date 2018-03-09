using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.NumericalMethods.FiniteDifferenceFormulas;

namespace BackendTesting
{
    [TestClass]
    public class NumericalMethods
    {
        [TestMethod]
        public void FiniteDifferenceFormulas()
        {
            // First Derivative
            double actual = FirstDerivative.ThreePointBackward(2 * 2, 1, 0, 1);
            Assert.AreEqual(actual, 0, "First Derivative");

            actual = FirstDerivative.TwoPointCentral(1, 1, 1);
            Assert.AreEqual(actual, 0, "First Derivative");

            actual = FirstDerivative.ThreePointForward(0, 1, 2 * 2, 1);
            Assert.AreEqual(actual, 0, "First Derivative");
        }
    }
}
