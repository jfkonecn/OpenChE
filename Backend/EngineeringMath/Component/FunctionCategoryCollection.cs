using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Resources;
using System.Linq;
using ReplaceableParaBld = EngineeringMath.Component.Builder.ReplaceableParameterBuilder;

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
                return _AllFunctions;
            }
        }

        internal static void BuildAllFunctions()
        {
            _AllFunctions = new FunctionCategoryCollection(LibraryResources.AllFunctions)
            {
                new FunctionCategory(LibraryResources.FluidDynamics)
                {
                    new Function(LibraryResources.OrificePlate)
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
                    }
                }
            };
        }
    }
}
