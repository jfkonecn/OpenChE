using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class VolumetricFlowRate : UnitCategory
    {
        public VolumetricFlowRate() : base(LibraryResources.VolumetricFlowRate,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Volume(), 1, false),
            new UnitCategoryElement(new Time(), -1, false)
        }, false)
        {

        }
    }
}
