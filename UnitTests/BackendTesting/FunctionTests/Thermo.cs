using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendTesting.FunctionTests
{
    [TestClass]
    public class Thermo
    {
        private void RunThermoSolveTest(string funName, Dictionary<string, object> paramValues)
        {
            ComponentFunction.RunFunctionSolveTest(LibraryResources.Thermodynamics, funName, paramValues);
        }

        [TestMethod]
        public void SteamTable()
        {
            RunThermoSolveTest(LibraryResources.SteamTable, new Dictionary<string, object>()
            {
                { "region", Region.Liquid },
                { "satRegion", SaturationRegion.Liquid },
                { "xv", 0.0 },
                { "xl", 1.0 },
                { "xs", 0.0 },
                { "T", 393.361545936488 },
                { "P", 0.2e6 },
                { "Vs", 0.00106051840643552 },
                { "U", 504471.741847973 },
                { "H", 504683.84552926 },
                { "S", 1530.0982011075 },
                { "cv", 3666.99397284121 },
                { "cp", 4246.73524917536 },
                { "u", 1520.69128792808 },
                { "rho", 1 / 0.00106051840643552 }
            });
        }
    }
}
