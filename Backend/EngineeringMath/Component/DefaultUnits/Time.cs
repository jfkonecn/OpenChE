using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Time : UnitCategory
    {
        public Time() : base(LibraryResources.Time, false)
        {
            Children.AddRange(new Unit[]
                    {
                        new Unit(LibraryResources.Seconds, LibraryResources.SecondsAbbrev, UnitSystem.ImperialAndMetric.SI_USCS),
                        new Unit(LibraryResources.Minutes, LibraryResources.MinutesAbbrev, 60.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Hours, LibraryResources.HoursAbbrev, 3600.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Milliseconds, LibraryResources.MillisecondsAbbrev, 1e-3, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Days, LibraryResources.DaysAbbrev, (3600.0*24), UnitSystem.ImperialAndMetric.BaselineSystem)
                    });
        }
    }
}
