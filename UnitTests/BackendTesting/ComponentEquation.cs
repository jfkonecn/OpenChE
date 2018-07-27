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
            ParameterList someParameters = new ParameterList()
            {
                new SIUnitParameter("x", 0, double.MaxValue)
                {
                    Value = 10
                },
                new SIUnitParameter("y", 0, double.MaxValue)
                {
                    Value = 10
                }
            };
            Equation equation = new Equation()
            {
                Parameters = someParameters,
                EquationExpression = "x * y"
            };

            Assert.AreEqual(100, equation.Evaluate());

            equation.EquationExpression = string.Empty;

            equation.Parameters = new ParameterList()
            {
                new SIUnitParameter("s", 0, double.MaxValue)
                {
                    Value = 10
                },
                new SIUnitParameter("t", 0, double.MaxValue)
                {
                    Value = 10
                }
            };

            equation.EquationExpression = "s * t";
            Assert.AreEqual(100, equation.Evaluate());
        }

    }
}
