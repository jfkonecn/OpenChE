using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Energy : UnitCategory
    {
        public Energy() : base(LibraryResources.Energy, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Joules, "J", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.BTU, "BTU", 1055.06, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Kilocalories, "kCal", 4184, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilojoules, "kJ", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Therms, "Therms", 1.055e+8, UnitSystem.Imperial.BaselineSystem)
                    });
        }
    }
}
