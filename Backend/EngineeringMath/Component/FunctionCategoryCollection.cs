using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Resources;
using System.Linq;
using ReplaceableParaBld = EngineeringMath.Component.Builder.ReplaceableParameterBuilder;
using EngineeringMath.Component.Builder;
using System.Threading;
using System.Diagnostics;

namespace EngineeringMath.Component
{
    public class FunctionCategoryCollection : CategoryCollection<Function>
    {
        protected FunctionCategoryCollection(string name) : base(name)
        {

        }






        public new FunctionCategory GetCategoryByName(string catName)
        {
            return (FunctionCategory)base.GetCategoryByName(catName);

        }




        private static FunctionCategoryCollection _AllFunctions = null;
        internal static FunctionCategoryCollection AllFunctions
        {
            get
            {
                if (_AllFunctions == null)
                {
                    BuildAllFunctions();
                }
                lock (LockAllFunctions)
                {
                    return _AllFunctions;
                }                
            }
        }

        private static readonly object LockAllFunctions = new object();

        private static Thread CreateFunctionBuilderThread(string catName, string funName, Func<Function> buildFunction)
        {
            Thread thread = new Thread(() =>
            {
                Function function = buildFunction();
                lock (LockAllFunctions)
                {
                    Category<Function> funCat = _AllFunctions.Children.SingleOrDefault((x) => x.Name == catName);
                    if (funCat == null)
                    {
                        funCat = new Category<Function>(catName, false);
                        _AllFunctions.Add(funCat);
                    }
                    funCat.Add(function);
                }
            })
            {
                Name = funName
            };
            thread.Start();
            return thread;
        }

        private static Thread CreateOrificePlateFunction()
        {
            return CreateFunctionBuilderThread(
                LibraryResources.FluidDynamics, 
                LibraryResources.OrificePlate, 
                () => 
                {
                    return new Function(LibraryResources.OrificePlate)
                    {
                        NextNode = new FunctionBranch(LibraryResources.ChangeOutputs)
                        {
                            Parameters =
                            {
                                new UnitlessParameter(LibraryResources.DischargeCoefficient, "dc", 0, 1),
                                new SIUnitParameter(LibraryResources.Density, "rho", LibraryResources.Density, minSIValue:0),
                                ReplaceableParaBld.AreaParameter(new SIUnitParameter(LibraryResources.InletPipeArea, "pArea", LibraryResources.Area, minSIValue:0)),
                                ReplaceableParaBld.AreaParameter(new SIUnitParameter(LibraryResources.OrificeArea, "oArea", LibraryResources.Area, minSIValue:0)),
                                new SIUnitParameter(LibraryResources.PressureDrop, "deltaP", LibraryResources.Pressure, minSIValue:0),
                                new SIUnitParameter(LibraryResources.VolumetricFlowRate, "Q", LibraryResources.VolumetricFlowRate, minSIValue:0)
                            },
                            Children =
                            {
                                new FunctionLeaf("$Q / ($pArea * Sqrt((2 * $deltaP) / ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1))))", "dc"),
                                new FunctionLeaf("(2 * $deltaP) / ((($Q  / ($dc * $pArea)) ^ 2) * ($pArea ^ 2 / $oArea ^ 2 - 1))", "rho"),
                                new FunctionLeaf("Sqrt(1 / ((1 / $oArea ^ 2) - ((2 * $deltaP * $dc ^ 2) / ($Q ^ 2 * $rho))))", "pArea"),
                                new FunctionLeaf("Sqrt(1 / ((1 / $pArea ^ 2) + ((2 * $deltaP * $dc ^ 2) / ($Q ^ 2 * $rho))))", "oArea"),
                                new FunctionLeaf("(($Q / ($dc * $pArea)) ^ 2 * ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1))) / 2", "deltaP"),
                                new FunctionLeaf("$dc * $pArea * Sqrt((2 * $deltaP) / ($rho * ($pArea ^ 2 / $oArea ^ 2 - 1)))", "Q")
                            }
                        }
                    };
                });
        }

        private static Thread CreateSteamTableFunction()
        {
            return CreateFunctionBuilderThread(
                LibraryResources.Thermodynamics, 
                LibraryResources.SteamTable,
                () =>
                {
                    VisitableNodeDirector dir = new VisitableNodeDirector()
                    {
                        NodeBuilder = PVTTableNodeBuilder.SteamTableBuilder()
                    };
                    dir.BuildNode(LibraryResources.SteamTable);
                    return new Function(LibraryResources.SteamTable)
                    {
                        NextNode = dir.Node
                    };
                });
        }

        internal static void BuildAllFunctions()
        {
            _AllFunctions = new FunctionCategoryCollection(LibraryResources.AllFunctions);
            List<Thread> threads = new List<Thread>
            {
                CreateSteamTableFunction(),
                CreateOrificePlateFunction()
            };
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
            // update the search results
            _AllFunctions.SearchKeyword = string.Empty;
            _AllFunctions.Search.Execute(null);
        }
    }
}
