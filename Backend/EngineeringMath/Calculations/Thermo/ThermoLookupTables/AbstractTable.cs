using EngineeringMath.Calculations.Components.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Selectors;
using System.Collections;
using EngineeringMath.Resources.LookupTables;

namespace EngineeringMath.Calculations.Thermo.ThermoLookupTables
{
    public abstract class AbstractTable : FunctionSubber
    {
        internal AbstractTable(Resources.LookupTables.ThermoTable table) : base(AllSteamTableFuns)
        {
            Table = table;
        }

        private static Resources.LookupTables.ThermoTable Table;

        internal static readonly Dictionary<string, Type> AllSteamTableFuns =
            new Dictionary<string, Type>
            {
                { LibraryResources.SaturatedSpecies, typeof(SatTemperature) },
                { LibraryResources.NonSaturatedSpecies, typeof(PropertyGivenTempPressure) }
            };

        public class SatTemperature : PropertyGivenTempPressure
        {
            public SatTemperature() : base(
                new SimpleParameter((int)Field.temp, LibraryResources.SatTemperature, new AbstractUnit[] { Units.Temperature.C }, true, Table.MinSatTableTemperature, Table.MaxSatTableTemperature))
            {
                ParameterBeingUsed = new SimplePicker<SimpleParameter>(new SimpleParameter[]
                    {
                       GetParameter((int)Field.pressure),
                        GetParameter((int)Field.temp) 
                    }.ToDictionary(x => x.Title, x => x)
                    , LibraryResources.Given);
                PhaseSelection.IsEnabled = true;
                ParameterBeingUsed.OnSelectedIndexChanged += ParameterBeingUsed_OnSelectedIndexChanged;
                ParameterBeingUsed.SelectedIndex = 0;
            }

