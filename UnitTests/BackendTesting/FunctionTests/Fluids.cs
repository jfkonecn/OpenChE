using EngineeringMath.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendTesting.FunctionTests
{
    [TestClass]
    public class Fluids
    {
        private void RunFluidsSolveTest(string funName, Dictionary<string, object> paramValues)
        {
            ComponentFunction.RunFunctionSolveTest(LibraryResources.FluidDynamics, funName, paramValues);
        }
        [TestMethod]
        public void OrificePlate()
        {
            RunFluidsSolveTest(LibraryResources.OrificePlate, new Dictionary<string, object>()
            {
                { "dc", 0.7 },
                { "rho", 1000.0 },
                { "pArea", 10 * 10 * Math.PI / 4.0 },
                { "oArea",  8 * 8 * Math.PI / 4.0 },
                { "deltaP", 10.0 },
                { "Q", 6.476 }
            });
        }

        [TestMethod]
        public void BernoullisEquation()
        {
            RunFluidsSolveTest(LibraryResources.BernoullisEquation, new Dictionary<string, object>()
            {
                { "uin", 11.0 },
                { "uout", 10.0},
                { "pin", 100.0 },
                { "pout",  20410.0 },
                { "hin", 10.0 },
                { "hout", 9.0 },
                { "rho", 1000.0 }
            });
        }

        [TestMethod]
        public void PitotTube()
        {
            RunFluidsSolveTest(LibraryResources.PitotTube, new Dictionary<string, object>()
            {
                { "u", 13.727 },
                { "h", 10.0},
                { "c", 0.98 }
            });
        }
    }
}
