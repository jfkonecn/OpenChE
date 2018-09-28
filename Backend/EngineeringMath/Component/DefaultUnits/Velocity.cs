using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Velocity : UnitCategory
    {
        public Velocity() : base(LibraryResources.Velocity, 
            new UnitCategoryElement[] 
            {
                new UnitCategoryElement(new Length(), 1, false),
                new UnitCategoryElement(new Time(), -1, false)
            }, false)
        {
            
        }
    }
}
