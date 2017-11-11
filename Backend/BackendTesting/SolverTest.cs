using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;

namespace BackendTesting
{
    [TestClass]
    public class SolverTest
    {
        private static Random rnd = new Random();
        [TestMethod]
        public void NewtonsMethodTest()
        {
            double expected = 0,
                actual = 0;
            for (int i = 0; i < 1e10; i++)
            {
                expected = (rnd.NextDouble() - 0.5) * 1e1;
                double y = Math.Log(expected);
                if (!y.Equals(double.NaN))
                {
                    actual = Solver.NewtonsMethod(y, x => Math.Log(x), 1e-6, 1e-63);
                    Assert.AreEqual(expected, actual, 0.001, "Standard Case");
                }
                

                
            }

            

            //Valid Inputs
            

         



        }
    }
}
