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
using System.Collections.ObjectModel;

namespace EngineeringMath.Calculations.Thermo.ThermoLookupTables
{
    public abstract class AbstractTable : FunctionSubber
    {
        internal AbstractTable(ThermoTable table) : base(new TableFunctionPicker(table))
        {
            
        }

        internal static readonly Dictionary<string, Type> AllSteamTableFuns =
            new Dictionary<string, Type>
            {
                { LibraryResources.SaturatedSpecies, typeof(SatTemperature) },
                { LibraryResources.NonSaturatedSpecies, typeof(PropertyGivenTempPressure) }
            };


        public class TableFunctionPicker : FunctionPicker
        {
            internal TableFunctionPicker(ThermoTable table) : base(new Dictionary<string, Type>
            {
                { LibraryResources.SaturatedSpecies, typeof(SatTemperature) },
                { LibraryResources.NonSaturatedSpecies, typeof(PropertyGivenTempPressure) }
            })
            {
                Table = table;
                this.SelectedIndex = 0;
            }

            private readonly ThermoTable Table;

            protected override SimpleFunction FunctionConstructor()
            {
                if (typeof(SatTemperature).Equals(SelectedObject))
                {
                    return new SatTemperature(Table);
                }
                else if (typeof(PropertyGivenTempPressure).Equals(SelectedObject))
                {
                    return new PropertyGivenTempPressure(Table);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }



        public class SatTemperature : PropertyGivenTempPressure
        {
            internal SatTemperature(ThermoTable table) : base(table)
            {
                PhaseSelection.IsEnabled = true;
                ParameterBeingUsed.OnSelectedIndexChanged += ParameterBeingUsed_OnSelectedIndexChanged;
                ParameterBeingUsed.SelectedIndex = 0;
            }

            

            private void ParameterBeingUsed_OnSelectedIndexChanged(object sender, EventArgs e)
            {
                OnReset();
                if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.pressure)))
                {
                    GetParameter((int)Field.pressure).IsInput = true;
                    GetParameter((int)Field.temp).IsOutput = true;
                }
                else if (ParameterBeingUsed.SelectedObject.Equals(GetParameter((int)Field.temp)))
                {
                    GetParameter((int)Field.pressure).IsOutput = true;
                    GetParameter((int)Field.temp).IsInput = true;
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


            protected override SimpleParameter CreateInputTemperatureParameter()
            {
                return new SimpleParameter((int)Field.temp, LibraryResources.SatTemperature, new AbstractUnit[] { Units.Temperature.C }, true, Table.MinSatTableTemperature, Table.MaxSatTableTemperature);
            }

            protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
            {

                ObservableCollection<AbstractComponent> oldCollection = base.CreateRemainingDefaultComponentCollection();
                SimpleParameter pressure = oldCollection.Single((x) => x.ID == (int)Field.pressure && x as SimpleParameter != null) as SimpleParameter;
                SimpleParameter temperature = oldCollection.Single((x) => x.ID == (int)Field.temp && x as SimpleParameter != null) as SimpleParameter;

                ParameterBeingUsed = new SimplePicker<SimpleParameter>(new SimpleParameter[]
                {
                                    pressure,
                                    temperature
                }.ToDictionary(x => x.Title, x => x)
                , LibraryResources.Given);
                ObservableCollection<AbstractComponent> temp = new ObservableCollection<AbstractComponent>
                {
                    ParameterBeingUsed
                };
                foreach (AbstractComponent comp in oldCollection)
                {
                    temp.Add(comp);
                }                
                return temp;
            }
        }


        public class PropertyGivenTempPressure : SimpleFunction
        {
            internal PropertyGivenTempPressure(ThermoTable table)
            {

                Table = table;
                BuildComponentCollection();
                CurrentPhase = ThermoEntry.Phase.vapor;
                PhaseSelection.IsEnabled = false;
                PhaseSelection.OnSelectedIndexChanged += PhaseSelection_OnSelectedIndexChanged;
            }

            private void PhaseSelection_OnSelectedIndexChanged(object sender, EventArgs e)
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

            protected readonly ThermoTable Table;

            /// <summary>
            /// Pressure (Pa)
            /// </summary>
            public SimpleParameter Pressure
            {
                get
                {
                    return GetParameter((int)Field.pressure);
                }
            }

            /// <summary>
            /// Temperature (C)
            /// </summary>
            public SimpleParameter Temperature
            {
                get
                {
                    return GetParameter((int)Field.temp);
                }
            }

