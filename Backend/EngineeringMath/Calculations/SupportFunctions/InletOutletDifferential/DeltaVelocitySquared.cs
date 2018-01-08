using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential
{
    /// <summary>
    /// 
    /// </summary>
    class DeltaVelocitySquared : InletMinusOutput
    {
        DeltaVelocitySquared() : base(new AbstractUnit[] { Length.m, Time.sec }, InletMinusOutput.InletSqMinusOutletSq)
        {

        }
    }
}
