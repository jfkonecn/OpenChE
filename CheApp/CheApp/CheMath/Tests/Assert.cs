using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Tests
{
    /// <summary>
    /// Can't make a refence to Microsoft.VisualStudio inside this project...
    /// </summary>
    class Assert
    {
        /// <summary>
        /// Verifies that two specified doubles are equal, or within the specified accuracy of each other. 
        /// The assertion fails if they are not within the specified accuracy of each other. 
        /// Displays a message if the assertion fails.
        /// </summary>
        /// <param name="expected">The first double to compare. This is the double the unit test expects.</param>
        /// <param name="actual">The second double to compare. This is the double the unit test produced.</param>
        /// <param name="delta">The required accuracy. The assertion will fail only if expected is different from actual by more than delta.</param>
        /// <param name="message">A message to display if the assertion fails. This message can be seen in the unit test results.</param>
        public static void AreEqual(double expected, double actual, double delta, string message)
        {

            if (Math.Abs(expected - actual) > delta)
            {
                throw new Exception(message);
            }
        }



        /// <summary>
        /// Verifies that two specified doubles are equal, or within the specified accuracy of each other. 
        /// The assertion fails if they are not within the specified accuracy of each other. 
        /// Displays a message if the assertion fails.
        /// </summary>
        /// <param name="expected">The first double to compare. This is the double the unit test expects.</param>
        /// <param name="actual">The second double to compare. This is the double the unit test produced.</param>
        /// <param name="delta">The required accuracy. The assertion will fail only if expected is different from actual by more than delta.</param>
        public static void AreEqual(double expected, double actual, double delta)
        {

            if (Math.Abs(expected - actual) > delta)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Fails the assertion without checking any conditions. Displays a message, and applies the specified formatting to it.
        /// </summary>
        /// <param name="message">A message to display. This message can be seen in the unit test results.</param>
        public static void Fail(string message)
        {
            throw new Exception(message);
        }
    }
}