            private void ParameterBeingUsed_OnSelectedIndexChanged()
            {
                OnReset();
                if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.pressure)))
                {
                    GetParameter((int)Field.pressure).isInput = true;
                    GetParameter((int)Field.temp).isOutput = true;
                }
                else if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.temp)))
                {
                    GetParameter((int)Field.pressure).isOutput = true;
                    GetParameter((int)Field.temp).isInput = true;
                }
                else
                {
                    throw new Exception("something went wrong...");
                }
            }

            protected override void Calculation()
            {
                ThermoEntry entry = null;
                bool tempIsInput = false;
                if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.pressure)))
                {
                    entry = Table.GetThermoEntryAtSatPressure(Pressure, CurrentPhase);
                }
                else if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.temp)))
                {
                    tempIsInput = true;
                    entry = Table.GetThermoEntryAtSatTemp(Temperature, CurrentPhase);

                }
                else
                {
                    throw new Exception("something went wrong...");
                }

                if (entry != null && tempIsInput)
                {
                    Pressure = entry.Pressure;
                }
                else if (entry != null)
                {
                    Temperature = entry.Temperature;
                }

                FinishCalculation(entry);
            }

            /// <summary>
            /// The parameter being used to look up the saturation point information
            /// </summary>
            public SimplePicker<SimpleParameter> ParameterBeingUsed;


            public override IEnumerator GetEnumerator()
            {
                yield return ParameterBeingUsed;
                yield return PhaseSelection;                
                foreach (AbstractComponent obj in ParameterCollection())
                {
                    yield return obj;
                }
            }
        }


        public class PropertyGivenTempPressure : SimpleFunction
        {
            public PropertyGivenTempPressure() : this(
                    CreateInputTemperatureParameter()
                )
            {

            }


            protected PropertyGivenTempPressure(SimpleParameter temperature) : base(
                new SimpleParameter[]
                {
                                CreatePressureParameter(),
                                temperature,
                                CreateSpecificVolumeParameter(),
                                CreateEnthalpyParameter(),
                                CreateEntropyParameter(),
                                CreateBetaParameter(),
                                CreateKappaParameter(),
                                CreateCpParameter(),
                                CreateCvParameter()
                }
            )
            {
                CurrentPhase = ThermoEntry.Phase.vapor;
                PhaseSelection.IsEnabled = false;
                PhaseSelection.OnSelectedIndexChanged += PhaseSelection_OnSelectedIndexChanged;
            }

            private void PhaseSelection_OnSelectedIndexChanged()
            {
                OnReset();
            }

            /// <summary>
            /// The current phase of the themo properties
            /// </summary>
            /// <returns></returns>
            protected ThermoEntry.Phase CurrentPhase
            {
                get
                {
                    return (ThermoEntry.Phase)PhaseSelection.SelectedObject;
                }
                set
                {
                    PhaseSelection.SelectedObject = (int)value;
                }
            }

            /// <summary>
            /// The selection of what phase the thermo properties are in 
            /// </summary>
            public SimplePicker<int> PhaseSelection = new SimplePicker<int>(new Dictionary<string, int> {
                { LibraryResources.Vapor, (int)ThermoEntry.Phase.vapor },
                { LibraryResources.Liquid, (int)ThermoEntry.Phase.liquid }
            }, LibraryResources.Phase);


            protected override void Calculation()
            {
                ThermoEntry entry = Table.GetThermoEntryAtTemperatureAndPressure(Temperature, Pressure);
                FinishCalculation(entry);

                if(entry != null)
                {
                    // update phase of this species
                    double satTemp = Table.GetThermoEntryAtSatPressure(Pressure, ThermoEntry.Phase.liquid).Temperature;

                    if (satTemp > Temperature)
                    {
                        CurrentPhase = ThermoEntry.Phase.liquid;
                    }
                    else
                    {
                        CurrentPhase = ThermoEntry.Phase.vapor;
                    }
                }
            }

            /// <summary>
            /// Updates all of the entries given the resulting entry, but does not change the pressure or temperature
            /// </summary>
            /// <param name="entry"></param>

            protected void FinishCalculation(ThermoEntry entry)
            {
                if (entry == null)
                {
                    // bad inputs
                    return;
                }
                SpecificVolume = entry.V;
                Enthalpy = entry.H;
                Entropy = entry.S;
                Beta = entry.Beta;
                Kappa = entry.Kappa;
                Cp = entry.Cp;
                Cv = entry.Cv;
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


            /// <summary>
            /// Volume Expansivity or Coefficient of thermal expansion  (1/K)
            /// </summary>
            public double Beta
            {
                get
                {
                    return GetParameter((int)Field.beta).Value;
                }
                set
                {
                    GetParameter((int)Field.beta).Value = value;
                }
            }
            /// <summary>
            /// Isothermal Compressibility (1/Pa)
            /// </summary>
            public double Kappa
            {
                get
                {
                    return GetParameter((int)Field.kappa).Value;
                }
                set
                {
                    GetParameter((int)Field.kappa).Value = value;
                }
            }
            /// <summary>
            /// Heat Capacity Constant Pressure (kJ/(kg * K))
            /// </summary>
            public double Cp
            {
                get
                {
                    return GetParameter((int)Field.cp).Value;
                }
                set
                {
                    GetParameter((int)Field.cp).Value = value;
                }
            }
            /// <summary>
            /// Heat Capacity Constant Volume (kJ/(kg * K))
            /// </summary>
            public double Cv
            {
                get
                {
                    return GetParameter((int)Field.cv).Value;
                }
                set
                {
                    GetParameter((int)Field.cv).Value = value;
                }
            }

            /// <summary>
            /// Creates a pressure parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreatePressureParameter()
            {
                return new SimpleParameter((int)Field.pressure, LibraryResources.Pressure, new AbstractUnit[] { Units.Pressure.Pa }, true, Table.MinTablePressure, Table.MaxTablePressure);
            }

            /// <summary>
            /// Creates a input temperature parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateInputTemperatureParameter()
            {
                return new SimpleParameter((int)Field.temp, LibraryResources.Temperature, new AbstractUnit[] { Units.Temperature.C }, true, Table.MinTableTemperature, Table.MaxTableTemperature);
            }

            /// <summary>
            /// Creates a saturated temperature output parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            protected static SimpleParameter CreateSaturatedTemperatureParameter()
            {
                // will always be an output so don't care about the temperature range
                return new SimpleParameter((int)Field.temp, LibraryResources.SatTemperature, new AbstractUnit[] { Units.Temperature.C }, false);
            }

            /// <summary>
            /// Creates a SpecificVolume parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateSpecificVolumeParameter()
            {
                return new SimpleParameter((int)Field.v, LibraryResources.SpecificVolume, new AbstractUnit[] { Units.SpecificVolume.m3kg }, false);
            }

            /// <summary>
            /// Creates a Enthalpy parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateEnthalpyParameter()
            {
                return new SimpleParameter((int)Field.h, LibraryResources.Enthalpy, new AbstractUnit[] { Units.Enthalpy.kJkg }, false);
            }

            /// <summary>
            /// Creates a Entropy parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateEntropyParameter()
            {
                return new SimpleParameter((int)Field.s, LibraryResources.Entropy, new AbstractUnit[] { Units.Entropy.kJkgK }, false);
            }



            /// <summary>
            /// Creates a beta parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateBetaParameter()
            {
                return new SimpleParameter((int)Field.beta, LibraryResources.VolumeExpansivity, new AbstractUnit[] { Units.VolumeExpansivity.Kinv }, false);
            }

            /// <summary>
            /// Creates a kappa parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateKappaParameter()
            {
                return new SimpleParameter((int)Field.kappa, LibraryResources.IsothermalCompressibility, new AbstractUnit[] { Units.IsothermalCompressibility.PaInv }, false);
            }

            /// <summary>
            /// Creates a Cp parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateCpParameter()
            {
                // Entropy and Heat Capacity have the same units
                return new SimpleParameter((int)Field.cp, LibraryResources.HeatCapacityConstantPressure, new AbstractUnit[] { Units.Entropy.kJkgK }, false);
            }

            /// <summary>
            /// Creates a Cv parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private static SimpleParameter CreateCvParameter()
            {
                // Entropy and Heat Capacity have the same units
                return new SimpleParameter((int)Field.cv, LibraryResources.HeatCapacityConstantVolume, new AbstractUnit[] { Units.Entropy.kJkgK }, false);
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
                s,
                /// <summary>
                /// Volume Expansivity or Coefficient of thermal expansion (1/K)
                /// </summary>
                beta,
                /// <summary>
                /// Isothermal Compressibility (1/Pa)
                /// </summary>
                kappa,
                /// <summary>
                /// Heat Capacity Constant Pressure (kJ/(kg * K))
                /// </summary>
                cp,
                /// <summary>
                /// Heat Capacity Constant Volume (kJ/(kg * K))
                /// </summary>
                cv
            }





            public override IEnumerator GetEnumerator()
            {
                yield return PhaseSelection;
                foreach (AbstractComponent obj in ParameterCollection())
                {
                    yield return obj;
                }
            }
        }


    }
}
