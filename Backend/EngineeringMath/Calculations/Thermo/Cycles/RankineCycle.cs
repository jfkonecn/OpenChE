using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Functions;
using System.Collections;
using EngineeringMath.Resources.LookupTables;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;
using System.Collections.ObjectModel;

namespace EngineeringMath.Calculations.Thermo.Cycles
{
    public class RankineCycle : SimpleFunction
    {
        public RankineCycle():this(SteamTable.Table)
        {

        }
        
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public RankineCycle(ThermoTable table) : base()
        {




            this.Title = LibraryResources.RankineCycle;
            this.Table = table;
            BuildComponentCollection();
#if DEBUG
            BoilerPressure.Value = 8600e3;
            BoilerTemperature.Value = 500;
            CondenserPressure.Value = 10e3;
            PumpEfficiency.Value = 0.75;
            TurbineEfficiency.Value = 0.75;
            PowerRequirement.Value = 80e3;
#endif

        }

        public enum Field
        {
            /// <summary>
            /// Steam Pressure (Pa)
            /// </summary>
            boilerP,
            /// <summary>
            /// Steam Temperature (C)
            /// </summary>
            boilerTemp,
            /// <summary>
            /// Mass Flow Rate of steam in the cycle (kg/s)
            /// </summary>
            steamRate,
            /// <summary>
            /// Condenser Pressure (Pa)
            /// </summary>
            condenserP,
            /// <summary>
            /// Condenser Steam Quality Fraction (Unitless)
            /// </summary>
            condenserSQ,
            /// <summary>
            /// Work done by the condenser (kJ/kg)
            /// </summary>
            condenserQ,
            /// <summary>
            /// Condenser Heat Transfer Rate (kJ/s)
            /// </summary>
            condenserHeatTrans,
            /// <summary>
            /// Work done by the turbine (kJ/kg)
            /// </summary>
            turbineQ,
            /// <summary>
            /// Turbine Efficiency Fraction (Unitless)
            /// </summary>
            turbineEff,
            /// <summary>
            /// Power Requirement (kW)
            /// </summary>
            powerReq,
            /// <summary>
            /// Pump Efficiency Fraction (Unitless)
            /// </summary>
            pumpEff,
            /// <summary>
            /// Work done by the pump (kJ/kg)
            /// </summary>
            pumpQ,
            /// <summary>
            /// Work done by the boiler (kJ/kg)
            /// </summary>
            boilerQ,
            /// <summary>
            /// Boiler Heat Transfer Rate (kJ/s)
            /// </summary>
            boilerHeatTrans,
            /// <summary>
            /// Work produced by the system (kJ/kg)
            /// </summary>
            netQ,
            /// <summary>
            /// Thermal Efficiency Fraction (Unitless)
            /// </summary>
            thermoEff
        };


        /// <summary>
        /// Power Requirement for the cycle (kW)
        /// </summary>
        public SimpleParameter PowerRequirement
        {
            get
            {
                return GetParameter((int)Field.powerReq);
            }
        }


        /// <summary>
        /// Boiler Pressure (Pa)
        /// </summary>
        public SimpleParameter BoilerPressure
        {
            get
            {
                return GetParameter((int)Field.boilerP);
            }
        }

        /// <summary>
        /// Boiler Temperature (C)
        /// </summary>
        public SimpleParameter BoilerTemperature
        {
            get
            {
                return GetParameter((int)Field.boilerTemp);
            }
        }

        /// <summary>
        /// Steam Mass Flow Rate in the system (kg/s)
        /// </summary>
        public SimpleParameter SteamRate
        {
            get
            {
                return GetParameter((int)Field.steamRate);
            }
        }

        /// <summary>
        /// Condenser Pressure (Pa)
        /// </summary>
        public SimpleParameter CondenserPressure
        {
            get
            {
                return GetParameter((int)Field.condenserP);
            }
        }

        /// <summary>
        /// Condenser Steam Quality (Unitless)
        /// </summary>
        public SimpleParameter CondenserSteamQuality
        {
            get
            {
                return GetParameter((int)Field.condenserSQ);
            }
        }

        /// <summary>
        /// Condenser Work (kJ/kg)
        /// </summary>
        public SimpleParameter CondenserWork
        {
            get
            {
                return GetParameter((int)Field.condenserQ);
            }
        }

        /// <summary>
        /// Condenser Heat Transfer Rate (kJ/s)
        /// </summary>
        public SimpleParameter CondenserHeatTransRate
        {
            get
            {
                return GetParameter((int)Field.condenserHeatTrans);
            }
        }

        /// <summary>
        /// Turbine Work (kJ/kg)
        /// Negative number means turbine is consuming energy
        /// </summary>
        public SimpleParameter TurbineWork
        {
            get
            {
                return GetParameter((int)Field.turbineQ);
            }
        }

        /// <summary>
        /// Pump Work (kJ/kg)
        /// </summary>
        public SimpleParameter PumpWork
        {
            get
            {
                return GetParameter((int)Field.pumpQ);
            }
        }

        /// <summary>
        /// Boiler Work (kJ/kg)
        /// </summary>
        public SimpleParameter BoilerWork
        {
            get
            {
                return GetParameter((int)Field.boilerQ);
            }
        }


