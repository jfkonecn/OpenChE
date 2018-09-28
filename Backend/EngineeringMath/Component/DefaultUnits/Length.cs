using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Length : UnitCategory
    {
        public Length() : base(LibraryResources.Length, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Meters, "m", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.Feet, "ft", 1 / 3.28084, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Inches, "in", 1 / 39.3701, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Miles, "mi", 1609.34, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Millimeters, "mm", 1e-3, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Centimeters, "cm", 1e-2, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilometers, "km", 1000, UnitSystem.Metric.BaselineSystem)
                    });
        }
    }
}
