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
            _AllUnits = new UnitCategoryCollection(nameof(LibraryResources.AllUnits))
            {
                Children =
                {
                    new UnitCategory(nameof(LibraryResources.Temperature))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Kelvin), "K", 1.8, UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.Rankine), "°R", UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Fahrenheit), "°F",$"${Unit.CurUnitVar} + 459.67", 
                            $"${Unit.BaseUnitVar} - 459.67", UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Celsius), "°C",$"(${Unit.CurUnitVar} + 273.15) * 9/5", 
                            $"${Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(nameof(LibraryResources.Energy))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Joules), "J", UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.BTU), "BTU", 1055.06, UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Kilocalories), "kCal", 4184, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Kilojoules), "kJ", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Therms), "Therms", 1.055e+8, UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(nameof(LibraryResources.Length))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Meters), "m", UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.Feet), "ft", 1 / 3.28084, UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Inches), "in", 1 / 39.3701, UnitSystem.Imperial.BaselineSystem),
                            new Unit(nameof(LibraryResources.Miles), "mi", 1609.34, UnitSystem.Imperial.BaselineSystem),
                            new Unit(nameof(LibraryResources.Millimeters), "mm", 1e-3, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Centimeters), "cm", 1e-2, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Kilometers), "km", 1000, UnitSystem.Metric.BaselineSystem)
                        }
                    },
                    new UnitCategory(nameof(LibraryResources.Mass))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Kilograms), "kg", UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.PoundsMass), "lbs\u2098", 1/2.20462, UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Grams), "g", 1e-3, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Milligrams), "mg", 1e-6, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Micrograms), "\u03BCg", 1e-9, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.MetricTons), "Mg", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Ounces), "oz", 1/35.274, UnitSystem.Imperial.BaselineSystem),
                            new Unit(nameof(LibraryResources.USTons), "Ton", 907.185, UnitSystem.Imperial.BaselineSystem)
                        }
                    },
                    new UnitCategory(nameof(LibraryResources.Power))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Watt), "W", UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.Horsepower), "Hp", 745.7, UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Kilowatt), "kW", 1000, UnitSystem.Metric.BaselineSystem)
                        }
                    },
                    new UnitCategory(nameof(LibraryResources.Pressure))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Pascals), "Pa", UnitSystem.Metric.SI),
                            new Unit(nameof(LibraryResources.PoundsForcePerSqIn), "psi", 6894.76, UnitSystem.Imperial.USCS),
                            new Unit(nameof(LibraryResources.Atmospheres), "atm", 101325, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Bar), "bar", 1e5, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Kilopascals), "kPa", 1000, UnitSystem.Metric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Torr), "torr", 133.322, UnitSystem.Metric.BaselineSystem)
                        }

                    },
                    new UnitCategory(nameof(LibraryResources.Time))
                    {
                        Children =
                        {
                            new Unit(nameof(LibraryResources.Seconds), LibraryResources.SecondsAbbrev, UnitSystem.ImperialAndMetric.SI_USCS),
                            new Unit(nameof(LibraryResources.Minutes), LibraryResources.MinutesAbbrev, 60.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Hours), LibraryResources.HoursAbbrev, 3600.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Milliseconds), LibraryResources.MillisecondsAbbrev, 1e-3, UnitSystem.ImperialAndMetric.BaselineSystem),
                            new Unit(nameof(LibraryResources.Days), LibraryResources.DaysAbbrev, (3600.0*24), UnitSystem.ImperialAndMetric.BaselineSystem)
                        }
                    }
                }                
            };

            _AllUnits.Children.Add(
                new UnitCategory(nameof(LibraryResources.IsothermalCompressibility),
                new UnitCategory.UnitCategoryElement[]
                {
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Pressure), -1)
                })
                {

                });

            _AllUnits.Children.Add(
                new UnitCategory(nameof(LibraryResources.Area), 
                new UnitCategory.UnitCategoryElement[]
                {
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Length), 2)
                })
            {

            });

            _AllUnits.Children.Add(
                new UnitCategory(nameof(LibraryResources.VolumeExpansivity),
                new UnitCategory.UnitCategoryElement[]
                {
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Temperature), -1)
                })
                {

                });
            _AllUnits.Children.Add(
                new UnitCategory(nameof(LibraryResources.Volume), 
                new UnitCategory.UnitCategoryElement[]
                {
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Length), 3)
                })
            {
                    Children =
                    {
                        new Unit(nameof(LibraryResources.Gallons), "gal", 1/264.172, UnitSystem.Imperial.BaselineSystem),
                        new Unit(nameof(LibraryResources.Liters), "l", 1e-3, UnitSystem.Imperial.BaselineSystem),
                        new Unit(nameof(LibraryResources.Milliliters), "ml", 1e-6, UnitSystem.Imperial.BaselineSystem)
                    }

            });

            _AllUnits.Children.Add(
                    new UnitCategory(nameof(LibraryResources.VolumetricFlowRate),
                    new UnitCategory.UnitCategoryElement[]
                    {
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Volume), 1),
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Time), -1)
                    })
                {
                });

            _AllUnits.Children.Add(new UnitCategory(nameof(LibraryResources.Density), new UnitCategory.UnitCategoryElement[]
                    {
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Mass), 1),
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Volume), -1)
                    })
            {

            });

            _AllUnits.Children.Add(new UnitCategory(nameof(LibraryResources.SpecificVolume), new UnitCategory.UnitCategoryElement[]
            {
                new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Mass), -1),
                new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Volume), 1)
            })
            {

            });

            _AllUnits.Children.Add(new UnitCategory(nameof(LibraryResources.Enthalpy), new UnitCategory.UnitCategoryElement[]
                    {
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Energy), 1),
                        new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Mass), -1)
                    })
            {
            });

            _AllUnits.Children.Add(new UnitCategory(nameof(LibraryResources.Entropy), new UnitCategory.UnitCategoryElement[]
                {
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Energy), 1),
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Mass), -1),
                    new UnitCategory.UnitCategoryElement(nameof(LibraryResources.Temperature), -1)
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
