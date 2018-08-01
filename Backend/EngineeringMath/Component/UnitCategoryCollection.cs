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
                    new Unit(LibraryResources.Kelvin, "K", 1.8, UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.Rankine, "°R", UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Fahrenheit, "°F",$"{Unit.CurUnitVar} + 459.67", $"{Unit.BaseUnitVar} - 459.67", UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Celsius, "°C",$"({Unit.CurUnitVar} + 273.15) * 9/5", $"{Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Energy)
                {
                    new Unit(LibraryResources.Joules, "J", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.BTU, "BTU", 1055.06, UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Kilocalories, "kCal", 4184, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilojoules, "kJ", 1000, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Therms, "Therms", 1.055e+8, UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Length)
                {
                    new Unit(LibraryResources.Meters, "m", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.Feet, "ft", 1 / 3.28084, UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Inches, "in", 1 / 39.3701, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Miles, "mi", 1609.34, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Millimeters, "mm", 1e-3, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Centimeters, "cm", 1e-2, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilometers, "km", 1000, UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Mass)
                {
                    new Unit(LibraryResources.Kilograms, "kg", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.PoundsMass, "lbs\u2098", 1/2.20462, UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Grams, "g", 1e-3, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Milligrams, "mg", 1e-6, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Micrograms, "\u03BCg", 1e-9, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.MetricTons, "Mg", 1000, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Ounces, "oz", 1/35.274, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.USTons, "Ton", 907.185, UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Power)
                {
                    new Unit(LibraryResources.Watt, "W", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.Horsepower, "Hp", 745.7, UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Kilowatt, "kW", 1000, UnitSystem.UnitSystemBaseUnit.None)
                },
                new UnitCategory(LibraryResources.Pressure)
                {
                    new Unit(LibraryResources.Pascals, "Pa", UnitSystem.UnitSystemBaseUnit.SI),
                    new Unit(LibraryResources.PoundsForcePerSqIn, "psi", 6894.76, UnitSystem.UnitSystemBaseUnit.USCS),
                    new Unit(LibraryResources.Atmospheres, "atm", 101325, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Bar, "bar", 1e5, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Kilopascals, "kPa", 1000, UnitSystem.UnitSystemBaseUnit.None),
                    new Unit(LibraryResources.Torr, "torr", 133.322, UnitSystem.UnitSystemBaseUnit.None)
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
