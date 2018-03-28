using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Resources;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.Thermo.Cycles
{
    public class RegenerativeCycle : RankineCycle
    {
        public RegenerativeCycle() : base()
        {
            this.Title = LibraryResources.RegenerativeCycle;
            RegenerationStagesSelector.OnSelectedIndexChanged += RegenerationStagesSelector_OnSelectedIndexChanged;
            RegenerationStagesSelector.SelectedIndex = 0;
            InletBoilerTemperature = new SimpleParameter((int)Field.inletBoilerTemp, LibraryResources.InletBoilerTemperature, new AbstractUnit[] { Temperature.C }, true);
            SteamRate.isInput = true;
        }


        new public enum Field
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
            thermoEff,
            /// <summary>
            /// The temperature inlet boiler temperature (C)
            /// </summary>
            inletBoilerTemp
        };

        private void RegenerationStagesSelector_OnSelectedIndexChanged()
        {
            StageStates = new RegenerativeStage[RegenerationStagesSelector.SelectedObject];
        }

        private RegenerativeStage[] StageStates;

        private static readonly UInt16 MIN_STAGES = 1;
        private static readonly UInt16 MAX_STAGES = 10;

        /// <summary>
        /// The spinner which has the total number of Regeneration Stages
        /// </summary>
        public IntegerSpinner RegenerationStagesSelector = new IntegerSpinner(MIN_STAGES, MAX_STAGES, LibraryResources.RegenerationStages);


        protected override void Calculation()
        {
            ThermoEntry boilerConditions = Table.GetThermoEntryAtTemperatureAndPressure(BoilerTemperature.Value, BoilerPressure.Value),
                condenserLiquidConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure.Value, ThermoEntry.Phase.liquid),
                condenserVaporConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure.Value, ThermoEntry.Phase.vapor),
                inletBoilerConditions = Table.GetThermoEntryAtSatTemp(InletBoilerTemperature.Value, ThermoEntry.Phase.liquid);

            if(boilerConditions.EntryPhase != ThermoEntry.Phase.vapor)
            {
                GetParameter((int)Field.boilerP).OnError(new Exception(LibraryResources.BoilerNotVapor));
                return;
            }

            double inletPumpTemperature = condenserLiquidConditions.Temperature;
            ThermoEntry pumpConditions = Table.GetThermoEntryAtSatTemp(inletPumpTemperature, ThermoEntry.Phase.liquid);
            // in C
            double outletPumpTemperature = 
                // pump enthalpy
                (((pumpConditions.V * (boilerConditions.Pressure - pumpConditions.Pressure) * 1e-3) / PumpEfficiency.Value)
                - (pumpConditions.V * (1 - pumpConditions.Beta * (pumpConditions.Temperature + 273.15)))) 
                / pumpConditions.Cp + pumpConditions.Temperature;

            double stageTempStep = (inletBoilerConditions.Temperature - outletPumpTemperature) / (StageStates.Length - 1),
                curInletTemp = inletBoilerConditions.Temperature - stageTempStep,
                // the smallest different in temperature between the saturated steam and the cooling liquid
                heatExchangerTempBuffer = 5;

            StageStates[0] = new RegenerativeStage(
                SteamRate.Value, boilerConditions,
                SteamRate.Value, Table.GetThermoEntryAtTemperatureAndPressure(
                    inletBoilerConditions.Temperature - heatExchangerTempBuffer, inletBoilerConditions.Pressure),
                stageTempStep, TurbineEfficiency.Value, Table, heatExchangerTempBuffer
                );

            for (int i = 1; i < StageStates.Length; i++)
            {
                StageStates[i] = new RegenerativeStage(StageStates[i - 1], Table);
                curInletTemp -= stageTempStep;
            }

            CondenserSteamQuality.Value = (boilerConditions.S - condenserLiquidConditions.S)
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            // in kj / kg
            double condenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality.Value * (condenserVaporConditions.H - condenserLiquidConditions.H);

            PumpWork.Value = ((condenserLiquidConditions.V * (BoilerPressure.Value - CondenserPressure.Value)) * 1e-6) / PumpEfficiency.Value;

            // in kj / kg
            double boilerEnthalpy = condenserLiquidConditions.H + PumpWork.Value;

            BoilerWork.Value = boilerConditions.H - boilerEnthalpy;

            TurbineWork.Value = -(condenserEnthalpy - boilerConditions.H) * TurbineEfficiency.Value;

            CondenserWork.Value = -(condenserLiquidConditions.H - (boilerConditions.H - TurbineWork.Value));

            NetWork.Value = TurbineWork.Value - PumpWork.Value;

            ThermalEfficiency.Value = Math.Abs(TurbineWork.Value) / BoilerWork.Value;

            SteamRate.Value = PowerRequirement.Value / NetWork.Value;

            BoilerHeatTransRate.Value = SteamRate.Value * BoilerWork.Value;

            CondenserHeatTransRate.Value = SteamRate.Value * CondenserWork.Value;
        }

        public readonly SimpleParameter InletBoilerTemperature;

        /// <summary>
        /// The total number of Regeneration Stages
        /// </summary>
        public int TotalRegenerationStages
        {
            get { return RegenerationStagesSelector.SelectedObject; }
            set { RegenerationStagesSelector.SelectedObject = value; }
        }


        public override IEnumerator GetEnumerator()
        {
            yield return RegenerationStagesSelector;
            foreach (AbstractComponent obj in ParameterCollection())
            {
                yield return obj;
            }
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
                case Field.inletBoilerTemp:
                    return InletBoilerTemperature;
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
            yield return SteamRate;
            yield return InletBoilerTemperature;
            yield return CondenserSteamQuality;
            yield return PumpWork;
            yield return BoilerWork;
            yield return CondenserWork;
            yield return TurbineWork;
            yield return ThermalEfficiency;
            yield return NetWork;            
            yield return BoilerHeatTransRate;
            yield return CondenserHeatTransRate;
        }


        protected class RegenerativeStage
        {
            /// <summary>
            /// Uses previous stage to construct this stage
            /// </summary>
            /// <param name="previousStage"></param>
            /// <param name="table">thermo table reference table to be used</param>
            internal RegenerativeStage(RegenerativeStage previousStage,
                Resources.LookupTables.ThermoTable table) :
                this
                (
                    previousStage.ExitVaporFlowRate, previousStage.ExitVaporConditions,
                    previousStage.CoolingLiquidFlowRate, table.GetThermoEntryAtTemperatureAndPressure(
                    previousStage.ExitCoolingLiquidConditions.Temperature - previousStage.DeltaTCoolingLiquid * 2,
                    previousStage.ExitCoolingLiquidConditions.Pressure),
                    previousStage.ExitCondensateFlowRate, previousStage.ExitCondensateConditions,
                    previousStage.DeltaTCoolingLiquid, previousStage.TurbineEfficiency,
                    table, previousStage.HeatExchangerTempBuffer
                )
            {

            }

            /// <summary>
            /// First Stage constructor
            /// </summary>
            /// <param name="inletVaporFlowRate">The mass flow rate of vapor entering the stage (kg/s)</param>
            /// <param name="inletVaporConditions">The conditions of the vapor entering the stage</param>
            /// <param name="inletCoolingLiquidFlowRate">The mass flow rate of cooling liquid entering the stage (kg/s)</param>
            /// <param name="inletCoolingLiquidConditions">The conditions of the liquid entering the stage</param>
            /// <param name="deltaTCoolingLiquid">The change in the temperature between incoming and outgoing cooling liquid in C</param>
            /// <param name="turbineEff">Efficiency as fraction between 0 and 1</param>
            /// <param name="table">thermo table reference table to be used</param>
            /// <param name="heatExchangerTempBuffer">Smallest allowed difference in temperature between cooling water and condensate leaving stage in C</param>
            internal RegenerativeStage(
                double inletVaporFlowRate,
                ThermoEntry inletVaporConditions,
                double inletCoolingLiquidFlowRate,
                ThermoEntry inletCoolingLiquidConditions,
                double deltaTCoolingLiquid,
                double turbineEff,
                Resources.LookupTables.ThermoTable table,
                double heatExchangerTempBuffer
                ) : this(
                 inletVaporFlowRate,
                 inletVaporConditions,
                 inletCoolingLiquidFlowRate,
                 inletCoolingLiquidConditions,
                 0,
                 null,
                 deltaTCoolingLiquid,
                 turbineEff,
                table,
                 heatExchangerTempBuffer
                    )
            {
  
            }




            /// <summary>
            /// 
            /// </summary>
            /// <param name="inletVaporFlowRate">The mass flow rate of vapor entering the stage (kg/s)</param>
            /// <param name="inletVaporConditions">The conditions of the vapor entering the stage</param>
            /// <param name="inletCoolingLiquidFlowRate">The mass flow rate of cooling liquid entering the stage (kg/s)</param>
            /// <param name="inletCoolingLiquidConditions">The conditions of the liquid entering the stage</param>
            /// <param name="inletCondensateFlowRate">The mass flow rate of condensate liquid entering the stage (kg/s)</param>
            /// <param name="inletCondensateConditions">The conditions of the condensate liquid entering the stage allowed to be null if the mass flow rate is zero</param>
            /// <param name="deltaTCoolingLiquid">The change in the temperature between incoming and outgoing cooling liquid in C</param>
            /// <param name="turbineEfficiency">Efficiency as fraction between 0 and 1</param>
            /// <param name="table">thermo table reference table to be used</param>
            /// <param name="heatExchangerTempBuffer">Smallest allowed difference in temperature between cooling water and condensate leaving stage in C</param>
            private RegenerativeStage(
                double inletVaporFlowRate,
                ThermoEntry inletVaporConditions,
                double inletCoolingLiquidFlowRate,
                ThermoEntry inletCoolingLiquidConditions,
                double inletCondensateFlowRate,
                ThermoEntry inletCondensateConditions,
                double deltaTCoolingLiquid,
                double turbineEfficiency,
                Resources.LookupTables.ThermoTable table,
                double heatExchangerTempBuffer
                )
            {
                if(heatExchangerTempBuffer <= 0)
                {
                    throw new Exception("Buffer cannot be less than or equal to zero!");
                }
                else if(deltaTCoolingLiquid <= 0)
                {
                    throw new Exception("The detaT cannot be less than or equal to zero!");
                }
                else if(inletCondensateConditions == null && inletCondensateFlowRate != 0)
                {
                    // if this is negative you need to rethink your life
                    throw new Exception("must have condensate conditions cannot be null if the flow rate is not 0");
                }

                CoolingLiquidFlowRate = inletCoolingLiquidFlowRate;
                TurbineEfficiency = turbineEfficiency;
                HeatExchangerTempBuffer = heatExchangerTempBuffer;

                ExitCoolingLiquidConditions = table.GetThermoEntryAtTemperatureAndPressure(
                    inletCoolingLiquidConditions.Temperature + deltaTCoolingLiquid, 
                    inletCoolingLiquidConditions.Pressure);

                ExitCondensateConditions = table.GetThermoEntryAtSatTemp(
                    ExitCoolingLiquidConditions.Temperature + heatExchangerTempBuffer, ThermoEntry.Phase.liquid);

                WorkProduced = (table.GetThermoEntryAtEntropyAndPressure(
                            inletVaporConditions.S, ExitCondensateConditions.Pressure).H
                            - inletVaporConditions.H) * turbineEfficiency;

                ExitVaporConditions =
                    table.GetThermoEntryAtEnthapyAndPressure(
                    inletVaporConditions.H + WorkProduced,
                        ExitCondensateConditions.Pressure);
                // in kg/s
                double condensateCreated;
                if (inletCondensateConditions == null)
                {
                    condensateCreated = 
                        (CoolingLiquidFlowRate * (ExitCoolingLiquidConditions.H - inletCoolingLiquidConditions.H)) 
                        /
                        (ExitVaporConditions.H - ExitCondensateConditions.H);
                }
                else
                {
                    condensateCreated = 
                        (inletCondensateFlowRate * (inletCondensateConditions.H - ExitCondensateConditions.H) 
                            - CoolingLiquidFlowRate * (ExitCoolingLiquidConditions.H - inletCoolingLiquidConditions.H)) 
                            / (ExitCondensateConditions.H - inletVaporConditions.H);
                }

                ExitVaporFlowRate = inletVaporFlowRate - condensateCreated;
                ExitCondensateFlowRate = condensateCreated + inletCondensateFlowRate;
            }

            /// <summary>
            /// Cooling Liquid Conditions leaving stage
            /// </summary>
            protected ThermoEntry ExitCoolingLiquidConditions { get; private set; }

            /// <summary>
            /// Vapor leaving the stage
            /// </summary>
            protected ThermoEntry ExitVaporConditions { get; private set; }

            /// <summary>
            /// kg/s of steam extracted from the stage
            /// </summary>
            protected double ExitVaporFlowRate { get; private set; }
            /// <summary>
            /// Condensate leaving the stage
            /// </summary>
            protected ThermoEntry ExitCondensateConditions { get; private set; }
            /// <summary>
            /// kg/s of condensate extracted
            /// </summary>
            protected double ExitCondensateFlowRate { get; private set; }

            /// <summary>
            /// The total worked per mass flow rate of vapor produced by the the stage in kj/kg
            /// <para>Note that a positive number means energy was produced</para>
            /// </summary>
            protected double WorkProduced { get; private set; }
            /// <summary>
            /// The mass flow rate of cooling liquid entering the stage (kg/s)
            /// </summary>
            private double CoolingLiquidFlowRate { get; set; }
            /// <summary>
            /// The change in the temperature between incoming and outgoing cooling liquid in C
            /// </summary>
            private double DeltaTCoolingLiquid { get; set; }
            /// <summary>
            /// Efficiency as fraction between 0 and 1
            /// </summary>
            private double TurbineEfficiency { get; set; }
            /// <summary>
            /// Smallest allowed difference in temperature between cooling water and condensate leaving stage in C
            /// </summary>
            private double HeatExchangerTempBuffer { get; set; }
        }
    }
}
