using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EngineeringMath.Calculations
{

    /// <summary>
    /// Thrown when a function cannot be solved
    /// </summary>
    public class UnsolvableException : Exception
    {
        public UnsolvableException(string message)
           : base(message)
        {
        }
    }

    /// <summary>
    /// Contains methods to solve functions
    /// </summary>
    public class Solver
    {
        private static Random rnd = new Random();

        public delegate double MyFunction(double x);
        /// <summary>
        /// Solves for x given f(x) using NewtonsMethod
        /// </summary>
        /// <param name="y">f(x)</param>
        /// <param name="fun"></param>
        /// <param name="maxFracError">The maximum error allowed between y and actual fun(x) result
        /// <para>(parameter in terms of fraction)</para></param>
        /// <returns>x</returns>
        public static double NewtonsMethod(double y, MyFunction fun, double maxFracError = 1e-6, 
            double minValue = double.MinValue, 
            double maxValue = double.MaxValue)
        {
            // maximum amount of tries allowed to attempt to solve the function
            double maxTries = 1e6;
            double curFracError = double.MaxValue;
            double curX = 0;
            double totalTries = 0;
            // create a new function where so that f(x) - y
            // since Newton's method finds where f(x) is equal to zero
            MyFunction zeroFun = x => fun(x) - y;

            // newton's method function... returns a new value for x
            MyFunction newMethFun = x => x - (zeroFun(x) / FirstDerivative(x, zeroFun));

            // this function may not be defined for all real number
            // try to find a valid x value to start
            while ((newMethFun(curX)).Equals(double.NaN))
            {
                curX = (rnd.NextDouble() - 0.5) * 1e1;
                if (totalTries++ > maxTries)
                {
                    Debug.WriteLine($"{y:n} maxed me out when trying random numbers!");
                    throw new UnsolvableException("Maximum Tries Reached");
                }
            }

            // the last step size x had
            double lastStep = 1e6;

            while (maxFracError < curFracError || curFracError.Equals(double.NaN))
            {
                double tempX = newMethFun(curX);
                totalTries++;
                if (!((tempX).Equals(double.NaN) || double.IsInfinity(tempX)))
                {
                    lastStep = tempX - curX;
                }
                

                // factor by which we step backwards
                double stepFactor = 1;
                // loop until step is valid
                while ((tempX).Equals(double.NaN) || double.IsInfinity(tempX))
                {
                    // we must go backwards
                    tempX = curX - lastStep * stepFactor;

                    tempX = newMethFun(tempX);

                    if (tempX < minValue)
                    {
                        tempX = minValue;
                    }
                    else if (tempX > maxValue)
                    {
                        tempX = maxValue;
                    }

                    stepFactor *= 0.5;

                    if (totalTries++ > maxTries)
                    {
                        Debug.WriteLine($"{y:n} \nmaxed me out!\n This is the last value I had:\n {fun(curX):n}");
                        throw new UnsolvableException("Maximum Tries Reached");
                    }
                }


                curX = tempX;

                curFracError = Math.Abs((y - fun(curX)) / y);

              
            }
            Debug.WriteLine($"It only took me {totalTries:n0} tries!\nTo Solve:\n{fun(curX):n}");
            return curX;
        }
        
        /// <summary>
        /// Finds first derivative using finite difference formulas
        /// </summary>
        /// <param name="x">x where derivative will be calculated</param>
        /// <param name="fun">f(x)</param>
        /// <returns>dx/dy</returns>
        private static double FirstDerivative(double x, MyFunction fun)
        {
            double stepSize = 1e-3;
            // Two-point central difference
            return (fun(x + stepSize) - fun(x - stepSize)) /
                (2 * stepSize);
        }
    }
}
