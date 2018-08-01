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

        
        private static readonly string 
            // the parameter string used in the equation string for converting to base unit
            CurUnit = "curUnit",
            // the parameter string used in the equation string for converting from base unit 
            BaseUnit = "baseUnit";

        // https://en.wikipedia.org/wiki/Unicode_subscripts_and_superscripts

        private static UnitCategoryCollection _AllUnits = null;
        internal static UnitCategoryCollection AllUnits
        {
            get
            {
                if(_AllUnits == null)
                {
                    BuildAllUnits();
                }
                return _AllUnits;
            }
        }

        internal static void BuildAllUnits()
        {
            _AllUnits = new UnitCategoryCollection()
            {
                new UnitCategory(LibraryResources.Temperature)
                {
                    new Unit(LibraryResources.Kelvin, "K",$"{CurUnit} * 9/5",$"{BaseUnit} * 5/9", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.Rankine, "°R","","", UnitSystem.UnitSystemBaseUnit.USCS, isBaseUnit: true),
                    new Unit(LibraryResources.Fahrenheit, "°F",$"{CurUnit} + 459.67", $"{BaseUnit} - 459.67", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Celsius, "°C",$"({CurUnit} + 273.15) * 9/5", $"{BaseUnit} * 5/9 - 273.15", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Energy)
                {
                    new Unit(LibraryResources.Joules, "J", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                    new Unit(LibraryResources.BTU, "BTU", $"{CurUnit} * 1055.06", $"{BaseUnit} / 1055.06", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Kilocalories, "kCal", $"{CurUnit} * 4184", $"{BaseUnit} / 4184", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilojoules, "kJ", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Therms, "Therms", $"{CurUnit} * 1.055e+8", $"{BaseUnit} / 1.055e+8", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Length)
                {
                    new Unit(LibraryResources.Meters, "m", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                    new Unit(LibraryResources.Feet, "ft", $"{CurUnit} / 3.28084", $"{BaseUnit} * 3.28084", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Inches, "in", $"{CurUnit} / 39.3701", $"{BaseUnit} * 39.3701", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Miles, "mi", $"{CurUnit} * 1609.34", $"{BaseUnit} / 1609.34", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Millimeters, "mm", $"{CurUnit} / 1000", $"{BaseUnit} * 1000", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Centimeters, "cm", $"{CurUnit} / 100", $"{BaseUnit} * 100", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilometers, "km", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Mass)
                {
                    new Unit(LibraryResources.Kilograms, "kg", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                    new Unit(LibraryResources.PoundsMass, "lbs\u2098", $"{CurUnit} / 2.20462", $"{BaseUnit} * 2.20462", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Grams, "g", $"{CurUnit} / 1000", $"{BaseUnit} * 1000", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Milligrams, "mg", $"{CurUnit} / 1e6", $"{BaseUnit} * 1e6", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Micrograms, "\u03BCg", $"{CurUnit} / 1e9", $"{BaseUnit} * 1e9", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.MetricTons, "Mg", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Ounces, "oz", $"{CurUnit} / 35.274", $"{BaseUnit} * 35.274", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.USTons, "Ton", $"{CurUnit} * 907.185", $"{BaseUnit} / 907.185", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Power)
                {
                    new Unit(LibraryResources.Watt, "W", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                    new Unit(LibraryResources.Horsepower, "Hp", $"{CurUnit} * 745.7", $"{BaseUnit} / 745.7", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Kilowatt, "kW", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Pressure)
                {
                    new Unit(LibraryResources.Pascals, "Pa", "", "", UnitSystem.UnitSystemBaseUnit.SI, isBaseUnit: true),
                    new Unit(LibraryResources.PoundsForcePerSqIn, "psi", $"{CurUnit} * 6894.76", $"{BaseUnit} / 6894.76", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Atmospheres, "atm", $"{CurUnit} * 101325", $"{BaseUnit} / 101325", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Bar, "bar", $"{CurUnit} * 1e5", $"{BaseUnit} / 1e5", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilopascals, "kPa", $"{CurUnit} * 1000", $"{BaseUnit} / 1000", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Torr, "torr", $"{CurUnit} * 133.322", $"{BaseUnit} / 133.322", UnitSystem.UnitSystemBaseUnit.None)
                }
            };
            _AllUnits.Add(new UnitCategory(LibraryResources.Density, new UnitCategory.CompositeUnitElement[]
                    {
                        new UnitCategory.CompositeUnitElement()
                        {
                            CategoryName = LibraryResources.Mass,
                            IsInverse = false,
                            power = UnitCategory.ToPowerOf.One
                        },
                        new UnitCategory.CompositeUnitElement()
                        {
                            CategoryName = LibraryResources.Length,
                            IsInverse = true,
                            power = UnitCategory.ToPowerOf.Three
                        }
                    })
            {

            });

            _AllUnits.Add(new UnitCategory(LibraryResources.Enthalpy, new UnitCategory.CompositeUnitElement[]
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
            });

            _AllUnits.Add(new UnitCategory(LibraryResources.Entropy, new UnitCategory.CompositeUnitElement[]
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
                                },
                                new UnitCategory.CompositeUnitElement()
                                {
                                    CategoryName = LibraryResources.Temperature,
                                    IsInverse = true,
                                    power = UnitCategory.ToPowerOf.One
                                }
                })
            {
            });

            _AllUnits.Add(new UnitCategory(LibraryResources.Area, new UnitCategory.CompositeUnitElement[]
                {
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Length,
                        IsInverse = false,
                        power = UnitCategory.ToPowerOf.Two
                    }
                }));
        }

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
