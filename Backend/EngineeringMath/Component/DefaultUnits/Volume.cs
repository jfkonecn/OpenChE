using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Volume : UnitCategory
    {
        public Volume() : base(LibraryResources.Volume,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Length(), 3, false)
        }, false)
        {
            Children.AddRange(new Unit[]
            {
                new Unit(LibraryResources.Gallons, "gal", 1 / 264.172, UnitSystem.Imperial.BaselineSystem),
                new Unit(LibraryResources.Liters, "l", 1e-3, UnitSystem.Metric.BaselineSystem),
                new Unit(LibraryResources.Milliliters, "ml", 1e-6, UnitSystem.Metric.BaselineSystem)
            });
        }
    }
}
