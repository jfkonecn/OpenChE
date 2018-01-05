using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;
using EngineeringMath.Units;

namespace BackendTesting
{
    [TestClass]
    public class FluidsTest
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
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.pArea].UnitSelection[0].SelectedObject = Area.m2;
            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.pArea].SetValue(pipeArea);

            fun.FieldDic[(int)EngineeringMath.Calculations.Fluids.OrificePlate.Field.oArea].UnitSelection[0].SelectedObject = Area.m2;
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
    }
}
