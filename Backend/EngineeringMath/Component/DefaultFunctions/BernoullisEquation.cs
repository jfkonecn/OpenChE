using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class BernoullisEquation : Function
    {
        public BernoullisEquation() : base(LibraryResources.BernoullisEquation, LibraryResources.FluidDynamics, false)
        {
            NextNode = new FunctionBranch(LibraryResources.ChangeOutputs)
            {
                Parameters =
                            {
                                new SIUnitParameter(string.Format(LibraryResources.InletBlank, LibraryResources.Velocity),
                                "uin", LibraryResources.Velocity, minSIValue:0),
                                new SIUnitParameter(string.Format(LibraryResources.OutletBlank, LibraryResources.Velocity),
                                "uout", LibraryResources.Velocity, minSIValue:0),
                                new SIUnitParameter(string.Format(LibraryResources.InletBlank, LibraryResources.Pressure),
                                "pin", LibraryResources.Pressure, minSIValue:0),
                                new SIUnitParameter(string.Format(LibraryResources.OutletBlank, LibraryResources.Pressure),
                                "pout", LibraryResources.Pressure, minSIValue:0),
                                new SIUnitParameter(string.Format(LibraryResources.InletBlank, LibraryResources.Height),
                                "hin", LibraryResources.Length, minSIValue:0),
                                new SIUnitParameter(string.Format(LibraryResources.OutletBlank, LibraryResources.Height),
                                "hout", LibraryResources.Length, minSIValue:0),
                                new SIUnitParameter(LibraryResources.Density,
                                "rho", LibraryResources.Density, minSIValue:0)
                            },
                Children =
                            {
                                new FunctionLeaf("$rho * ($pin / $rho + 9.81 * ($hin - $hout) + ($uin ^ 2 - $uout ^ 2) / 2)", "pout")
                            }
            };
        }
    }
}
