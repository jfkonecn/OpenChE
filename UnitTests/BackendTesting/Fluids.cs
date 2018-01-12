using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;
using EngineeringMath.Units;

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
            EngineeringMath.Calculations.Fluids.OrificePlate fun = 
                (EngineeringMath.Calculations.Fluids.OrificePlate)
                FunctionFactory.BuildFunction(typeof(EngineeringMath.Calculations.Fluids.OrificePlate));

            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.disCo, "Discharge Coefficient");
            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.density, "Density");
            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.pArea, "Pipe Area");
            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.oArea, "Orifice Area");
            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.deltaP, "Delta P");
            OrificePlateSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.volFlow, "Volumetric Flow Rate");



        }
        /// <summary>
        /// Tests the output for a single orifice plate parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void OrificePlateSingleOutputTest(ref EngineeringMath.Calculations.Fluids.OrificePlate fun, int outputId, string testName)
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
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.pArea].UnitSelection[0].SelectedObject = EngineeringMath.Units.Area.m2;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.pArea].SetValue(pipeArea);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.oArea].UnitSelection[0].SelectedObject = EngineeringMath.Units.Area.m2;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.oArea].SetValue(orfArea);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.deltaP].UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.deltaP].SetValue(pressureDrop);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.density].UnitSelection[0].SelectedObject = Density.kgm3;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.density].SetValue(density);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.disCo].UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.disCo].SetValue(cd);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.volFlow].UnitSelection[0].SelectedObject = Volume.m3;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.volFlow].UnitSelection[1].SelectedObject = Time.sec;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.volFlow].SetValue(volumeFlowRate);

            double actual = 0;
            double expected = fun.FieldDic[outputId].GetValue();

            // set the output to something completely wrong
            fun.FieldDic[outputId].SetValue(double.NaN);
            fun.OutputSelection.SelectedObject = fun.FieldDic[outputId];
            fun.Solve();
            actual = fun.FieldDic[outputId].GetValue();

            //Valid Inputs
            Assert.AreEqual(expected,
                fun.FieldDic[outputId].GetValue()
                , 0.05, testName);
        }

        /// <summary>
        /// Tests Bernoulli's Equation function
        /// </summary>
        [TestMethod]
        public void BernoullisEquationTest()
        {
            EngineeringMath.Calculations.Fluids.BernoullisEquation fun =
                (EngineeringMath.Calculations.Fluids.BernoullisEquation)
                FunctionFactory.BuildFunction(typeof(EngineeringMath.Calculations.Fluids.BernoullisEquation));

            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletVelo, "Inlet Velocity");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletVelo, "Outlet Velocity");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletP, "Inlet Pressure");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletP, "Outlet Pressure");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletHeight, "Inlet Height");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletHeight, "Outlet Height");
            BernoullisEquationSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.density, "Density");


        }

        /// <summary>
        /// Tests the output for Bernoulli's Equation parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void BernoullisEquationSingleOutputTest(ref EngineeringMath.Calculations.Fluids.BernoullisEquation fun, int outputId, string testName)
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
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletVelo].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletVelo].UnitSelection[1].SelectedObject = Time.sec;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletVelo].SetValue(inletVelo);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletVelo].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletVelo].UnitSelection[1].SelectedObject = Time.sec;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletVelo].SetValue(outletVelo);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletHeight].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletHeight].SetValue(inletHeight);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletHeight].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletHeight].SetValue(outletHeight);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletP].UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.inletP].SetValue(inletP);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletP].UnitSelection[0].SelectedObject = Pressure.Pa;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.outletP].SetValue(outletP);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.density].UnitSelection[0].SelectedObject = Density.kgm3;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.BernoullisEquation.Field.density].SetValue(density);

            double actual = 0;
            double expected = fun.FieldDic[outputId].GetValue();

            // set the output to something completely wrong
            fun.FieldDic[outputId].SetValue(double.NaN);
            fun.OutputSelection.SelectedObject = fun.FieldDic[outputId];
            fun.Solve();
            actual = fun.FieldDic[outputId].GetValue();

            //Valid Inputs
            Assert.AreEqual(expected,
                fun.FieldDic[outputId].GetValue()
                , 0.05, testName);
        }

        /// <summary>
        /// Tests Pitot Tube function
        /// </summary>
        [TestMethod]
        public void PitotTubeTest()
        {
            EngineeringMath.Calculations.Fluids.PitotTube fun =
                (EngineeringMath.Calculations.Fluids.PitotTube)
                FunctionFactory.BuildFunction(typeof(EngineeringMath.Calculations.Fluids.PitotTube));

            PitotTubeSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.PitotTube.Field.correctionCo, "Correction Coefficient");
            PitotTubeSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.PitotTube.Field.deltaH, "Change in Height");
            PitotTubeSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.PitotTube.Field.fluidDensity, "Fluid Density");
            PitotTubeSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.PitotTube.Field.manoDensity, "Manometer Density");
            PitotTubeSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Fluids.PitotTube.Field.velo, "Velocity");

        }

        /// <summary>
        /// Tests the output for Pitot Tube parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void PitotTubeSingleOutputTest(ref EngineeringMath.Calculations.Fluids.PitotTube fun, int outputId, string testName)
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
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.correctionCo].UnitSelection[0].SelectedObject = Unitless.unitless;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.correctionCo].SetValue(correctionCo);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.deltaH].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.deltaH].SetValue(deltaH);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.manoDensity].UnitSelection[0].SelectedObject = Density.kgm3;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.manoDensity].SetValue(manoDensity);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.fluidDensity].UnitSelection[0].SelectedObject = Density.kgm3;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.fluidDensity].SetValue(fluidDensity);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.velo].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.velo].UnitSelection[1].SelectedObject = Time.sec;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.PitotTube.Field.velo].SetValue(velo);

            double actual = 0;
            double expected = fun.FieldDic[outputId].GetValue();

            // set the output to something completely wrong
            fun.FieldDic[outputId].SetValue(double.NaN);
            fun.OutputSelection.SelectedObject = fun.FieldDic[outputId];
            fun.Solve();
            actual = fun.FieldDic[outputId].GetValue();

            //Valid Inputs
            Assert.AreEqual(expected,
                fun.FieldDic[outputId].GetValue()
                , 0.8, testName);
        }
    }
}
