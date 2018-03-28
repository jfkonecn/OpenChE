using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Thermo;
using System;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Calculations.Thermo.Cycles;

namespace BackendTesting
{
    [TestClass]
    public class Thermo
    {

        /// <summary>
        /// Tests the rankine cycle
        /// </summary>
        [TestMethod]
        public void RankineCycleTest()
        {
            RankineCycle fun =
                (RankineCycle)
                FunctionFactory.BuildFunction(typeof(RankineCycle));

            // set all parameters to something completely wrong
            foreach (AbstractComponent obj in fun)
            {
                Type type = obj.CastAs();
                if (type.Equals(typeof(SimpleParameter)) || type.Equals(typeof(SubFunctionParameter)))
                {
                    ((SimpleParameter)obj).Value = double.NaN;
                }                
            }

            fun.GetParameter((int)RankineCycle.Field.boilerP).UnitSelection[0].SelectedObject = Pressure.kPa;
            fun.GetParameter((int)RankineCycle.Field.boilerP).ValueStr = "8600";

            fun.GetParameter((int)RankineCycle.Field.boilerTemp).UnitSelection[0].SelectedObject = Temperature.C;
            fun.GetParameter((int)RankineCycle.Field.boilerTemp).ValueStr = "500";

            fun.GetParameter((int)RankineCycle.Field.condenserP).UnitSelection[0].SelectedObject = Pressure.kPa;
            fun.GetParameter((int)RankineCycle.Field.condenserP).ValueStr = "10";

            fun.GetParameter((int)RankineCycle.Field.pumpEff).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)RankineCycle.Field.pumpEff).ValueStr = "0.75";

            fun.GetParameter((int)RankineCycle.Field.turbineEff).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)RankineCycle.Field.turbineEff).ValueStr = "0.75";

            fun.GetParameter((int)RankineCycle.Field.powerReq).UnitSelection[0].SelectedObject = Power.kW;
            fun.GetParameter((int)RankineCycle.Field.powerReq).ValueStr = "80e3";

            fun.Solve();
            double delta = 0.1;

            Assert.AreEqual(0.8051, fun.CondenserSteamQuality.Value, delta, "Steam Quality");
            Assert.AreEqual(11.6, fun.PumpWork.Value, delta, "Pump Work");
            Assert.AreEqual(3189, fun.BoilerWork.Value, delta, "Boiler Work");
            Assert.AreEqual(2245, fun.CondenserWork.Value, delta, "Condenser Work");
            Assert.AreEqual(955.9, fun.TurbineWork.Value, delta, "Turbine Work");
            Assert.AreEqual(0.2961, fun.ThermalEfficiency.Value, delta, "Thermal Efficiency");
            Assert.AreEqual(944.3, fun.NetWork.Value, delta, "Net Work");
            Assert.AreEqual(84.75, fun.SteamRate.Value, delta, "Steam Rate");
            Assert.AreEqual(270.2e3, fun.BoilerHeatTransRate.Value, delta, "Boiler Heat Transfer Rate");
            Assert.AreEqual(190.2e3, fun.CondenserHeatTransRate.Value, delta, "Condenser Heat Transfer Rate");
        }


        /// <summary>
        /// Tests the Regenerative Cycle function
        /// </summary>
        [TestMethod]
        public void RegenerativeCycleTest()
        {
            RegenerativeCycle fun =
                (RegenerativeCycle)
                FunctionFactory.BuildFunction(typeof(RegenerativeCycle));

            // set all parameters to something completely wrong
            foreach (AbstractComponent obj in fun)
            {
                Type type = obj.CastAs();
                if (type.Equals(typeof(SimpleParameter)) || type.Equals(typeof(SubFunctionParameter)))
                {
                    ((SimpleParameter)obj).Value = double.NaN;
                }
            }

            fun.GetParameter((int)RegenerativeCycle.Field.boilerP).UnitSelection[0].SelectedObject = Pressure.kPa;
            fun.GetParameter((int)RegenerativeCycle.Field.boilerP).ValueStr = "8600";

            fun.GetParameter((int)RegenerativeCycle.Field.boilerTemp).UnitSelection[0].SelectedObject = Temperature.C;
            fun.GetParameter((int)RegenerativeCycle.Field.boilerTemp).ValueStr = "500";

            fun.GetParameter((int)RegenerativeCycle.Field.condenserP).UnitSelection[0].SelectedObject = Pressure.kPa;
            fun.GetParameter((int)RegenerativeCycle.Field.condenserP).ValueStr = "10";

            fun.GetParameter((int)RegenerativeCycle.Field.pumpEff).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)RegenerativeCycle.Field.pumpEff).ValueStr = "0.75";

            fun.GetParameter((int)RegenerativeCycle.Field.turbineEff).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)RegenerativeCycle.Field.turbineEff).ValueStr = "0.75";

            fun.GetParameter((int)RegenerativeCycle.Field.powerReq).UnitSelection[0].SelectedObject = Power.kW;
            fun.GetParameter((int)RegenerativeCycle.Field.powerReq).ValueStr = "80e3";

            fun.GetParameter((int)RegenerativeCycle.Field.inletBoilerTemp).UnitSelection[0].SelectedObject = Temperature.C;
            fun.GetParameter((int)RegenerativeCycle.Field.inletBoilerTemp).ValueStr = "226";

            fun.TotalRegenerationStages = 5;

            fun.Solve();
            double delta = 0.1;

            Assert.AreEqual(0.8051, fun.CondenserSteamQuality.Value, delta, "Steam Quality");
            Assert.AreEqual(11.6, fun.PumpWork.Value, delta, "Pump Work");
            Assert.AreEqual(3189, fun.BoilerWork.Value, delta, "Boiler Work");
            Assert.AreEqual(2245, fun.CondenserWork.Value, delta, "Condenser Work");
            Assert.AreEqual(955.9, fun.TurbineWork.Value, delta, "Turbine Work");
            Assert.AreEqual(0.2961, fun.ThermalEfficiency.Value, delta, "Thermal Efficiency");
            Assert.AreEqual(944.3, fun.NetWork.Value, delta, "Net Work");
            Assert.AreEqual(84.75, fun.SteamRate.Value, delta, "Steam Rate");
            Assert.AreEqual(270.2e3, fun.BoilerHeatTransRate.Value, delta, "Boiler Heat Transfer Rate");
            Assert.AreEqual(190.2e3, fun.CondenserHeatTransRate.Value, delta, "Condenser Heat Transfer Rate");
        }
    }
}
