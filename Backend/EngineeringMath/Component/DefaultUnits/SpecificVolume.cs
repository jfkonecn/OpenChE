using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class SpecificVolume : UnitCategory
    {
        public SpecificVolume() : base(LibraryResources.SpecificVolume,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Volume(), 1, false),
            new UnitCategoryElement(new Mass(), -1, false)
        }, false)
        {

        }
    }
}
