using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential
{
    /// <summary>
    /// Function which represents P inlet - P outlet
    /// </summary>
    public class DeltaP : InletMinusOutput
    {
        public DeltaP() : base(new AbstractUnit[] { Pressure.Pa }, InletMinusOutput.InletMinusOutlet)
        {
        }

    }
}
