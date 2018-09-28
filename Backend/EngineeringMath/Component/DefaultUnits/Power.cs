using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Power : UnitCategory
    {
        public Power() : base(LibraryResources.Power, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Watt, "W", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.Horsepower, "Hp", 745.7, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Kilowatt, "kW", 1000, UnitSystem.Metric.BaselineSystem)
                    });
        }
    }
}
