using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class UnitCategoryCollection : NotifyPropertySortedList<string, UnitCategory>
    {
        private UnitCategoryCollection()
        {

        }


        public UnitCategory GetUnitCategoryByName(string name)
        {
            IEnumerable<UnitCategory> temp = from cat in AllUnits
                                       where cat.Name.Equals(name)
                                       select cat;
            if (temp.Count() == 0)
            {
                throw new UnitCategoryNotFoundException(name);
            }
            else if (temp.Count() > 1)
            {
                throw new UnitCategoriesWithSameNameException(name);
            }
            return temp.ElementAt(0);
        }

        // https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts
        private static readonly string 
            KelvinSymbol = "K",
            RankineSymbol = "°R",
            FahrenheitSymbol = "°F",
            CelsiusSymbol = "°C",
            MetersSymbol = "m",
            FeetSymbol = "ft",
            KilogramSymbol = "kg",
            PoundsMassSymbol = "lbs\u2098",
            JoulesSymbol = "J",
            KilojoulesSymbol = "kJ",
            BTUSymbol = "BTU",
            Kilocalories = "kCal",
            // the parameter string used in the equation string for converting to base unit
            CurUnit = "curUnit",
            // the parameter string used in the equation string for converting from base unit 
            BaseUnit = "baseUnit";


        internal static readonly UnitCategoryCollection AllUnits = new UnitCategoryCollection()
        {
            new UnitCategory(LibraryResources.Temperature)
            {
                new Unit(LibraryResources.Kelvin, KelvinSymbol,$"{CurUnit} * 9/5",$"{BaseUnit} * 5/9", UnitSystem.UnitSystemBaseUnit.SI),
                new Unit(LibraryResources.Rankine, RankineSymbol,"","", UnitSystem.UnitSystemBaseUnit.USCS, isBaseUnit: true),
                new Unit(LibraryResources.Fahrenheit, FahrenheitSymbol,$"{CurUnit} + 459.67", $"{BaseUnit} - 459.67", UnitSystem.UnitSystemBaseUnit.None),
                new Unit(LibraryResources.Celsius, CelsiusSymbol,$"({CurUnit} + 273.15) * 9/5", $"{BaseUnit} * 5/9 - 273.15", UnitSystem.UnitSystemBaseUnit.None)
            },
            new UnitCategory(LibraryResources.Area)
            {
                new Unit(LibraryResources.MetersSquared,$"{MetersSymbol}{SquaredSymbol}",
                    "","", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                new Unit(LibraryResources.FeetSquared, $"{FeetSymbol}{SquaredSymbol}",
                    $"{CurUnit} / (3.28084 * 3.28084)",$"{BaseUnit} * 3.28084 * 3.28084", UnitSystem.UnitSystemBaseUnit.USCS)
            },
            new UnitCategory(LibraryResources.Density)
            {
                new Unit(LibraryResources.KgPerMeterCubed, $"{KilogramSymbol}*{MetersSymbol}{NegativeSuperScript}{CubedSymbol}",
                    "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                new Unit(LibraryResources.LbsmPerFeetCubed, $"{PoundsMassSymbol}*{FeetSymbol}{NegativeSuperScript}{CubedSymbol}",
                    $"{CurUnit} * 16.0185", $"{BaseUnit} / 16.0185", UnitSystem.UnitSystemBaseUnit.USCS)
            },
            new UnitCategory(LibraryResources.Energy)
            {
                new Unit(LibraryResources.Joules, $"{JoulesSymbol}", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                new Unit(LibraryResources.BTU, $"{BTUSymbol}", $"{CurUnit} * 1055.06", $"{BaseUnit} / 1055.06", UnitSystem.UnitSystemBaseUnit.USCS),
                new Unit(LibraryResources.Kilocalories, $"{Kilocalories}", $"{CurUnit} * 4184", $"{BaseUnit} / 4184", UnitSystem.UnitSystemBaseUnit.None),
                new Unit(LibraryResources.Kilojoules, $"{KilojoulesSymbol}", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None),
                new Unit(LibraryResources.Therms, "Therms", $"{CurUnit} * 1.055e+8", $"{BaseUnit} / 1.055e+8", UnitSystem.UnitSystemBaseUnit.None)
            },
            new UnitCategory(LibraryResources.Enthalpy, new UnitCategory.CompositeUnitElement[]
                {
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Energy,
                        IsInverse = false,
                        power = UnitCategory.ToPowerOf.One
                    },
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Mass,
                        IsInverse = true,
                        power = UnitCategory.ToPowerOf.One
                    }
                })
            {
            }
        };

        public class UnitCategoryNotFoundException : ArgumentException
        {
            public UnitCategoryNotFoundException(string unitCatName) : base(unitCatName) { }
        }
        public class UnitCategoriesWithSameNameException : ArgumentException
        {
            public UnitCategoriesWithSameNameException(string unitCatName) : base(unitCatName) { }
        }
    }
}
