using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineeringMath.Calculations;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Area;

namespace BackendTesting
{
    [TestClass]
    public class Area
    {
        /// <summary>
        /// Tests area of a circle
        /// </summary>
        [TestMethod]
        public void CircleTest()
        {
            Circle fun =
    (Circle)FunctionFactory.BuildFunction(typeof(Circle));

            CircleSingleOutputTest(ref fun, (int)Circle.Field.cirDia, "Diameter");
            CircleSingleOutputTest(ref fun, (int)Circle.Field.cirArea, "Area");
        }

        /// <summary>
        /// Tests the output for a single circle parameter
        /// </summary>
        /// <param name="fun"></param>
        /// <param name="outputId"></param>
        private void CircleSingleOutputTest(ref Circle fun, int outputId, string testName)
        {
            // in m
            double dia = 10;
            // in m2
            double area = dia * dia * Math.PI / 4.0;

            // set all inputs
            fun.GetParameter((int)Circle.Field.cirDia).UnitSelection[0].SelectedObject = Length.m;
            fun.GetParameter((int)Circle.Field.cirDia).Value = dia;

            fun.GetParameter((int)Circle.Field.cirArea).UnitSelection[0].SelectedObject = EngineeringMath.Units.Area.m2;
            fun.GetParameter((int)Circle.Field.cirArea).Value = area;

            double actual = 0;
            double expected = fun.GetParameter(outputId).Value;

            // set the output to something completely wrong
            fun.GetParameter(outputId).Value = double.NaN;
            fun.OutputSelection.SelectedObject = fun.GetParameter(outputId);
            fun.Solve();
            actual = fun.GetParameter(outputId).Value;

            //Valid Inputs
            Assert.AreEqual(expected,
                actual
                , 0.05, testName);
        }
    }


}
