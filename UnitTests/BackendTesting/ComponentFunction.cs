using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Component;
using EngineeringMath;
using EngineeringMath.Resources;


namespace BackendTesting
{
    [TestClass]
    public class ComponentFunction
    {
        [TestMethod]
        public void CustomFunction()
        {
            Function fun = new Function("Test Function", "Thermo")
            {
                Children =
                {
                    new FunctionLeaf("Hello",$"$r$ * $r$ * {Math.PI}", "a")
                    {
                        Parameters =
                        {
                            new SIUnitParameter("a", LibraryResources.Area),
                            new SIUnitParameter("r", LibraryResources.Length)
                        }
                    }
                }
            };
            
        }

    }
}