            /// <summary>
            /// Specific Volume (m3/kg)
            /// </summary>
            public SimpleParameter SpecificVolume
            {
                get
                {
                    return GetParameter((int)Field.v);
                }
            }

            /// <summary>
            /// Enthalpy (kJ/kg)
            /// </summary>
            public SimpleParameter Enthalpy
            {
                get
                {
                    return GetParameter((int)Field.h);
                }
            }

            /// <summary>
            /// Entropy (kJ/(kg*K))
            /// </summary>
            public SimpleParameter Entropy
            {
                get
                {
                    return GetParameter((int)Field.s);
                }
            }


            /// <summary>
            /// Volume Expansivity or Coefficient of thermal expansion  (1/K)
            /// </summary>
            public SimpleParameter Beta
            {
                get
                {
                    return GetParameter((int)Field.beta);
                }
            }

            /// <summary>
            /// Isothermal Compressibility (1/Pa)
            /// </summary>
            public SimpleParameter Kappa
            {
                get
                {
                    return GetParameter((int)Field.kappa);
                }
            }

            /// <summary>
            /// Heat Capacity Constant Pressure (kJ/(kg * K))
            /// </summary>
            public SimpleParameter Cp
            {
                get
                {
                    return GetParameter((int)Field.cp);
                }
            }

            /// <summary>
            /// Heat Capacity Constant Volume (kJ/(kg * K))
            /// </summary>
            public SimpleParameter Cv
            {
                get
                {
                    return GetParameter((int)Field.cv);
                }
            }

            /// <summary>
            /// Creates a pressure parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreatePressureParameter()
            {
                return new SimpleParameter((int)Field.pressure, LibraryResources.Pressure, new AbstractUnit[] { Units.Pressure.Pa }, true, Table.MinTablePressure, Table.MaxTablePressure);
            }

            /// <summary>
            /// Creates a input temperature parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            protected virtual SimpleParameter CreateInputTemperatureParameter()
            {
                return new SimpleParameter((int)Field.temp, LibraryResources.Temperature, new AbstractUnit[] { Units.Temperature.C }, true, Table.MinTableTemperature, Table.MaxTableTemperature);
            }
            /// <summary>
            /// Creates a SpecificVolume parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateSpecificVolumeParameter()
            {
                return new SimpleParameter((int)Field.v, LibraryResources.SpecificVolume, new AbstractUnit[] { Units.SpecificVolume.m3kg }, false);
            }

            /// <summary>
            /// Creates a Enthalpy parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateEnthalpyParameter()
            {
                return new SimpleParameter((int)Field.h, LibraryResources.Enthalpy, new AbstractUnit[] { Units.Enthalpy.kJkg }, false);
            }

            /// <summary>
            /// Creates a Entropy parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateEntropyParameter()
            {
                return new SimpleParameter((int)Field.s, LibraryResources.Entropy, new AbstractUnit[] { Units.Entropy.kJkgK }, false);
            }



            /// <summary>
            /// Creates a beta parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateBetaParameter()
            {
                return new SimpleParameter((int)Field.beta, LibraryResources.VolumeExpansivity, new AbstractUnit[] { Units.VolumeExpansivity.Kinv }, false);
            }

            /// <summary>
            /// Creates a kappa parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateKappaParameter()
            {
                return new SimpleParameter((int)Field.kappa, LibraryResources.IsothermalCompressibility, new AbstractUnit[] { Units.IsothermalCompressibility.PaInv }, false);
            }

            /// <summary>
            /// Creates a Cp parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateCpParameter()
            {
                // Entropy and Heat Capacity have the same units
                return new SimpleParameter((int)Field.cp, LibraryResources.HeatCapacityConstantPressure, new AbstractUnit[] { Units.Entropy.kJkgK }, false);
            }

            /// <summary>
            /// Creates a Cv parameter for all SimpleFunctions within this class
            /// </summary>
            /// <returns></returns>
            private SimpleParameter CreateCvParameter()
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


            protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
            {
                return new ObservableCollection<AbstractComponent>
                {
                    PhaseSelection,
                    CreatePressureParameter(),
                    CreateInputTemperatureParameter(),
                    CreateSpecificVolumeParameter(),
                    CreateEnthalpyParameter(),
                    CreateEntropyParameter(),
                    CreateBetaParameter(),
                    CreateKappaParameter(),
                    CreateCpParameter(),
                    CreateCvParameter()
                };
            }
        }


    }
}
