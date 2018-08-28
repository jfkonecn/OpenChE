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
            string siArea = MathManager.AllUnits.GetCategoryByName(LibraryResources.Area).GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI),
            siLength = MathManager.AllUnits.GetCategoryByName(LibraryResources.Length).GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI);
            SIUnitParameter areaPara = new SIUnitParameter("x", "a", LibraryResources.Area)
            {
            },
            lenPara = new SIUnitParameter("y", "r", LibraryResources.Length)
            {
            };            
            lenPara.ParameterUnits.ItemAtSelectedIndex = MathManager.AllUnits.GetItemByFullName(LibraryResources.Length, siLength);
            areaPara.ParameterUnits.ItemAtSelectedIndex = MathManager.AllUnits.GetItemByFullName(LibraryResources.Area, siArea);
            lenPara.BindValue = 10;

            Function fun = new Function("Test Function")
            {
                NextNode =
                new FunctionQueueNode("Hello")
                {
                    Children =
                    {
                        new PriorityFunctionBranch(
                            "Hello", 1,
                            new FunctionLeaf($"$r * $r * {Math.PI}", "a")
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
            };

            fun.Calculate();
            Assert.AreEqual(10.0 * 10.0 * Math.PI, areaPara.BindValue, 0.001);

            
        }

        [TestMethod]
        public void OrificePlate()
        {
            RunFunctionSolveTest(LibraryResources.FluidDynamics, LibraryResources.OrificePlate, new Dictionary<string, double>()
            {
                { LibraryResources.DischargeCoefficient, 0.7 },
                { LibraryResources.Density, 1000 },
                { LibraryResources.InletPipeArea, 10 * 10 * Math.PI / 4.0 },
                { LibraryResources.OrificeArea,  8 * 8 * Math.PI / 4.0 },
                { LibraryResources.PressureDrop, 10 },
                { LibraryResources.VolumetricFlowRate, 6.476 }
            });
        }

        /// <summary>
        /// Runs the same parameter values for each setting to make sure the function is solving correctly 
        /// </summary>
        /// <param name="paramValues">where the key is the display name of the parameter and the double is its value</param>
        /// <param name="funCat"></param>
        /// <param name="funName"></param>
        private void RunFunctionSolveTest(string funCat, string funName, Dictionary<string, double> paramValues)
        {
            Function fun;
            if (MathManager.AllFunctions.TryGetValue(funCat, out Category<Function> cat))
            {
                if (!cat.TryGetValue(funName, out fun))
                    throw new Exception($"{funName} not found");
            }
            else
            {
                throw new Exception($"{funCat} not found");
            }

            Stack<ISetting> preStack = new Stack<ISetting>();
            Stack<ISetting> postStack = new Stack<ISetting>();

            for (int i = 0; i < fun.AllSettings.Count; i++)
            {
                fun.AllSettings[i].SelectedIndex = 0;
                preStack.Push(fun.AllSettings[i]);
            }

            if(preStack.Count == 0)
            {
                CheckFunctionWithCurrentSettings(fun, paramValues);
            }


            while (true)
            {

                CheckFunctionWithCurrentSettings(fun, paramValues);


                ISetting curSetting = preStack.Pop();
                postStack.Push(curSetting);

                while (curSetting.SelectedIndex == curSetting.AllOptions.Count - 1 || curSetting.CurrentState == SettingState.Inactive)
                {
                    if (curSetting.CurrentState == SettingState.Active)
                        curSetting.SelectedIndex = 0;

                    if (!preStack.TryPop(out curSetting))
                    {
                        return;
                    }
                    postStack.Push(curSetting);
                    if (curSetting.CurrentState == SettingState.Active)
                        curSetting.SelectedIndex++;
                }
                while (postStack.Count > 0)
                {
                    preStack.Push(postStack.Pop());
                }


                preStack.Peek().SelectedIndex++;
            }
        }

        private void CheckFunctionWithCurrentSettings(Function fun, Dictionary<string, double> paramValues)
        {
            bool paramsChecked = false;
            
            foreach (IParameter para in fun.InputParameters)
            {
                para.BaseValue = paramValues[para.DisplayName];
            }
            foreach(IParameter para in fun.OutputParameters)
            {
                para.BaseValue = double.NaN;
            }

            fun.Solve.Execute(null);
            foreach (IParameter para in fun.InputParameters)
            {
                Assert.AreEqual(paramValues[para.DisplayName], para.BaseValue, 1e-3);
                paramsChecked = true;
            }
            foreach (IParameter para in fun.OutputParameters)
            {
                Assert.AreEqual(paramValues[para.DisplayName], para.BaseValue, 1e-1, $"Output: {para.DisplayName}");
                paramsChecked = true;
            }
            if (!paramsChecked)
                Assert.Fail();
        }
    }
}
