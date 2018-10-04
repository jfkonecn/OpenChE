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
                { "steamTable_region", Region.Liquid },
                { "steamTable_satRegion", SaturationRegion.Liquid },
                { "steamTable_xv", 0.0 },
                { "steamTable_xl", 1.0 },
                { "steamTable_xs", 0.0 },
                { "steamTable_T", 393.361545936488 },
                { "steamTable_P", 0.2e6 },
                { "steamTable_Vs", 0.00106051840643552 },
                { "steamTable_U", 504471.741847973 },
                { "steamTable_H", 504683.84552926 },
                { "steamTable_S", 1530.0982011075 },
                { "steamTable_cv", 3666.99397284121 },
                { "steamTable_cp", 4246.73524917536 },
                { "steamTable_u", 1520.69128792808 },
                { "steamTable_rho", 1 / 0.00106051840643552 }
            });
        }
    }
}