        /// <summary>
        /// Boiler Heat Transfer Rate (kJ/s)
        /// </summary>
        public SimpleParameter BoilerHeatTransRate
        {
            get
            {
                return GetParameter((int)Field.boilerHeatTrans);
            }
        }

        /// <summary>
        /// Net Work (kJ/kg)
        /// </summary>
        public SimpleParameter NetWork
        {
            get
            {
                return GetParameter((int)Field.netQ);
            }
        }

        /// <summary>
        /// Turbine Efficiency (Unitless)
        /// </summary>
        public SimpleParameter TurbineEfficiency
        {
            get
            {
                return GetParameter((int)Field.turbineEff);
            }
        }

        /// <summary>
        /// Pump Efficiency (Unitless)
        /// </summary>
        public SimpleParameter PumpEfficiency
        {
            get
            {
                return GetParameter((int)Field.pumpEff);
            }
        }

        /// <summary>
        /// Thermal Efficiency (Unitless)
        /// </summary>
        public SimpleParameter ThermalEfficiency
        {
            get
            {
                return GetParameter((int)Field.thermoEff);
            }
        }

        /// <summary>
        /// The thermo table being used in this function
        /// </summary>
        protected readonly ThermoTable Table;

        protected override void Calculation()
        {
            ThermoEntry boilerConditions = Table.GetThermoEntryAtTemperatureAndPressure(BoilerTemperature.Value, BoilerPressure.Value),
                condenserLiquidConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure.Value, ThermoEntry.Phase.liquid),
                condenserConditions = Table.IsentropicExpansion(BoilerTemperature.Value, BoilerPressure.Value, CondenserPressure.Value, out double temp);

            CondenserSteamQuality.Value = temp;           

            PumpWork.Value = ((condenserLiquidConditions.V * (BoilerPressure.Value - CondenserPressure.Value)) * 1e-3) / PumpEfficiency.Value;

            // in kj / kg
            double boilerEnthalpy = condenserLiquidConditions.H + PumpWork.Value;

            BoilerWork.Value = boilerConditions.H - boilerEnthalpy;

            TurbineWork.Value = -(condenserConditions.H - boilerConditions.H) * TurbineEfficiency.Value;

            CondenserWork.Value = -(condenserLiquidConditions.H - (boilerConditions.H - TurbineWork.Value));

            NetWork.Value =  TurbineWork.Value - PumpWork.Value;

            ThermalEfficiency.Value = Math.Abs(TurbineWork.Value) / BoilerWork.Value;

            SteamRate.Value = PowerRequirement.Value / NetWork.Value;

            BoilerHeatTransRate.Value = SteamRate.Value * BoilerWork.Value;

            CondenserHeatTransRate.Value = SteamRate.Value * CondenserWork.Value;
        }

    

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.boilerP, LibraryResources.BoilerPressure, new AbstractUnit[] { Pressure.Pa }, true, Table.MinTablePressure, Table.MaxTablePressure),
                new SimpleParameter((int)Field.boilerTemp, LibraryResources.BoilerTemp, new AbstractUnit[] { Temperature.C }, true, Table.MinTableTemperature, Table.MaxTableTemperature),
                new SimpleParameter((int)Field.condenserP, LibraryResources.CondenserPressure, new AbstractUnit[] { Pressure.Pa }, true, Table.MinTablePressure, Table.MaxTablePressure),
                new SimpleParameter((int)Field.pumpEff, LibraryResources.PumpEfficiency, new AbstractUnit[] { Unitless.unitless }, true, 0, 1),
                new SimpleParameter((int)Field.turbineEff, LibraryResources.TurbineEfficiency, new AbstractUnit[] { Unitless.unitless }, true, 0, 1),
                new SimpleParameter((int)Field.powerReq, LibraryResources.PowerRequirement, new AbstractUnit[] { Power.kW }, true, 0),
                new SimpleParameter((int)Field.condenserSQ, LibraryResources.CondenserSQ, new AbstractUnit[] { Unitless.unitless }, false, 0, 1),
                new SimpleParameter((int)Field.pumpQ, LibraryResources.PumpWork, new AbstractUnit[] { Enthalpy.kJkg }, false),
                new SimpleParameter((int)Field.boilerQ, LibraryResources.BoilerWork, new AbstractUnit[] { Enthalpy.kJkg }, false),
                new SimpleParameter((int)Field.condenserQ, LibraryResources.CondenserWork, new AbstractUnit[] { Enthalpy.kJkg }, false),
                new SimpleParameter((int)Field.turbineQ, LibraryResources.TurbineWork, new AbstractUnit[] { Enthalpy.kJkg }, false),
                new SimpleParameter((int)Field.thermoEff, LibraryResources.ThermalEfficiency, new AbstractUnit[] { Unitless.unitless }, false),
                new SimpleParameter((int)Field.netQ, LibraryResources.NetWork, new AbstractUnit[] { Enthalpy.kJkg }, false),
                new SimpleParameter((int)Field.steamRate, LibraryResources.SteamRate, new AbstractUnit[] { Mass.kg, Time.sec }, false),
                new SimpleParameter((int)Field.boilerHeatTrans, LibraryResources.BoilerHeatTransRate, new AbstractUnit[] { Energy.kJ, Time.sec }, false),
                new SimpleParameter((int)Field.condenserHeatTrans, LibraryResources.CondenserHeatTransRate, new AbstractUnit[] { Energy.kJ, Time.sec }, false),
            };
        }
    }
}
