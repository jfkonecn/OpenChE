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
            double x, y, low = 10, high = 100;
            for(int i = 0; i < 100; i++)
            {
                x = Rnd.NextDouble() * (high - low) + low;
                y = Rnd.NextDouble() * (high - low) + low;
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x} + {y}"), 1e-3);
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x} +{y}"), 1e-3);
                Assert.AreEqual(x + y, EquationParser.Evaluate($"{x}+{y}"), 1e-3);
            }
            Assert.ThrowsException<SyntaxException>(() => { EquationParser.Evaluate("1 + 4 +"); });
            Assert.ThrowsException<SyntaxException>(() => { EquationParser.Evaluate("*-+1 + 4"); });
        }

        [TestMethod]
        public void Function()
        {
            double x = 100, y = 10;
            Assert.AreEqual(Math.Sqrt(x), EquationParser.Evaluate($"Sqrt({x})"), 1e-3);
            Assert.AreEqual(Math.Pow(x, y), EquationParser.Evaluate($"Pow({x},{y})"), 1e-3);
        }

        [TestMethod]
        public void OrderOfOperations()
        {
            Assert.AreEqual(3, EquationParser.Evaluate(" 3 + 4 * 2 / ( 1 − 5 ) ^ 2 ^ 3 "), 1e-3);
            Assert.AreEqual(20.4, EquationParser.Evaluate($"20 * 2 - (1/2) * 9.8 * 2^2"), 1e-3);
            Assert.AreEqual(262144, EquationParser.Evaluate($"4^3^2"), 1e-3);
            Assert.AreEqual(160, EquationParser.Evaluate($"7 + (6 * 5^2 + 3)"), 1e-3);
        }

        [TestMethod]
        public void Variable()
        {
            Dictionary<string, double> dic = new Dictionary<string, double>()
            {
                { "Hello", 10 },
                { "World", 50 }
            };

            Func<string, double> varFun = 
                (string paraName) => 
                {
                    return dic[paraName];
                };
            Assert.AreEqual(60, EquationParser.Evaluate($"$Hello + $World", varFun), 1e-3);
        }
    }
}
