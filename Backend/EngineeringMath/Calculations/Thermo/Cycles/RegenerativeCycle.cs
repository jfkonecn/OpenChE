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
            ThermoEntry boilerConditions = Table.GetThermoEntryAtTemperatureAndPressure(BoilerTemperature, SteamPressure),
                condenserLiquidConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure, ThermoEntry.Phase.liquid),
                condenserVaporConditions = Table.GetThermoEntryAtSatPressure(CondenserPressure, ThermoEntry.Phase.vapor);

            // get floor of saturation temperature to ensure liquid state
            double InletPumpTemperature = Math.Floor(condenserLiquidConditions.Temperature);

            double OutletPumpTemperature = Math.Floor(condenserLiquidConditions.Temperature);

            CondenserSteamQuality = (boilerConditions.S - condenserLiquidConditions.S)
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            // in kj / kg
            double condenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality * (condenserVaporConditions.H - condenserLiquidConditions.H);

            PumpWork = ((condenserLiquidConditions.V * (SteamPressure - CondenserPressure)) * 1e-6) / PumpEfficiency;

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
            protected double Pressure { get; set; }

            protected double Temperature { get; set; }
        }
    }
}
