using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Thermo;
using System;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Parameter;

namespace BackendTesting
{
    [TestClass]
    public class Thermo
    {

        /// <summary>
        /// Tests the orifice plate function
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

            fun.GetParameter((int)RankineCycle.Field.steamP).UnitSelection[0].SelectedObject = Pressure.kPa;
            fun.GetParameter((int)RankineCycle.Field.steamP).ValueStr = "8600";

            fun.GetParameter((int)RankineCycle.Field.steamTemp).UnitSelection[0].SelectedObject = Temperature.C;
            fun.GetParameter((int)RankineCycle.Field.steamTemp).ValueStr = "500";

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

            Assert.AreEqual(0.8051, fun.CondenserSteamQuality, delta, "Steam Quality");
            Assert.AreEqual(11.6, fun.PumpWork, delta, "Pump Work");
            Assert.AreEqual(3189, fun.BoilerWork, delta, "Boiler Work");
            Assert.AreEqual(2245, fun.CondenserWork, delta, "Condenser Work");
            Assert.AreEqual(955.9, fun.TurbineWork, delta, "Turbine Work");
            Assert.AreEqual(0.2961, fun.ThermalEfficiency, delta, "Thermal Efficiency");
            Assert.AreEqual(944.3, fun.NetWork, delta, "Net Work");
            Assert.AreEqual(84.75, fun.SteamRate, delta, "Steam Rate");
            Assert.AreEqual(270.2e3, fun.BoilerHeatTransRate, delta, "Boiler Heat Transfer Rate");
            Assert.AreEqual(190.2e3, fun.CondenserHeatTransRate, delta, "Condenser Heat Transfer Rate");
        }
    }
}
