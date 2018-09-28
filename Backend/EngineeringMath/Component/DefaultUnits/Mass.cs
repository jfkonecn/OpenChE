using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Mass : UnitCategory
    {
        public Mass() : base(LibraryResources.Mass, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Kilograms, "kg", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.PoundsMass, "lbs\u2098", 1/2.20462, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Grams, "g", 1e-3, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Milligrams, "mg", 1e-6, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Micrograms, "\u03BCg", 1e-9, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.MetricTons, "Mg", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Ounces, "oz", 1/35.274, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.USTons, "Ton", 907.185, UnitSystem.Imperial.BaselineSystem)
                    });
        }
    }
}
