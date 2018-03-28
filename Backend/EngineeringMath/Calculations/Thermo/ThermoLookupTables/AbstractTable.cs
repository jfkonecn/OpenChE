﻿using EngineeringMath.Calculations.Components.Functions;
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
                    entry = Table.GetThermoEntryAtSatPressure(Pressure.Value, CurrentPhase);
                }
                else if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.temp)))
                {
                    tempIsInput = true;
                    entry = Table.GetThermoEntryAtSatTemp(Temperature.Value, CurrentPhase);

                }
                else
                {
                    throw new Exception("something went wrong...");
                }

                if (entry != null && tempIsInput)
                {
                    Pressure.Value = entry.Pressure;
                }
                else if (entry != null)
                {
                    Temperature.Value = entry.Temperature;
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


            protected PropertyGivenTempPressure(SimpleParameter temperature)
            {
                Pressure = CreatePressureParameter();
                Temperature = temperature;
                SpecificVolume = CreateSpecificVolumeParameter();
                Enthalpy = CreateEnthalpyParameter();
                Entropy = CreateEntropyParameter();
                Beta = CreateBetaParameter();
                Kappa = CreateKappaParameter();
                Cp = CreateCpParameter();
                Cv = CreateCvParameter();

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
                ThermoEntry entry = Table.GetThermoEntryAtTemperatureAndPressure(Temperature.Value, Pressure.Value);
                FinishCalculation(entry);

                if(entry != null)
                {
                    // update phase of this species
                    double satTemp = Table.GetThermoEntryAtSatPressure(Pressure.Value, ThermoEntry.Phase.liquid).Temperature;

                    if (satTemp > Temperature.Value)
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
                SpecificVolume.Value = entry.V;
                Enthalpy.Value = entry.H;
                Entropy.Value = entry.S;
                Beta.Value = entry.Beta;
                Kappa.Value = entry.Kappa;
                Cp.Value = entry.Cp;
                Cv.Value = entry.Cv;
            }


            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            public readonly SimpleParameter Pressure;

            /// <summary>
            /// Temperature (C)
            /// </summary>
            public readonly SimpleParameter Temperature;

            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            public readonly SimpleParameter SpecificVolume;

            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            public readonly SimpleParameter Enthalpy;

            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            public readonly SimpleParameter Entropy;


            /// <summary>
            /// Volume Expansivity or Coefficient of thermal expansion  (1/K)
            /// </summary>
            public readonly SimpleParameter Beta;

            /// <summary>
            /// Isothermal Compressibility (1/Pa)
            /// </summary>
            public readonly SimpleParameter Kappa;

            /// <summary>
            /// Heat Capacity Constant Pressure (kJ/(kg * K))
            /// </summary>
            public readonly SimpleParameter Cp;

            /// <summary>
            /// Heat Capacity Constant Volume (kJ/(kg * K))
            /// </summary>
            public readonly SimpleParameter Cv;

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

            public override SimpleParameter GetParameter(int ID)
            {
                switch ((Field)ID)
                {
                    case Field.pressure:
                        return Pressure;
                    case Field.temp:
                        return Temperature;
                    case Field.v:
                        return SpecificVolume;
                    case Field.h:
                        return Enthalpy;
                    case Field.s:
                        return Entropy;
                    case Field.beta:
                        return Beta;
                    case Field.kappa:
                        return Kappa;
                    case Field.cp:
                        return Cp;
                    case Field.cv:
                        return Cv;
                    default:
                        throw new NotImplementedException();
                }
                
            }

            internal override IEnumerable<SimpleParameter> ParameterCollection()
            {
                yield return Pressure;
                yield return Temperature;
                yield return SpecificVolume;
                yield return Enthalpy;
                yield return Entropy;
                yield return Beta;
                yield return Kappa;
                yield return Cp;
                yield return Cv;
            }
        }


    }
}
