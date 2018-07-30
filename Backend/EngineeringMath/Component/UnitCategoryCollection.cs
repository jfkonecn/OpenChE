using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class UnitCategoryCollection : NotifyPropertyList<UnitCategory>
    {
        private UnitCategoryCollection()
        {

        }

        internal static readonly UnitCategoryCollection AllUnits = new UnitCategoryCollection()
        {
            new UnitCategory(LibraryResources.Temperature)
            {
                new Unit(LibraryResources.Kelvin, "°K","curUnit * 9/5","baseUnit * 5/9", UnitSystem.UnitSystemType.SI),
                new Unit(LibraryResources.Rankine,"°R","","", UnitSystem.UnitSystemType.USCS, isBaseUnit: true),
                new Unit(LibraryResources.Fahrenheit, "°F","curUnit + 459.67", "baseUnit - 459.67", UnitSystem.UnitSystemType.None),
                new Unit(LibraryResources.Celsius, "°C","(curUnit + 273.15) * 9/5", "baseUnit * 5/9 - 273.15", UnitSystem.UnitSystemType.None)
            },
            new UnitCategory(LibraryResources.Area)
            {
                new Unit(LibraryResources.MetersSquared,"m\xB2","","", UnitSystem.UnitSystemType.SI, isBaseUnit: true),
                new Unit(LibraryResources.FeetSquared, "ft\xB2","curUnit / (3.28084 * 3.28084)","baseUnit * 3.28084 * 3.28084", UnitSystem.UnitSystemType.USCS)
            }
        };
    }
}
