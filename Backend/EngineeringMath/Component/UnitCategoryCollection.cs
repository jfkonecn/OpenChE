using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                lock (LockAllUnits)
                {
                    return _AllUnits;
                }                
            }
        }


        private static readonly System.Object LockAllUnits = new System.Object();

        public new UnitCategory GetCategoryByName(string catName)
        {
            return (UnitCategory)base.GetCategoryByName(catName);

        }

        private static Thread BuildUnitThread(string catName,
            Func<UnitCategory.UnitCategoryElement[], UnitCategory> unitFunc,
            params UnitCategoryElementThread[] eleTasks)
        {
            Thread thread = new Thread(() =>
            {
                Debug.WriteLine($"Thread {Thread.CurrentThread.Name} started!");
                UnitCategory.UnitCategoryElement[] unitCatsEles = null;
                if (eleTasks != null)
                {
                    unitCatsEles = new UnitCategory.UnitCategoryElement[eleTasks.Length];

                    for (int i = 0; i < unitCatsEles.Length; i++)
                    {
                        eleTasks[i].UnitCategoryThread.Join();
                        unitCatsEles[i] = new UnitCategory.UnitCategoryElement(eleTasks[i].UnitCategoryThread.Name, eleTasks[i].Power, false);
                    }
                }
                UnitCategory unitCat = unitFunc(unitCatsEles);
                lock (LockAllUnits)
                {
                    _AllUnits.Add(unitCat);
                }
                Debug.WriteLine($"Thread {Thread.CurrentThread.Name} is done!");

            })
            {
                Name = catName
            };
            thread.Start();
            return thread;
        }

        private static Func<UnitCategory.UnitCategoryElement[], UnitCategory> UnitCategoryBuilderFunc(
            string name, Func<Unit[]> getUnits)
        {
            return (unitCatEle) =>
            {
                UnitCategory unitCat = null;
                Debug.WriteLine($"{name}: Is about to build");
                if (unitCatEle == null)
                    unitCat = new UnitCategory(name, false);
                else
                    unitCat = new UnitCategory(name, unitCatEle, false);
                Debug.WriteLine($"{name}: successfully built");
                if (getUnits != null)
                {
                    foreach (Unit unit in getUnits())
                    {
                        unitCat.Add(unit);
                    }
                }
                return unitCat;
            };
        }

        private static void KeyValuePairBuilder(
            ref Dictionary<string, Thread> dic,
            string name, Func<Unit[]> getUnits, UnitCategoryElementThread[] eleTasks)
        {
            dic.Add(name, BuildUnitThread(name, UnitCategoryBuilderFunc(name, getUnits), eleTasks));
        }

        private static void KeyValuePairBuilder(
            ref Dictionary<string, Thread> dic, string name, UnitCategoryElementThread[] eleTasks)
        {
            KeyValuePairBuilder(ref dic, name, null, eleTasks);
        }

        private static void KeyValuePairBuilder(
            ref Dictionary<string, Thread> dic, string name, Func<Unit[]> getUnits)
        {
            KeyValuePairBuilder(ref dic, name, getUnits, null);
        }


        private static void BuildTemperatureKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
            ref dic,
            LibraryResources.Temperature, 
            () => 
            {
                return new Unit[]
                {
                    new Unit(LibraryResources.Kelvin, "K", 1.8, UnitSystem.Metric.SI),
                    new Unit(LibraryResources.Rankine, "°R", UnitSystem.Imperial.USCS),
                    new Unit(LibraryResources.Fahrenheit, "°F",$"${Unit.CurUnitVar} + 459.67",
                    $"${Unit.BaseUnitVar} - 459.67", UnitSystem.Metric.BaselineSystem),
                    new Unit(LibraryResources.Celsius, "°C",$"(${Unit.CurUnitVar} + 273.15) * 9/5",
                    $"${Unit.BaseUnitVar} * 5/9 - 273.15", UnitSystem.Imperial.BaselineSystem)
                };
            });
        }

        private static void BuildEnergyKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Energy,
                () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Joules, "J", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.BTU, "BTU", 1055.06, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Kilocalories, "kCal", 4184, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilojoules, "kJ", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Therms, "Therms", 1.055e+8, UnitSystem.Imperial.BaselineSystem)
                    };
                });
        }

        private static void BuildLengthKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Length, () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Meters, "m", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.Feet, "ft", 1 / 3.28084, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Inches, "in", 1 / 39.3701, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Miles, "mi", 1609.34, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Millimeters, "mm", 1e-3, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Centimeters, "cm", 1e-2, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilometers, "km", 1000, UnitSystem.Metric.BaselineSystem)
                    };
                });
        }

        private static void BuildMassKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Mass,
                () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Kilograms, "kg", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.PoundsMass, "lbs\u2098", 1/2.20462, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Grams, "g", 1e-3, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Milligrams, "mg", 1e-6, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Micrograms, "\u03BCg", 1e-9, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.MetricTons, "Mg", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Ounces, "oz", 1/35.274, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.USTons, "Ton", 907.185, UnitSystem.Imperial.BaselineSystem)
                    };
                });
        }

        private static void BuildPowerKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Power,
                () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Watt, "W", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.Horsepower, "Hp", 745.7, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Kilowatt, "kW", 1000, UnitSystem.Metric.BaselineSystem)
                    };
                });
        }

        private static void BuildPressureKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Pressure,
                () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Pascals, "Pa", UnitSystem.Metric.SI),
                        new Unit(LibraryResources.PoundsForcePerSqIn, "psi", 6894.76, UnitSystem.Imperial.USCS),
                        new Unit(LibraryResources.Atmospheres, "atm", 101325, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Bar, "bar", 1e5, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Kilopascals, "kPa", 1000, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Torr, "torr", 133.322, UnitSystem.Metric.BaselineSystem)
                    };
                });
        }

        private static void BuildTimeKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Time,
                () => 
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Seconds, LibraryResources.SecondsAbbrev, UnitSystem.ImperialAndMetric.SI_USCS),
                        new Unit(LibraryResources.Minutes, LibraryResources.MinutesAbbrev, 60.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Hours, LibraryResources.HoursAbbrev, 3600.0, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Milliseconds, LibraryResources.MillisecondsAbbrev, 1e-3, UnitSystem.ImperialAndMetric.BaselineSystem),
                        new Unit(LibraryResources.Days, LibraryResources.DaysAbbrev, (3600.0*24), UnitSystem.ImperialAndMetric.BaselineSystem)
                    };
                });
        }

        private static void BuildVelocityKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Velocity, 
                new UnitCategoryElementThread[] 
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Length], 1),
                    new UnitCategoryElementThread(dic[LibraryResources.Time], -1)
                });
        }

        private static void BuildIsothermalCompressibilityKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.IsothermalCompressibility,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Pressure], -1)
                });
        }

        private static void BuildAreaKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Area,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Length], 2)
                });
        }

        private static void BuildVolumeExpansivityKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.VolumeExpansivity,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Temperature], -1)
                });
        }

        private static void BuildVolumeKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Volume,
                () =>
                {
                    return new Unit[]
                    {
                        new Unit(LibraryResources.Gallons, "gal", 1 / 264.172, UnitSystem.Imperial.BaselineSystem),
                        new Unit(LibraryResources.Liters, "l", 1e-3, UnitSystem.Metric.BaselineSystem),
                        new Unit(LibraryResources.Milliliters, "ml", 1e-6, UnitSystem.Metric.BaselineSystem)
                    };
                },
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Length], 3)
                });
        }

        private static void BuildVolumetricFlowRateKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.VolumetricFlowRate,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Volume], 1),
                    new UnitCategoryElementThread(dic[LibraryResources.Time], -1)
                });
        }

        private static void BuildDensityKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Density,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Mass], 1),
                    new UnitCategoryElementThread(dic[LibraryResources.Volume], -1)
                });
        }

        private static void BuildSpecificVolumeKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.SpecificVolume,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Mass], -1),
                    new UnitCategoryElementThread(dic[LibraryResources.Volume], 1)
                });
        }

        private static void BuildEnthalpyKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Enthalpy,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Energy], 1),
                    new UnitCategoryElementThread(dic[LibraryResources.Mass], -1)
                });
        }

        private static void BuildEntropyKeyPair(ref Dictionary<string, Thread> dic)
        {
            KeyValuePairBuilder(
                ref dic,
                LibraryResources.Entropy,
                new UnitCategoryElementThread[]
                {
                    new UnitCategoryElementThread(dic[LibraryResources.Energy], 1),
                    new UnitCategoryElementThread(dic[LibraryResources.Mass], -1),
                    new UnitCategoryElementThread(dic[LibraryResources.Temperature], -1)
                });
        }

        internal static void BuildAllUnits()
        {

            _AllUnits = new UnitCategoryCollection(LibraryResources.AllUnits);
            Dictionary<string, Thread> dic = new Dictionary<string, Thread>();
            BuildTemperatureKeyPair(ref dic);
            BuildEnergyKeyPair(ref dic);
            BuildLengthKeyPair(ref dic);
            BuildMassKeyPair(ref dic);
            BuildPowerKeyPair(ref dic);
            BuildPressureKeyPair(ref dic);
            BuildTimeKeyPair(ref dic);
            BuildVelocityKeyPair(ref dic);
            BuildIsothermalCompressibilityKeyPair(ref dic);
            BuildAreaKeyPair(ref dic);
            BuildVolumeExpansivityKeyPair(ref dic);
            BuildVolumeKeyPair(ref dic);
            BuildVolumetricFlowRateKeyPair(ref dic);
            BuildDensityKeyPair(ref dic);
            BuildSpecificVolumeKeyPair(ref dic);
            BuildEnthalpyKeyPair(ref dic);
            BuildEntropyKeyPair(ref dic);
            foreach (Thread thread in dic.Values)
            {
                thread.Join();
            }
        }

        public class UnitCategoryElementThread
        {
            public UnitCategoryElementThread(Thread unitCategoryThread, int power)
            {
                UnitCategoryThread = unitCategoryThread;
                Power = power;
            }
            public Thread UnitCategoryThread { get; }
            public int Power { get; }
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
