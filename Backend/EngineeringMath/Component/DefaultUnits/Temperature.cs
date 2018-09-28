using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Temperature : UnitCategory
    {
        public Temperature() : base(LibraryResources.Temperature, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Kelvin, "K", 1.8, UnitSystem.Metric.SI),
                        new Unit(LibraryResources.Rankine, "°R", UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Fahrenheit, "°F",$"${Unit.CurUnitVar} + 459.67",
                        $"${Unit.BaseUnitVar} - 459.67", UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Celsius, "°C",$"(${Unit.CurUnitVar} + 273.15) * 9/5",
                        $"${Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.Imperial.BaselineSystem)
                    });
        }
    }
}
