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
            
            

            for (int i = 0; i < 1e2; i++)
            {
                expected = (rnd.NextDouble()) * 1e10 + 1;
                double y = Math.Log(expected);
                if (!y.Equals(double.NaN))
                {
                    actual = Solver.NewtonsMethod(y, x => Math.Log(x), minValueDbl: 1d);
                    Assert.AreEqual(expected, actual, expected * 1e-3, "Standard Case");
                }
                

                
            }

            

            //Valid Inputs
            

         



        }
    }
}
