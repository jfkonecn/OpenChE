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
            string siArea = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Area).GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
            siLength = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Length).GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI);
            SIUnitParameter areaPara = new SIUnitParameter("a", LibraryResources.Area)
            {
            },
            lenPara = new SIUnitParameter("r", LibraryResources.Length)
            {
            };            
            lenPara.ParameterUnits.ItemAtSelectedIndex = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Length).GetUnitByFullName(siLength);
            areaPara.ParameterUnits.ItemAtSelectedIndex = MathManager.AllUnits.GetUnitCategoryByName(LibraryResources.Area).GetUnitByFullName(siArea);
            lenPara.BindValue = 10;

            Function fun = new Function("Test Function", "Thermo")
            {
                Children =
                {
                    new FunctionQueueNode("Hello")
                    {
                        Children =
                        {
                            new PriorityFunctionBranch(
                                "Hello", 1,
                                new FunctionLeaf("Hello",$"$r$ * $r$ * {Math.PI}", "a")
                                    {
                                        Parameters =
                                        {
                                            areaPara,
                                            lenPara
                                        }
                                    })
                            {
                                
                            }
                        }
                    }

                }
            };

            fun.Calculate();
            Assert.AreEqual(10.0 * 10.0 * Math.PI, areaPara.BindValue, 0.001);

            
        }

    }
}
