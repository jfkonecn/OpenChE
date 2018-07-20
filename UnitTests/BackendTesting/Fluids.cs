using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Fluids;
using EngineeringMath.Calculations.Components.Functions;

namespace BackendTesting
{
    [TestClass]
    public class Fluids
    {
        // use this for when you expect the test method to throw an exception
        //[TestMethod, ExpectedException(typeof(System.ArgumentException))]

        /// <summary>
        /// Tests the orifice plate function
        /// </summary>
        [TestMethod]
        public void OrificePlateTest()
        {
            OrificePlate fun = 
                (OrificePlate)
                ComponentFactory.BuildComponent(typeof(OrificePlate));

            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.disCo, "Discharge Coefficient");
            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.density, "Density");
            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.pArea, "Pipe Area");
            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.oArea, "Orifice Area");
            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.deltaP, "Delta P");
            OrificePlateSingleOutputTest(ref fun, (int)OrificePlate.Field.volFlow, "Volumetric Flow Rate");



        }
        /// <summary>
        /// Tests the output for a single orifice plate parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void OrificePlateSingleOutputTest(ref OrificePlate fun, int outputId, string testName)
        {


            // in m
            double pipeArea = 10 * 10 * Math.PI / 4.0;
            // in m
            double orfArea = 8 * 8 * Math.PI / 4.0;
            // in pa
            double pressureDrop = 10;
            // in kg/m3
            double density = 1000;
            // discharge coefficient
            double cd = 0.7;
            // volume in m3/s
            double volumeFlowRate = 6.476;

            // set all inputs
            fun.GetParameter((int)OrificePlate.Field.pArea).UnitSelection[0].SelectedObject = EngineeringMath.Units.Area.m2;
            fun.GetParameter((int)OrificePlate.Field.pArea).Value = pipeArea;

            fun.GetParameter((int)OrificePlate.Field.oArea).UnitSelection[0].SelectedObject = EngineeringMath.Units.Area.m2;
            fun.GetParameter((int)OrificePlate.Field.oArea).Value = orfArea;

            fun.GetParameter((int)OrificePlate.Field.deltaP).UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.GetParameter((int)OrificePlate.Field.deltaP).Value = pressureDrop;

            fun.GetParameter((int)OrificePlate.Field.density).UnitSelection[0].SelectedObject = Density.kgm3;
            fun.GetParameter((int)OrificePlate.Field.density).Value = density;

            fun.GetParameter((int)OrificePlate.Field.disCo).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)OrificePlate.Field.disCo).Value = cd;

            fun.GetParameter((int)OrificePlate.Field.volFlow).UnitSelection[0].SelectedObject = Volume.m3;
            fun.GetParameter((int)OrificePlate.Field.volFlow).UnitSelection[1].SelectedObject = Time.sec;
            fun.GetParameter((int)OrificePlate.Field.volFlow).Value = volumeFlowRate;

            FinishTest(fun, outputId, testName);
        }

        /// <summary>
        /// Tests Bernoulli's Equation function
        /// </summary>
        [TestMethod]
        public void BernoullisEquationTest()
        {
            BernoullisEquation fun =
                (BernoullisEquation)
                ComponentFactory.BuildComponent(typeof(BernoullisEquation));

            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.inletVelo, "Inlet Velocity");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.outletVelo, "Outlet Velocity");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.inletP, "Inlet Pressure");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.outletP, "Outlet Pressure");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.inletHeight, "Inlet Height");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.outletHeight, "Outlet Height");
            BernoullisEquationSingleOutputTest(ref fun, (int)BernoullisEquation.Field.density, "Density");


        }

        /// <summary>
        /// Tests the output for Bernoulli's Equation parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void BernoullisEquationSingleOutputTest(ref BernoullisEquation fun, int outputId, string testName)
        {

            // in m /s
            double inletVelo = 11,
                // in m / s
                outletVelo = 10,
                // in m
                inletHeight = 10,
                // in m
                outletHeight = 9,
                // in Pa
                inletP = 100,
                // in Pa
                outletP = 20410,
                // in kg/m3
                density = 1000;

            // set all inputs
            fun.GetParameter((int)BernoullisEquation.Field.inletVelo).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)BernoullisEquation.Field.inletVelo).UnitSelection[1].SelectedObject = Time.sec;
            fun.GetParameter((int)BernoullisEquation.Field.inletVelo).Value = inletVelo;

            fun.GetParameter((int)BernoullisEquation.Field.outletVelo).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)BernoullisEquation.Field.outletVelo).UnitSelection[1].SelectedObject = Time.sec;
            fun.GetParameter((int)BernoullisEquation.Field.outletVelo).Value = outletVelo;

            fun.GetParameter((int)BernoullisEquation.Field.inletHeight).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)BernoullisEquation.Field.inletHeight).Value = inletHeight;

            fun.GetParameter((int)BernoullisEquation.Field.outletHeight).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)BernoullisEquation.Field.outletHeight).Value = outletHeight;

            fun.GetParameter((int)BernoullisEquation.Field.inletP).UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.GetParameter((int)BernoullisEquation.Field.inletP).Value = inletP;

            fun.GetParameter((int)BernoullisEquation.Field.outletP).UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.GetParameter((int)BernoullisEquation.Field.outletP).Value = outletP;

            fun.GetParameter((int)BernoullisEquation.Field.density).UnitSelection[0].SelectedObject = Density.kgm3;
            fun.GetParameter((int)BernoullisEquation.Field.density).Value = density;

            FinishTest(fun, outputId, testName);
        }

        /// <summary>
        /// Tests Pitot Tube function
        /// </summary>
        [TestMethod]
        public void PitotTubeTest()
        {
            PitotTube fun =
                (PitotTube)
                ComponentFactory.BuildComponent(typeof(PitotTube));

            PitotTubeSingleOutputTest(ref fun, (int)PitotTube.Field.correctionCo, "Correction Coefficient");
            PitotTubeSingleOutputTest(ref fun, (int)PitotTube.Field.deltaH, "Change in Height");
            PitotTubeSingleOutputTest(ref fun, (int)PitotTube.Field.fluidDensity, "Fluid Density");
            PitotTubeSingleOutputTest(ref fun, (int)PitotTube.Field.manoDensity, "Manometer Density");
            PitotTubeSingleOutputTest(ref fun, (int)PitotTube.Field.velo, "Velocity");

        }

        /// <summary>
        /// Tests the output for Pitot Tube parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void PitotTubeSingleOutputTest(ref PitotTube fun, int outputId, string testName)
        {

            // unitless
            double correctionCo = 0.98,
                // in m
                deltaH = 0.0107,
                // in kg/m3
                manoDensity = 1000,
                // in kg/m3
                fluidDensity = 1.063,
                // in m/sec
                velo = 13.7648;

            // set all inputs
            fun.GetParameter((int)PitotTube.Field.correctionCo).UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.GetParameter((int)PitotTube.Field.correctionCo).Value = correctionCo;

            fun.GetParameter((int)PitotTube.Field.deltaH).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)PitotTube.Field.deltaH).Value = deltaH;

            fun.GetParameter((int)PitotTube.Field.manoDensity).UnitSelection[0].SelectedObject = Density.kgm3;
            fun.GetParameter((int)PitotTube.Field.manoDensity).Value = manoDensity;

            fun.GetParameter((int)PitotTube.Field.fluidDensity).UnitSelection[0].SelectedObject = Density.kgm3;
            fun.GetParameter((int)PitotTube.Field.fluidDensity).Value = fluidDensity;

            fun.GetParameter((int)PitotTube.Field.velo).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)PitotTube.Field.velo).UnitSelection[1].SelectedObject = Time.sec;
            fun.GetParameter((int)PitotTube.Field.velo).Value = velo;

            FinishTest(fun, outputId, testName);
        }

        private void FinishTest(SolveForFunction fun, int outputId, string testName)
        {
            double actual = 0;
            double expected = fun.GetParameter(outputId).Value;

            // set the output to something completely wrong
            fun.GetParameter(outputId).Value = double.NaN;
            fun.OutputSelection.SelectedObject = fun.GetParameter(outputId);
            fun.SolveButton.Command.Execute(null);
            actual = fun.GetParameter(outputId).Value;

            //Valid Inputs
            Assert.AreEqual(expected,
                actual
                , 0.8, testName);
        }
    }
}
