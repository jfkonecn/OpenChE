using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Resources;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;

namespace EngineeringMath.Calculations.Thermo.Cycles
{
    public class RegenerativeCycle : RankineCycle
    {
        public RegenerativeCycle() : base()
        {
            this.Title = LibraryResources.RegenerativeCycle;
            RegenerationStagesSelector.OnSelectedIndexChanged += RegenerationStagesSelector_OnSelectedIndexChanged;
            RegenerationStagesSelector.SelectedIndex = 0;
        }

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
            ThermoEntry boilerConditions = Table.GetThermoEntryAtTemperatureAndPressure(BoilerTemperature, BoilerPressure),
                condenserLiquidConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure, ThermoEntry.Phase.liquid),
                condenserVaporConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure, ThermoEntry.Phase.vapor),
                inletBoilerConditions = Table.GetThermoEntryAtSatPressure(BoilerPressure, ThermoEntry.Phase.liquid);

            if(boilerConditions.EntryPhase != ThermoEntry.Phase.vapor)
            {
                GetParameter((int)Field.boilerP).OnError(new Exception(LibraryResources.BoilerNotVapor));
                return;
            }

            // get floor of saturation temperature to ensure liquid state
            double InletPumpTemperature = Math.Floor(condenserLiquidConditions.Temperature);
            ThermoEntry pumpConditions = Table.GetThermoEntryAtSatTemp(InletPumpTemperature, ThermoEntry.Phase.liquid);
            double OutletPumpTemperature = 
                // pump enthalpy
                (((pumpConditions.V * (boilerConditions.Pressure - pumpConditions.Pressure)) / PumpEfficiency)
                - (pumpConditions.V * (1 - pumpConditions.Beta * (pumpConditions.Temperature + 273.15)))) 
                / pumpConditions.Cp;

            double stageTempStep = (inletBoilerConditions.Temperature - OutletPumpTemperature) / StageStates.Length,
                curInletTemp = inletBoilerConditions.Temperature - stageTempStep,
                // the smaller different in temperature between the saturated steam and the cooling liquid
                HeatExchangerTempBuffer = 5;

            StageStates[0] = new RegenerativeStage(
                Table.GetThermoEntryAtEntropyAndPressure(boilerConditions.S, 
                inletBoilerConditions.Temperature + stageTempStep + HeatExchangerTempBuffer));


            for (int i = 1; i < StageStates.Length; i++)
            {

                Table.GetThermoEntryAtEntropyAndPressure(boilerConditions.S, inletBoilerConditions.Temperature + stageTempStep + HeatExchangerTempBuffer);
                StageStates[i] = new RegenerativeStage(
                    Table.GetThermoEntryAtTemperatureAndPressure(curInletTemp, BoilerPressure));
                curInletTemp -= stageTempStep;
            }

            CondenserSteamQuality = (boilerConditions.S - condenserLiquidConditions.S)
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            // in kj / kg
            double condenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality * (condenserVaporConditions.H - condenserLiquidConditions.H);

            PumpWork = ((condenserLiquidConditions.V * (BoilerPressure - CondenserPressure)) * 1e-6) / PumpEfficiency;

            // in kj / kg
            double boilerEnthalpy = condenserLiquidConditions.H + PumpWork;

            BoilerWork = boilerConditions.H - boilerEnthalpy;

            TurbineWork = -(condenserEnthalpy - boilerConditions.H) * TurbineEfficiency;

            CondenserWork = -(condenserLiquidConditions.H - (boilerConditions.H - TurbineWork));

            NetWork = TurbineWork - PumpWork;

            ThermalEfficiency = Math.Abs(TurbineWork) / BoilerWork;

            SteamRate = PowerRequirement / NetWork;

            BoilerHeatTransRate = SteamRate * BoilerWork;

            CondenserHeatTransRate = SteamRate * CondenserWork;
        }

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

        protected class RegenerativeStage
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="inletVaporFlowRate">The mass flow rate of vapor entering the stage (kg/s)</param>
            /// <param name="inletVaporConditions">The conditions of the vapor entering the stage</param>
            /// <param name="inletCoolingLiquidFlowRate">The mass flow rate of cooling liquid entering the stage (kg/s)</param>
            /// <param name="inletCoolingLiquidConditions">The conditions of the liquid entering the stage</param>
            /// <param name="inletCondensateFlowRate">The mass flow rate of condenstate entering this stage (kg/s)</param>
            /// <param name="inletCondensateConditions">The conditions of the condenstate entering this stage</param>
            internal RegenerativeStage(
                double inletVaporFlowRate,
                ThermoEntry inletVaporConditions,
                double inletCoolingLiquidFlowRate,
                ThermoEntry inletCoolingLiquidConditions,
                double inletCondensateFlowRate,
                ThermoEntry inletCondensateConditions)
            {

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="inletVaporFlowRate">The mass flow rate of vapor entering the stage (kg/s)</param>
            /// <param name="inletVaporConditions">The conditions of the vapor entering the stage</param>
            /// <param name="inletCoolingLiquidFlowRate">The mass flow rate of cooling liquid entering the stage (kg/s)</param>
            /// <param name="inletCoolingLiquidConditions">The conditions of the liquid entering the stage</param>
            /// <param name="deltaTCoolingLiquid">The change in the temperature between incoming and outgoing cooling liquid in C</param>
            /// <param name="turbineEff">Efficiency as fraction between 0 and 1</param>
            /// <param name="table">thermo table reference table to be used</param>
            /// <param name="HeatExchangerTempBuffer">Smallest allowed difference in temperature between cooling water and condensate leaving stage in C</param>
            internal RegenerativeStage(
                double inletVaporFlowRate,
                ThermoEntry inletVaporConditions,
                double inletCoolingLiquidFlowRate,
                ThermoEntry inletCoolingLiquidConditions,
                double deltaTCoolingLiquid,
                double turbineEff,
                ref Resources.LookupTables.ThermoTable table,
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

                ExitCoolingLiquidConditions = table.GetThermoEntryAtTemperatureAndPressure(
                    inletCoolingLiquidConditions.Temperature + deltaTCoolingLiquid, 
                    inletCoolingLiquidConditions.Pressure);

                ExitCondensateConditions = table.GetThermoEntryAtSatTemp(
                    ExitCoolingLiquidConditions.Temperature + heatExchangerTempBuffer, ThermoEntry.Phase.liquid);

                WorkProduced = (table.GetThermoEntryAtEntropyAndPressure(
                            inletVaporConditions.S, ExitCondensateConditions.Pressure).H
                            - inletVaporConditions.H) * turbineEff;

                ExitVaporConditions =
                    table.GetThermoEntryAtEnthapyAndPressure(
                    inletVaporConditions.H + WorkProduced,
                        ExitCondensateConditions.Pressure);

                double condensateFraction = 
                    (ExitCoolingLiquidConditions.H - inletCoolingLiquidConditions.H)
                    / (ExitVaporConditions.H - ExitCondensateConditions.H);

                VaporExtracted = inletVaporFlowRate * (1 - condensateFraction);
                CondensateExtracted = inletVaporFlowRate * condensateFraction;
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
            protected double VaporExtracted { get; private set; }
            /// <summary>
            /// Condensate leaving the stage
            /// </summary>
            protected ThermoEntry ExitCondensateConditions { get; private set; }
            /// <summary>
            /// kg/s of condensate extracted
            /// </summary>
            protected double CondensateExtracted { get; private set; }

            /// <summary>
            /// The total worked per mass flow rate of vapor produced by the the stage in kj/kg
            /// <para>Note that a positive number means energy was produced</para>
            /// </summary>
            protected double WorkProduced { get; private set; }
        }
    }
}
