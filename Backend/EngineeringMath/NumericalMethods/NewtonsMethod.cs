using EngineeringMath.NumericalMethods.FiniteDifferenceFormulas;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.NumericalMethods
{
    public static class NewtonsMethod
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startingX"></param>
        /// <param name="fx"></param>
        /// <param name="fxPrime"></param>
        /// <returns>first x value found where f(x) == 0 if max guesses reached then NaN is returned</returns>
        public static double Solve(double startingX, Func<double, double> fx, Func<double, double> fxPrime)
        {
            double curX = startingX,
                fxResult = fx(curX),
                totalGuesses = 0;
            const double maxErr = 1e-6,
                maxGuesses = 1e4;
            while (Math.Abs(fxResult) > maxErr && totalGuesses < maxGuesses)
            {
                fxResult = fx(curX);
                curX = curX - fxResult / fxPrime(curX);
                totalGuesses++;
            }
            if (totalGuesses == maxGuesses)
                return double.NaN;
            return curX;
        }
        /// <summary>
        /// Use a two point center finite difference formulta to calculate fxPrime
        /// </summary>
        /// <param name="startingX"></param>
        /// <param name="step"></param>
        /// <param name="fx"></param>
        /// <returns></returns>
        public static double Solve(double startingX, double step, Func<double, double> fx)
        {
            return Solve(startingX, fx, (x) => { return FirstDerivative.TwoPointCentral(x, fx, step); });
        }
    }
}
