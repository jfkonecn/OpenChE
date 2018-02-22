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
                double pressure = double.NaN;
                if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.pressure)))
                {
                    if (CurrentPhase == Phase.Vapor)
                    {
                        entry = Table.GetThermoEntrySatVaporAtPressure(Pressure);
                    }
                    else if (CurrentPhase == Phase.Liquid)
                    {
                        entry = Table.GetThermoEntrySatLiquidAtPressure(Pressure);
                    }
                    else
                    {
                        throw new Exception("Something went wrong...");
                    }                    
                }
                else if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.temp)))
                {
                    if (CurrentPhase == Phase.Vapor)
                    {
                        entry = Table.GetThermoEntrySatVaporAtSatTemp(Temperature, out pressure);
                    }
                    else if (CurrentPhase == Phase.Liquid)
                    {
                        entry = Table.GetThermoEntrySatLiquidAtSatTemp(Temperature, out pressure);                    
                    }
                    else
                    {
                        throw new Exception("Something went wrong...");
                    }
                                      
                }
                else
                {
                    throw new Exception("something went wrong...");
                }

                if (entry != null && !double.IsNaN(pressure))
                {
                    Pressure = pressure;
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
                                CreateEntropyParameter()
                }
            )
            {
                CurrentPhase = Phase.Vapor;
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
            protected Phase CurrentPhase
            {
                get
                {
                    return (Phase)PhaseSelection.SelectedObject;
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
                { LibraryResources.Vapor, (int)Phase.Vapor },
                { LibraryResources.Liquid, (int)Phase.Liquid }
            }, LibraryResources.Phase);


            protected override void Calculation()
            {
                ThermoEntry entry = Table.GetThermoEntryAtTemperatureAndPressure(Temperature, Pressure);
                FinishCalculation(entry);

                if(entry != null)
                {
                    // update phase of this species
                    double satTemp = Table.GetThermoEntrySatLiquidAtPressure(Pressure).Temperature;

                    if (satTemp > Temperature)
                    {
                        CurrentPhase = Phase.Liquid;
                    }
                    else
                    {
                        CurrentPhase = Phase.Vapor;
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

            public enum Phase
            {
                Vapor,
                Liquid
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
