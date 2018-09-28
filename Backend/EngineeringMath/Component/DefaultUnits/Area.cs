using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Area : UnitCategory
    {
        public Area() : base(LibraryResources.Area,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Length(), 2, false)
        }, false)
        {

        }
    }
}
