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

namespace EngineeringMath.Calculations.Thermo
{
    public class RankineCycle : SimpleFunction
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public RankineCycle() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.steamP, LibraryResources.SteamPressure, new AbstractUnit[] { Pressure.Pa }, true, SteamTable.Table.MinTablePressure, SteamTable.Table.MaxTablePressure),
                    new SimpleParameter((int)Field.steamTemp, LibraryResources.SteamTemp, new AbstractUnit[] { Temperature.C }, true, SteamTable.Table.MinTableTemperature, SteamTable.Table.MaxTableTemperature),
                    new SimpleParameter((int)Field.condenserP, LibraryResources.CondenserPressure, new AbstractUnit[] { Pressure.Pa}, true, SteamTable.Table.MinTablePressure, SteamTable.Table.MaxTablePressure),
                    new SimpleParameter((int)Field.pumpEff, LibraryResources.PumpEfficiency, new AbstractUnit[] { Unitless.unitless}, true, 0, 1),
                    new SimpleParameter((int)Field.turbineEff, LibraryResources.TurbineEfficiency, new AbstractUnit[] { Unitless.unitless}, true, 0, 1),
                    new SimpleParameter((int)Field.powerReq, LibraryResources.PowerRequirement, new AbstractUnit[] { Power.kW }, true, 0),
                    new SimpleParameter((int)Field.condenserSQ, LibraryResources.CondenserSQ, new AbstractUnit[] { Unitless.unitless}, false, 0, 1),
                    new SimpleParameter((int)Field.pumpQ, LibraryResources.PumpWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.boilerQ, LibraryResources.BoilerWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.condenserQ, LibraryResources.CondenserWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.turbineQ, LibraryResources.TurbineWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.thermoEff, LibraryResources.ThermalEfficiency, new AbstractUnit[] { Unitless.unitless}, false),
                    new SimpleParameter((int)Field.netQ, LibraryResources.NetWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.steamRate, LibraryResources.SteamRate, new AbstractUnit[] { Mass.kg, Time.sec }, false),
                    new SimpleParameter((int)Field.boilerHeatTrans, LibraryResources.BoilerHeatTransRate, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.condenserHeatTrans, LibraryResources.CondenserHeatTransRate, new AbstractUnit[] { Enthalpy.kJkg}, false)
                }
            )
        {
            this.Title = LibraryResources.RankineCycle;
            

#if DEBUG
            SteamPressure = 8600e3;
            SteamTemperature = 500;
            CondenserPressure = 10e3;
            PumpEfficiency = 0.75;
            TurbineEfficiency = 0.75;
            PowerRequirement = 80e3;
#endif

        }

        public enum Field
        {
            /// <summary>
            /// Steam Pressure (Pa)
            /// </summary>
            steamP,
            /// <summary>
            /// Steam Temperature (C)
            /// </summary>
            steamTemp,
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
        public double PowerRequirement
        {
            get
            {
                return GetParameter((int)Field.powerReq).Value;
            }

            set
            {
                GetParameter((int)Field.powerReq).Value = value;
            }
        }


        /// <summary>
        /// Steam Pressure (Pa)
        /// </summary>
        public double SteamPressure
        {
            get
            {
                return GetParameter((int)Field.steamP).Value;
            }

            set
            {
                GetParameter((int)Field.steamP).Value = value;
            }
        }

        /// <summary>
        /// Steam Temperature (C)
        /// </summary>
        public double SteamTemperature
        {
            get
            {
                return GetParameter((int)Field.steamTemp).Value;
            }

            set
            {
                GetParameter((int)Field.steamTemp).Value = value;
            }
        }

        /// <summary>
        /// Steam Mass Flow Rate in the system (kg/s)
        /// </summary>
        public double SteamRate
        {
            get
            {
                return GetParameter((int)Field.steamRate).Value;
            }

            set
            {
                GetParameter((int)Field.steamRate).Value = value;
            }
        }

        /// <summary>
        /// Condenser Pressure (Pa)
        /// </summary>
        public double CondenserPressure
        {
            get
            {
                return GetParameter((int)Field.condenserP).Value;
            }

            set
            {
                GetParameter((int)Field.condenserP).Value = value;
            }
        }

        /// <summary>
        /// Condenser Steam Quality (Unitless)
        /// </summary>
        public double CondenserSteamQuality
        {
            get
            {
                return GetParameter((int)Field.condenserSQ).Value;
            }

            set
            {
                GetParameter((int)Field.condenserSQ).Value = value;
            }
        }

        /// <summary>
        /// Condenser Work (kJ/kg)
        /// </summary>
        public double CondenserWork
        {
            get
            {
                return GetParameter((int)Field.condenserQ).Value;
            }

            set
            {
                GetParameter((int)Field.condenserQ).Value = value;
            }
        }

        /// <summary>
        /// Condenser Heat Transfer Rate (kJ/s)
        /// </summary>
        public double CondenserHeatTransRate
        {
            get
            {
                return GetParameter((int)Field.condenserHeatTrans).Value;
            }

            set
            {
                GetParameter((int)Field.condenserHeatTrans).Value = value;
            }
        }

        /// <summary>
        /// Turbine Work (kJ/kg)
        /// Negative number means turbine is consuming energy
        /// </summary>
        public double TurbineWork
        {
            get
            {
                return GetParameter((int)Field.turbineQ).Value;
            }

            set
            {
                GetParameter((int)Field.turbineQ).Value = value;
            }
        }

        /// <summary>
        /// Pump Work (kJ/kg)
        /// </summary>
        public double PumpWork
        {
            get
            {
                return GetParameter((int)Field.pumpQ).Value;
            }

            set
            {
                GetParameter((int)Field.pumpQ).Value = value;
            }
        }

        /// <summary>
        /// Boiler Work (kJ/kg)
        /// </summary>
        public double BoilerWork
        {
            get
            {
                return GetParameter((int)Field.boilerQ).Value;
            }

            set
            {
                GetParameter((int)Field.boilerQ).Value = value;
            }
        }


        /// <summary>
        /// Boiler Heat Transfer Rate (kJ/s)
        /// </summary>
        public double BoilerHeatTransRate
        {
            get
            {
                return GetParameter((int)Field.boilerHeatTrans).Value;
            }

            set
            {
                GetParameter((int)Field.boilerHeatTrans).Value = value;
            }
        }

        /// <summary>
        /// Net Work (kJ/kg)
        /// </summary>
        public double NetWork
        {
            get
            {
                return GetParameter((int)Field.netQ).Value;
            }

            set
            {
                GetParameter((int)Field.netQ).Value = value;
            }
        }

        /// <summary>
        /// Turbine Efficiency (Unitless)
        /// </summary>
        public double TurbineEfficiency
        {
            get
            {
                return GetParameter((int)Field.turbineEff).Value;
            }

            set
            {
                GetParameter((int)Field.turbineEff).Value = value;
            }
        }

        /// <summary>
        /// Pump Efficiency (Unitless)
        /// </summary>
        public double PumpEfficiency
        {
            get
            {
                return GetParameter((int)Field.pumpEff).Value;
            }

            set
            {
                GetParameter((int)Field.pumpEff).Value = value;
            }
        }

        /// <summary>
        /// Thermal Efficiency (Unitless)
        /// </summary>
        public double ThermalEfficiency
        {
            get
            {
                return GetParameter((int)Field.thermoEff).Value;
            }

            set
            {
                GetParameter((int)Field.thermoEff).Value = value;
            }
        }

        protected override void Calculation()
        {
            ThermoEntry steamConditions = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(SteamTemperature, SteamPressure),
                condenserLiquidConditions = SteamTable.Table.GetThermoEntrySatLiquidAtPressure(CondenserPressure),
                condenserVaporConditions = SteamTable.Table.GetThermoEntrySatVaporAtPressure(CondenserPressure);

            CondenserSteamQuality = (steamConditions.S - condenserLiquidConditions.S) 
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            // in kj / kg
            double condenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality * (condenserVaporConditions.H - condenserLiquidConditions.H);            

            PumpWork = ((condenserLiquidConditions.V * (SteamPressure - CondenserPressure)) * 1e-6) / PumpEfficiency;

            // in kj / kg
            double boilerEnthalpy = condenserLiquidConditions.H + PumpWork;

            BoilerWork = steamConditions.H - boilerEnthalpy;

            TurbineWork = -(condenserEnthalpy - steamConditions.H) * TurbineEfficiency;

            CondenserWork = -(condenserLiquidConditions.H - (steamConditions.H - TurbineWork));

            NetWork =  TurbineWork - PumpWork;

            ThermalEfficiency = Math.Abs(TurbineWork) / BoilerWork;

            SteamRate = PowerRequirement / NetWork;

            BoilerHeatTransRate = SteamRate * BoilerWork;

            CondenserHeatTransRate = SteamRate * CondenserWork;
        }
    }
}
