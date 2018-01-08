using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential
{
    /// <summary>
    /// Function which represents inlet length - outlet length
    /// </summary>
    public class DeltaLength : InletMinusOutput
    {
        public DeltaLength() : base(new AbstractUnit[] { Length.m }, InletMinusOutlet) { }
    }
}
