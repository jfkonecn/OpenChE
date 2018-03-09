using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.NumericalMethods.FiniteDifferenceFormulas
{
    public class FirstDerivative
    {
        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="iMinus2">two steps back</param>
        /// <param name="iMinus1">one step back</param>
        /// <param name="i">point where the first derivative will be calculated</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double ThreePointBackward(double iMinus2, double iMinus1, double i, double step)
        {
            return (iMinus2 - 4 * iMinus1 + 3 * i) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="iMinus1">one step back from point where the first derivative will be calculated</param>
        /// <param name="i1">one step forward from point where the first derivative will be calculated</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointCentral(double iMinus1, double i1, double step)
        {
            return (i1 - iMinus1) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="i">point where the first derivative will be calculated</param>
        /// <param name="i1">one step forward</param>
        /// <param name="i2">two steps forward</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double ThreePointForward(double i, double i1, double i2, double step)
        {
            return (-3 * i + 4 * i1 - i2) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step)
        /// </summary>
        /// <param name="iMinus1">one step back from point where the first derivative will be calculated</param>
        /// <param name="i">point where the first derivative will be calculated</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointBackward(double iMinus1, double i, double step)
        {
            return (i - iMinus1) / step;
        }

        /// <summary>
        /// Truncation Error O(step)
        /// </summary>
        /// <param name="i">point where the first derivative will be calculated</param>
        /// <param name="i1">one step forward from point where the first derivative will be calculated</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointForward(double i, double i1, double step)
        {
            return (i1 - i) / step;
        }
    }
}
