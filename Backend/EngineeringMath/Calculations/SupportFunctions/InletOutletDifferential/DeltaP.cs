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
        public DeltaP() : base(InletMinusOutput.InletMinusOutlet)
        {
        }

        private readonly AbstractUnit[] _DefaultUnit = new AbstractUnit[] { Pressure.Pa };
        protected override AbstractUnit[] DefaultUnit
        {
            get
            {
                return _DefaultUnit;
            }
        }
    }
}
