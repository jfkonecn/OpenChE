using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class UnitCategoryCollection : CategoryCollection<Unit>
    {
        protected UnitCategoryCollection(string name) : base(name)
        {
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

        public new UnitCategory GetCategoryByName(string catName)
        {
            return (UnitCategory)base.GetCategoryByName(catName);

        }

        internal static void BuildAllUnits()
        {
            _AllUnits = new UnitCategoryCollection(LibraryResources.AllUnits)
            {
                Children =
                {
                    new UnitCategory(LibraryResources.Temperature)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Kelvin, "K", 1.8, UnitSystem.Metric.SI),
                            new Unit(LibraryResources.Rankine, "°R", UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Fahrenheit, "°F",$"${Unit.CurUnitVar} + 459.67", 
                            $"${Unit.BaseUnitVar} - 459.67", UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Celsius, "°C",$"(${Unit.CurUnitVar} + 273.15) * 9/5", 
                            $"${Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(LibraryResources.Energy)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Joules, "J", UnitSystem.Metric.SI),
                            new Unit(LibraryResources.BTU, "BTU", 1055.06, UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Kilocalories, "kCal", 4184, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Kilojoules, "kJ", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Therms, "Therms", 1.055e+8, UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(LibraryResources.Length)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Meters, "m", UnitSystem.Metric.SI),
                            new Unit(LibraryResources.Feet, "ft", 1 / 3.28084, UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Inches, "in", 1 / 39.3701, UnitSystem.Imperial.BaselineSystem),
                            new Unit(LibraryResources.Miles, "mi", 1609.34, UnitSystem.Imperial.BaselineSystem),
                            new Unit(LibraryResources.Millimeters, "mm", 1e-3, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Centimeters, "cm", 1e-2, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Kilometers, "km", 1000, UnitSystem.Metric.BaselineSystem)
                        }
                    },
                    new UnitCategory(LibraryResources.Mass)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Kilograms, "kg", UnitSystem.Metric.SI),
                            new Unit(LibraryResources.PoundsMass, "lbs\u2098", 1/2.20462, UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Grams, "g", 1e-3, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Milligrams, "mg", 1e-6, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Micrograms, "\u03BCg", 1e-9, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.MetricTons, "Mg", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Ounces, "oz", 1/35.274, UnitSystem.Imperial.BaselineSystem),
                            new Unit(LibraryResources.USTons, "Ton", 907.185, UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(LibraryResources.Power)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Watt, "W", UnitSystem.Metric.SI),
                            new Unit(LibraryResources.Horsepower, "Hp", 745.7, UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Kilowatt, "kW", 1000, UnitSystem.Metric.BaselineSystem)
                        }
                    },
                    new UnitCategory(LibraryResources.Pressure)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Pascals, "Pa", UnitSystem.Metric.SI),
                            new Unit(LibraryResources.PoundsForcePerSqIn, "psi", 6894.76, UnitSystem.Imperial.USCS),
                            new Unit(LibraryResources.Atmospheres, "atm", 101325, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Bar, "bar", 1e5, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Kilopascals, "kPa", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(LibraryResources.Torr, "torr", 133.322, UnitSystem.Metric.BaselineSystem)
                        }

                    },
                    new UnitCategory(LibraryResources.Time)
                    {
                        Children =
                        {
                            new Unit(LibraryResources.Seconds, LibraryResources.SecondsAbbrev, UnitSystem.ImperialAndMetric.SI_USCS),
                            new Unit(LibraryResources.Minutes, LibraryResources.MinutesAbbrev, 60.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(LibraryResources.Hours, LibraryResources.HoursAbbrev, 3600.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(LibraryResources.Milliseconds, LibraryResources.MillisecondsAbbrev, 1e-3, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(LibraryResources.Days, LibraryResources.DaysAbbrev, (3600.0*24), UnitSystem.ImperialAndMetric.BaselineSystem)
                        }
                    }
                }                
            };

            _AllUnits.Children.Add(
                new UnitCategory(LibraryResources.IsothermalCompressibility,
                new UnitCategory.CompositeUnitElement[]
                {
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Pressure,
                        IsInverse = true,
                        power = UnitCategory.ToPowerOf.One
                    }
                })
                {

                });

            _AllUnits.Children.Add(
                new UnitCategory(LibraryResources.Area, 
                new UnitCategory.CompositeUnitElement[]
                {
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Length,
                        IsInverse = false,
                        power = UnitCategory.ToPowerOf.Two
                    }
                })
            {

            });

            _AllUnits.Children.Add(
                new UnitCategory(LibraryResources.VolumeExpansivity,
                new UnitCategory.CompositeUnitElement[]
                {
                                new UnitCategory.CompositeUnitElement()
                                {
                                    CategoryName = LibraryResources.Temperature,
                                    IsInverse = true,
                                    power = UnitCategory.ToPowerOf.One
                                }
                })
                {

                });
            _AllUnits.Children.Add(
                new UnitCategory(LibraryResources.Volume, 
                new UnitCategory.CompositeUnitElement[]
                {
                    new UnitCategory.CompositeUnitElement()
                    {
                        CategoryName = LibraryResources.Length,
                        IsInverse = false,
                        power = UnitCategory.ToPowerOf.Three
                    }
                })
            {
                    Children =
                    {
                        new Unit(LibraryResources.Gallons, "gal", 1/264.172, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Liters, "l", 1e-3, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Milliliters, "ml", 1e-6, UnitSystem.Imperial.BaselineSystem)
                    }

            });

            _AllUnits.Children.Add(
                    new UnitCategory(LibraryResources.VolumetricFlowRate,
                    new UnitCategory.CompositeUnitElement[]
                    {
                                    new UnitCategory.CompositeUnitElement()
                                    {
                                        CategoryName = LibraryResources.Volume,
                                        IsInverse = false,
                                        power = UnitCategory.ToPowerOf.One
                                    },
                                    new UnitCategory.CompositeUnitElement()
                                    {
                                        CategoryName = LibraryResources.Time,
                                        IsInverse = true,
                                        power = UnitCategory.ToPowerOf.One
                                    }
                    })
                {
                });

            _AllUnits.Children.Add(new UnitCategory(LibraryResources.Density, new UnitCategory.CompositeUnitElement[]
                    {
                        new UnitCategory.CompositeUnitElement()
                        {
                            CategoryName = LibraryResources.Mass,
                            IsInverse = false,
                            power = UnitCategory.ToPowerOf.One
                        },
                        new UnitCategory.CompositeUnitElement()
                        {
                            CategoryName = LibraryResources.Volume,
                            IsInverse = true,
                            power = UnitCategory.ToPowerOf.One
                        }
                    })
            {

            });

            _AllUnits.Children.Add(new UnitCategory(LibraryResources.SpecificVolume, new UnitCategory.CompositeUnitElement[]
            {
                            new UnitCategory.CompositeUnitElement()
                            {
                                CategoryName = LibraryResources.Mass,
                                IsInverse = true,
                                power = UnitCategory.ToPowerOf.One
                            },
                            new UnitCategory.CompositeUnitElement()
                            {
                                CategoryName = LibraryResources.Volume,
                                IsInverse = false,
                                power = UnitCategory.ToPowerOf.One
                            }
            })
            {

            });

            _AllUnits.Children.Add(new UnitCategory(LibraryResources.Enthalpy, new UnitCategory.CompositeUnitElement[]
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

            _AllUnits.Children.Add(new UnitCategory(LibraryResources.Entropy, new UnitCategory.CompositeUnitElement[]
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
