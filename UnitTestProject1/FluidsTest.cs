using Microsoft.VisualStudio.TestTools.UnitTesting;
using CheMath.Calculations;
using System;
using CheMath;
namespace UnitTests
{

    /// <summary>
    /// Tests the Fluids class in the core namespace within the CheApp project
    /// </summary>
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
            // in m
            double pipeDia = 10;
            // in m
            double orfDia = 8;
            // in pa
            double inletPressure = 1010;
            // in pa
            double outletPressure = 1000;
            // in kg/m3
            double density = 1000;
            // discharge coefficient
            double cd = 0.7;

            double actual = Fluids.OrificePlate(cd, density, pipeDia, orfDia, inletPressure, outletPressure);

            double expected = 6.476;

            //Valid Inputs
            Assert.AreEqual(expected, actual, 0.001, "Standard Case");

            //Invalid Inputs
            

            string failedToThrow = "An exception should have been thrown";

            try
            {
                Fluids.OrificePlate(50, density, pipeDia, orfDia, inletPressure, outletPressure);
                Assert.Fail(failedToThrow);
            }
            catch (ArgumentOutOfRangeException ae)
            {
                // moving on
            }
            catch (Exception e)
            {
                Assert.Fail(
                     string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message)
                );
            }

            try
            {
                Fluids.OrificePlate(50, density, pipeDia, orfDia, 10, 50);
                Assert.Fail(failedToThrow);
            }
            catch (ArgumentOutOfRangeException ae)
            {
                // moving on
            }
            catch (Exception e)
            {
                Assert.Fail(
                     string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message)
                );
            }



        }
    }
}
