using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Component;
using EngineeringMath;
using EngineeringMath.Resources;
using EngineeringMath.Resources.PVTTables;
using System.Reflection;

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

            Function fun = new Function("Test Function", "My Cat", true)
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






        /// <summary>
        /// Runs the same parameter values for each setting to make sure the function is solving correctly 
        /// </summary>
        /// <param name="paramValues">where the key is the display name of the parameter and the double is its value</param>
        /// <param name="funCat"></param>
        /// <param name="funName"></param>
        public static void RunFunctionSolveTest(string funCat, string funName, Dictionary<string, object> paramValues)
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
                return;
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

        private static void CheckFunctionWithCurrentSettings(Function fun, Dictionary<string, object> paramValues)
        {

            foreach (IParameter para in fun.AllParameters)
            {
                if(paramValues.ContainsKey(para.VarName))
                    SetParameter(para, paramValues[para.VarName]);
            }
            fun.Solve.Execute(null);
            foreach (IParameter para in fun.AllParameters)
            {
                if (paramValues.ContainsKey(para.VarName))
                    CheckParameter(para, paramValues[para.VarName]);
            }
        }

        private static void SetParameter(IParameter para, object obj)
        {
            if (para is INumericParameter numPara && obj is double num)
            {
                if (para.CurrentState != ParameterState.Output)
                {
                    numPara.BaseValue = num;
                }
                else
                {
                    numPara.BaseValue = double.NaN;
                }
                return;
            }

            try
            {
                PropertyInfo property = para.GetType().GetProperty(nameof(PickerParameter<string>.ItemAtSelectedIndex));
                if (para.CurrentState != ParameterState.Output)
                {
                    property.SetValue(para, obj);
                }
                else
                {
                    property.SetValue(para, null);
                }
                return;
            }
            catch
            {
                throw new Exception($"Parameter {para.DisplayName} type, {para.GetType()}, " +
                    $"does not match with the corresponding paramValue type {obj.GetType()}");
            }

        }
        private static void CheckParameter(IParameter para, object obj)
        {
            if (para is INumericParameter numPara && obj is double num)
            {
                AssertFractionDifference(num, numPara.BaseValue, 1e-3, numPara.DisplayName);
                return;
            }

            try
            {
                PropertyInfo property = para.GetType().GetProperty(nameof(PickerParameter<string>.ItemAtSelectedIndex));
                Assert.AreEqual(obj, property.GetValue(para));                
                return;
            }
            catch(AssertFailedException e)
            {
                throw e;
            }
            catch
            {
                throw new Exception($"Parameter {para.DisplayName} type, {para.GetType()}, " +
                    $"does not match with the corresponding paramValue type {obj.GetType()}");
            }
        }
        private static void AssertFractionDifference(double expected, double actual, double maxFracErr, string msg)
        {
            double delta = expected * maxFracErr;
            Assert.AreEqual(expected, actual, delta, msg);                
        }
    }
}
