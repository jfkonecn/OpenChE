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
        public RankineCycle(ThermoTable table)
        {
            BoilerPressure = new SimpleParameter((int)Field.boilerP, LibraryResources.BoilerPressure, new AbstractUnit[] { Pressure.Pa }, true, table.MinTablePressure, table.MaxTablePressure);
            BoilerTemperature = new SimpleParameter((int)Field.boilerTemp, LibraryResources.BoilerTemp, new AbstractUnit[] { Temperature.C }, true, table.MinTableTemperature, table.MaxTableTemperature);
            CondenserPressure = new SimpleParameter((int)Field.condenserP, LibraryResources.CondenserPressure, new AbstractUnit[] { Pressure.Pa }, true, table.MinTablePressure, table.MaxTablePressure);
            PumpEfficiency = new SimpleParameter((int)Field.pumpEff, LibraryResources.PumpEfficiency, new AbstractUnit[] { Unitless.unitless }, true, 0, 1);
            TurbineEfficiency = new SimpleParameter((int)Field.turbineEff, LibraryResources.TurbineEfficiency, new AbstractUnit[] { Unitless.unitless }, true, 0, 1);
            PowerRequirement = new SimpleParameter((int)Field.powerReq, LibraryResources.PowerRequirement, new AbstractUnit[] { Power.kW }, true, 0);
            CondenserSteamQuality = new SimpleParameter((int)Field.condenserSQ, LibraryResources.CondenserSQ, new AbstractUnit[] { Unitless.unitless }, false, 0, 1);
            PumpWork = new SimpleParameter((int)Field.pumpQ, LibraryResources.PumpWork, new AbstractUnit[] { Enthalpy.kJkg }, false);
            BoilerWork = new SimpleParameter((int)Field.boilerQ, LibraryResources.BoilerWork, new AbstractUnit[] { Enthalpy.kJkg }, false);
            CondenserWork = new SimpleParameter((int)Field.condenserQ, LibraryResources.CondenserWork, new AbstractUnit[] { Enthalpy.kJkg }, false);
            TurbineWork = new SimpleParameter((int)Field.turbineQ, LibraryResources.TurbineWork, new AbstractUnit[] { Enthalpy.kJkg }, false);
            ThermalEfficiency = new SimpleParameter((int)Field.thermoEff, LibraryResources.ThermalEfficiency, new AbstractUnit[] { Unitless.unitless }, false);
            NetWork = new SimpleParameter((int)Field.netQ, LibraryResources.NetWork, new AbstractUnit[] { Enthalpy.kJkg }, false);
            SteamRate = new SimpleParameter((int)Field.steamRate, LibraryResources.SteamRate, new AbstractUnit[] { Mass.kg, Time.sec }, false);
            BoilerHeatTransRate = new SimpleParameter((int)Field.boilerHeatTrans, LibraryResources.BoilerHeatTransRate, new AbstractUnit[] { Enthalpy.kJkg }, false);
            CondenserHeatTransRate = new SimpleParameter((int)Field.condenserHeatTrans, LibraryResources.CondenserHeatTransRate, new AbstractUnit[] { Enthalpy.kJkg }, false);



            this.Title = LibraryResources.RankineCycle;
            this.Table = table;

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
        public readonly SimpleParameter PowerRequirement;


        /// <summary>
        /// Boiler Pressure (Pa)
        /// </summary>
        public readonly SimpleParameter BoilerPressure;

        /// <summary>
        /// Boiler Temperature (C)
        /// </summary>
        public readonly SimpleParameter BoilerTemperature;

        /// <summary>
        /// Steam Mass Flow Rate in the system (kg/s)
        /// </summary>
        public readonly SimpleParameter SteamRate;

        /// <summary>
        /// Condenser Pressure (Pa)
        /// </summary>
        public readonly SimpleParameter CondenserPressure;

        /// <summary>
        /// Condenser Steam Quality (Unitless)
        /// </summary>
        public readonly SimpleParameter CondenserSteamQuality;

        /// <summary>
        /// Condenser Work (kJ/kg)
        /// </summary>
        public readonly SimpleParameter CondenserWork;

        /// <summary>
        /// Condenser Heat Transfer Rate (kJ/s)
        /// </summary>
        public readonly SimpleParameter CondenserHeatTransRate;

        /// <summary>
        /// Turbine Work (kJ/kg)
        /// Negative number means turbine is consuming energy
        /// </summary>
        public readonly SimpleParameter TurbineWork;

        /// <summary>
        /// Pump Work (kJ/kg)
        /// </summary>
        public readonly SimpleParameter PumpWork;

        /// <summary>
        /// Boiler Work (kJ/kg)
        /// </summary>
        public readonly SimpleParameter BoilerWork;


        /// <summary>
        /// Boiler Heat Transfer Rate (kJ/s)
        /// </summary>
        public readonly SimpleParameter BoilerHeatTransRate;

        /// <summary>
        /// Net Work (kJ/kg)
        /// </summary>
        public readonly SimpleParameter NetWork;

        /// <summary>
        /// Turbine Efficiency (Unitless)
        /// </summary>
        public readonly SimpleParameter TurbineEfficiency;

        /// <summary>
        /// Pump Efficiency (Unitless)
        /// </summary>
        public readonly SimpleParameter PumpEfficiency;

        /// <summary>
        /// Thermal Efficiency (Unitless)
        /// </summary>
        public readonly SimpleParameter ThermalEfficiency;

        /// <summary>
        /// The thermo table being used in this function
        /// </summary>
        protected readonly ThermoTable Table;

        protected override void Calculation()
        {
            ThermoEntry boilerConditions = Table.GetThermoEntryAtTemperatureAndPressure(BoilerTemperature.Value, BoilerPressure.Value),
                condenserLiquidConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure.Value, ThermoEntry.Phase.liquid),
                condenserVaporConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure.Value, ThermoEntry.Phase.vapor);

            CondenserSteamQuality.Value = (boilerConditions.S - condenserLiquidConditions.S) 
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            // in kj / kg
            double condenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality.Value * (condenserVaporConditions.H - condenserLiquidConditions.H);            

            PumpWork.Value = ((condenserLiquidConditions.V * (BoilerPressure.Value - CondenserPressure.Value)) * 1e-3) / PumpEfficiency.Value;

            // in kj / kg
            double boilerEnthalpy = condenserLiquidConditions.H + PumpWork.Value;

            BoilerWork.Value = boilerConditions.H - boilerEnthalpy;

            TurbineWork.Value = -(condenserEnthalpy - boilerConditions.H) * TurbineEfficiency.Value;

            CondenserWork.Value = -(condenserLiquidConditions.H - (boilerConditions.H - TurbineWork.Value));

            NetWork.Value =  TurbineWork.Value - PumpWork.Value;

            ThermalEfficiency.Value = Math.Abs(TurbineWork.Value) / BoilerWork.Value;

            SteamRate.Value = PowerRequirement.Value / NetWork.Value;

            BoilerHeatTransRate.Value = SteamRate.Value * BoilerWork.Value;

            CondenserHeatTransRate.Value = SteamRate.Value * CondenserWork.Value;
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.boilerP:
                    return BoilerPressure;
                case Field.boilerTemp:
                    return BoilerTemperature;
                case Field.steamRate:
                    return SteamRate;
                case Field.condenserP:
                    return CondenserPressure;
                case Field.condenserSQ:
                    return CondenserSteamQuality;
                case Field.condenserQ:
                    return CondenserWork;
                case Field.condenserHeatTrans:
                    return CondenserHeatTransRate;
                case Field.turbineQ:
                    return TurbineWork;
                case Field.turbineEff:
                    return TurbineEfficiency;
                case Field.powerReq:
                    return PowerRequirement;
                case Field.pumpEff:
                    return PumpEfficiency;
                case Field.pumpQ:
                    return PumpWork;
                case Field.boilerQ:
                    return BoilerWork;
                case Field.boilerHeatTrans:
                    return BoilerHeatTransRate;
                case Field.netQ:
                    return NetWork;
                case Field.thermoEff:
                    return ThermalEfficiency;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return BoilerPressure;
            yield return BoilerTemperature;
            yield return CondenserPressure;
            yield return PumpEfficiency;
            yield return TurbineEfficiency;
            yield return PowerRequirement;
            yield return CondenserSteamQuality;
            yield return PumpWork;
            yield return BoilerWork;
            yield return CondenserWork;
            yield return TurbineWork;
            yield return ThermalEfficiency;
            yield return NetWork;
            yield return SteamRate;
            yield return BoilerHeatTransRate;
            yield return CondenserHeatTransRate;
        }
    }
}
