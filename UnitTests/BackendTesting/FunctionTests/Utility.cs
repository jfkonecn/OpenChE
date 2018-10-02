using EngineeringMath.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackendTesting.FunctionTests
{
    [TestClass]
    public class Utility
    {
        private void RunUtilitySolveTest(string funName, Dictionary<string, object> paramValues)
        {
            ComponentFunction.RunFunctionSolveTest(LibraryResources.UnitConverter, funName, paramValues);
        }

        [TestMethod]
        public void UnitConverter()
        {
            
        }
    }
}
