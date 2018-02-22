using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Thermo.Cycles
{
    public class AllCycles : FunctionSubber
    {
        public AllCycles() : base(new Dictionary<string, Type> {
            { LibraryResources.RankineCycle, typeof(RankineCycle) },
            { LibraryResources.RegenerativeCycle, typeof(RegenerativeCycle) }
        })
        {
            this.Title = LibraryResources.ThermodynamicCycle;
        }
    }
}
