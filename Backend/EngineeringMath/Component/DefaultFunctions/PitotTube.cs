using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultFunctions
{
    public class PitotTube : Function
    {
        public PitotTube() : base(LibraryResources.PitotTube, LibraryResources.FluidDynamics, false)
        {
            NextNode = new FunctionLeaf("$c * Sqrt(2 * 9.81 * $h)", "u")
            {
                Parameters =
                            {
                                new SIUnitParameter(LibraryResources.Velocity, "u", LibraryResources.Velocity, minSIValue:0),
                                new SIUnitParameter(LibraryResources.PressureHead, "h", LibraryResources.Length, minSIValue:0),
                                new UnitlessParameter(LibraryResources.CorrectionCoefficient, "c", minValue: 0)
                            }
            };
        }
    }
}
