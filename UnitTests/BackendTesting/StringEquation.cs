using System;
using System.Collections.Generic;
using System.Text;
using StringMath;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackendTesting
{
    [TestClass]
    public class StringEquation
    {
        private readonly Random Rnd = new Random();
        [TestMethod]
        public void Add()
        {
            double x, y;
            for(int i = 0; i < 100; i++)
            {
                x = Rnd.NextDouble();
                y = Rnd.NextDouble();
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x} + {y}"), 1e-3);
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x} +{y}"), 1e-3);
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x}+{y}"), 1e-3);
            }
            Assert.ThrowsException<SyntaxException>(() => { EquationParser.Evaluate("1 + 4 +"); });
            Assert.ThrowsException<SyntaxException>(() => { EquationParser.Evaluate("*-+1 + 4"); });
        }
    }
}
