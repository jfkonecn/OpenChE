using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;
using EngineeringMath.Units;

namespace BackendTesting
{
    [TestClass]
    public class AreaTest
    {
        /// <summary>
        /// Tests area of a circle
        /// </summary>
        [TestMethod]
        public void CircleTest()
        {
            EngineeringMath.Calculations.Area.Circle fun =
    (EngineeringMath.Calculations.Area.Circle)
    FunctionFactory.BuildFunction(typeof(EngineeringMath.Calculations.Area.Circle));

            CircleSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Area.Circle.Field.cirDia, "Diameter");
            CircleSingleOutputTest(ref fun, (int)EngineeringMath.Calculations.Area.Circle.Field.cirArea, "Area");
        }

        /// <summary>
        /// Tests the output for a single circle parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void CircleSingleOutputTest(ref EngineeringMath.Calculations.Area.Circle fun, int outputId, string testName)
        {
            // in m
            double dia = 10;
            // in m2
            double area = dia * dia * Math.PI / 4.0;

            // set all inputs
            fun.FieldDic[(int)EngineeringMath.Calculations.Area.Circle.Field.cirDia].UnitSelection[0].SelectedObject = Length.m;
            fun.FieldDic[(int)EngineeringMath.Calculations.Area.Circle.Field.cirDia].SetValue(dia);

            fun.FieldDic[(int)EngineeringMath.Calculations.Area.Circle.Field.cirArea].UnitSelection[0].SelectedObject = Area.m2;
            fun.FieldDic[(int)EngineeringMath.Calculations.Area.Circle.Field.cirArea].SetValue(area);

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
