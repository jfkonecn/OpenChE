using EngineeringMath.Calculations.Components.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.Thermo.ThermoLookupTables
{
    public abstract class AbstractTable : FunctionSubber
    {
        internal AbstractTable(Resources.LookupTables.ThermoTable table) : base(AllSteamTableFuns)
        {
            Table = table;
        }

        private static Resources.LookupTables.ThermoTable Table;

        internal static readonly Dictionary<string, FunctionFactory.SolveForFactoryData> AllSteamTableFuns =
            new Dictionary<string, FunctionFactory.SolveForFactoryData>
            {
                { LibraryResources.SatLiquidGivenPressure, new FunctionFactory.SolveForFactoryData(typeof(SatLiquidGivenPressure)) },
                { LibraryResources.SatVaporGivenPressure, new FunctionFactory.SolveForFactoryData(typeof(SatVaporGivenPressure)) },
                { LibraryResources.SuperHeatedVaporGivenTempPressure, new FunctionFactory.SolveForFactoryData(typeof(SuperHeatedVaporGivenTempPressure)) }
            };

        public class SatLiquidGivenPressure : SimpleFunction
        {
            public SatLiquidGivenPressure() : base(
                    new SimpleParameter[]
                    {
                        CreatePressureParameter(),
                        CreateSaturatedTemperatureParameter(),
                        CreateSpecificVolumeParameter(),
                        CreateEnthalpyParameter(),
                        CreateEntropyParameter()
                    }
                )
            {

            }

            protected override void Calculation()
            {
                ThermoEntry entry = Table.GetThermoEntrySatLiquidAtPressure(Pressure);
                if(entry == null)
                {
                    // bad inputs
                    return;
                }
                Temperature = entry.Temperature;
                SpecificVolume = entry.V;
                Enthalpy = entry.H;
                Entropy = entry.S;                
            }

            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            public double Pressure
            {
                get
                {
                    return GetParameter((int)Field.pressure).Value;
                }
                set
                {
                    GetParameter((int)Field.pressure).Value = value;
                }
            }

            /// <summary>
            /// Temperature (C)
            /// </summary>
            public double Temperature
            {
                get
                {
                    return GetParameter((int)Field.temp).Value;
                }
                set
                {
                    GetParameter((int)Field.temp).Value = value;
                }
            }

            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            public double SpecificVolume
            {
                get
                {
                    return GetParameter((int)Field.v).Value;
                }
                set
                {
                    GetParameter((int)Field.v).Value = value;
                }
            }

            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            public double Enthalpy
            {
                get
                {
                    return GetParameter((int)Field.h).Value;
                }
                set
                {
                    GetParameter((int)Field.h).Value = value;
                }
            }

            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            public double Entropy
            {
                get
                {
                    return GetParameter((int)Field.s).Value;
                }
                set
                {
                    GetParameter((int)Field.s).Value = value;
                }
            }
        }

        public class SatVaporGivenPressure : SimpleFunction
        {
            public SatVaporGivenPressure() : base(
                    new SimpleParameter[]
                    {
                        CreatePressureParameter(),
                        CreateSaturatedTemperatureParameter(),
                        CreateSpecificVolumeParameter(),
                        CreateEnthalpyParameter(),
                        CreateEntropyParameter()
                    }
                )
            {

            }

            protected override void Calculation()
            {
                ThermoEntry entry = Table.GetThermoEntrySatVaporAtPressure(Pressure);
                if (entry == null)
                {
                    // bad inputs
                    return;
                }
                Temperature = entry.Temperature;
                SpecificVolume = entry.V;
                Enthalpy = entry.H;
                Entropy = entry.S;
            }

            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            public double Pressure
            {
                get
                {
                    return GetParameter((int)Field.pressure).Value;
                }
                set
                {
                    GetParameter((int)Field.pressure).Value = value;
                }
            }

            /// <summary>
            /// Temperature (C)
            /// </summary>
            public double Temperature
            {
                get
                {
                    return GetParameter((int)Field.temp).Value;
                }
                set
                {
                    GetParameter((int)Field.temp).Value = value;
                }
            }

            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            public double SpecificVolume
            {
                get
                {
                    return GetParameter((int)Field.v).Value;
                }
                set
                {
                    GetParameter((int)Field.v).Value = value;
                }
            }

            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            public double Enthalpy
            {
                get
                {
                    return GetParameter((int)Field.h).Value;
                }
                set
                {
                    GetParameter((int)Field.h).Value = value;
                }
            }

            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            public double Entropy
            {
                get
                {
                    return GetParameter((int)Field.s).Value;
                }
                set
                {
                    GetParameter((int)Field.s).Value = value;
                }
            }
        }


        public class SuperHeatedVaporGivenTempPressure : SimpleFunction
        {
            public SuperHeatedVaporGivenTempPressure() : base(
                    new SimpleParameter[]
                    {
                        CreatePressureParameter(),
                        CreateInputTemperatureParameter(),
                        CreateSpecificVolumeParameter(),
                        CreateEnthalpyParameter(),
                        CreateEntropyParameter()
                    }
                )
            {

            }

            protected override void Calculation()
            {
                ThermoEntry entry = Table.GetThermoEntryAtTemperatureAndPressure(Temperature, Pressure);
                if (entry == null)
                {
                    // bad inputs
                    return;
                }
                SpecificVolume = entry.V;
                Enthalpy = entry.H;
                Entropy = entry.S;
            }

            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            public double Pressure
            {
                get
                {
                    return GetParameter((int)Field.pressure).Value;
                }
                set
                {
                    GetParameter((int)Field.pressure).Value = value;
                }
            }

            /// <summary>
            /// Temperature (C)
            /// </summary>
            public double Temperature
            {
                get
                {
                    return GetParameter((int)Field.temp).Value;
                }
                set
                {
                    GetParameter((int)Field.temp).Value = value;
                }
            }

            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            public double SpecificVolume
            {
                get
                {
                    return GetParameter((int)Field.v).Value;
                }
                set
                {
                    GetParameter((int)Field.v).Value = value;
                }
            }

            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            public double Enthalpy
            {
                get
                {
                    return GetParameter((int)Field.h).Value;
                }
                set
                {
                    GetParameter((int)Field.h).Value = value;
                }
            }

            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            public double Entropy
            {
                get
                {
                    return GetParameter((int)Field.s).Value;
                }
                set
                {
                    GetParameter((int)Field.s).Value = value;
                }
            }
        }

        /// <summary>
        /// Creates a pressure parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreatePressureParameter()
        {
            return new SimpleParameter((int)Field.pressure, LibraryResources.Pressure, new AbstractUnit[] { Pressure.Pa }, true, Table.MinTablePressure, Table.MaxTablePressure);
        }

        /// <summary>
        /// Creates a input temperature parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreateInputTemperatureParameter()
        {
            return new SimpleParameter((int)Field.temp, LibraryResources.Temperature, new AbstractUnit[] { Temperature.C }, true, Table.MinTableTemperature, Table.MaxTableTemperature);
        }

        /// <summary>
        /// Creates a saturated temperature output parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreateSaturatedTemperatureParameter()
        {
            // will always be an output so don't care about the temperature range
            return new SimpleParameter((int)Field.temp, LibraryResources.SatTemperature, new AbstractUnit[] { Temperature.C }, false);
        }

        /// <summary>
        /// Creates a SpecificVolume parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreateSpecificVolumeParameter()
        {
            return new SimpleParameter((int)Field.v, LibraryResources.SpecificVolume, new AbstractUnit[] { SpecificVolume.m3kg }, false);
        }

        /// <summary>
        /// Creates a Enthalpy parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreateEnthalpyParameter()
        {
            return new SimpleParameter((int)Field.h, LibraryResources.Enthalpy, new AbstractUnit[] { Enthalpy.kJkg }, false);
        }

        /// <summary>
        /// Creates a Entropy parameter for all SimpleFunctions within this class
        /// </summary>
        /// <returns></returns>
        private static SimpleParameter CreateEntropyParameter()
        {
            return new SimpleParameter((int)Field.s, LibraryResources.Entropy, new AbstractUnit[] { Entropy.kJkgK }, false);
        }

        public enum Field
        {
            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            pressure,
            /// <summary>
            /// Temperature (C)
            /// </summary>
            temp,
            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            v,
            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            h,
            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            s
        }
    }
}
