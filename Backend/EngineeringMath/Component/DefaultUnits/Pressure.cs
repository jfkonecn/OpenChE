using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Pressure : UnitCategory
    {
        public Pressure() : base(LibraryResources.Pressure, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Pascals, "Pa", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.PoundsForcePerSqIn, "psi", 6894.76, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Atmospheres, "atm", 101325, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Bar, "bar", 1e5, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilopascals, "kPa", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Torr, "torr", 133.322, UnitSystem.Metric.BaselineSystem)
                    });
        }
    }
}
