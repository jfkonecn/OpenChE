using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;
using ReplaceableParaBld = EngineeringMath.Component.Builder.ReplaceableParameterBuilder;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class OrificePlate : Function
    {
        public OrificePlate() : base(LibraryResources.OrificePlate, LibraryResources.FluidDynamics, false)
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
            };
        }
    }
}
