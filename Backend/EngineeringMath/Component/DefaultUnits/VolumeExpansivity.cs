using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class VolumeExpansivity : UnitCategory
    {
        public VolumeExpansivity() : base(LibraryResources.VolumeExpansivity,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Temperature(), -1, false)
        }, false)
        {

        }
    }
}
