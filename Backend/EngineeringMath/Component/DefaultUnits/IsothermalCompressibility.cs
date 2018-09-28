using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class IsothermalCompressibility : UnitCategory
    {
        public IsothermalCompressibility() : base(
            LibraryResources.IsothermalCompressibility,
            new UnitCategoryElement[]
            {
                new UnitCategoryElement(new Pressure(), -1, false)
            }, false)
        {

        }
    }
}
