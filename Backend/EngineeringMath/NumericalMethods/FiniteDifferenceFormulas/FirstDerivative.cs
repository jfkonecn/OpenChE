using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.NumericalMethods.FiniteDifferenceFormulas
{
    public static class FirstDerivative
    {
        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="x">The x value where the derivative will be calculated</param>
        /// <param name="fx">Function which the derivative is being estimated for</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double ThreePointBackward(double x, Func<double, double> fx,  double step)
        {
            return (fx(x - 2 * step) - 4 * fx(x - step) + 3 * fx(x)) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="x">The x value where the derivative will be calculated</param>
        /// <param name="fx">Function which the derivative is being estimated for</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointCentral(double x, Func<double, double> fx, double step)
        {
            return (fx(x + step) - fx(x - step)) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step^2)
        /// </summary>
        /// <param name="x">The x value where the derivative will be calculated</param>
        /// <param name="fx">Function which the derivative is being estimated for</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double ThreePointForward(double x, Func<double, double> fx, double step)
        {
            return (-3 * fx(x) + 4 * fx(x + step) - fx(x + 2 * step)) / (2 * step);
        }

        /// <summary>
        /// Truncation Error O(step)
        /// </summary>
        /// <param name="x">The x value where the derivative will be calculated</param>
        /// <param name="fx">Function which the derivative is being estimated for</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointBackward(double x, Func<double, double> fx, double step)
        {
            return (fx(x) - fx(x - step)) / step;
        }

        /// <summary>
        /// Truncation Error O(step)
        /// </summary>
        /// <param name="x">The x value where the derivative will be calculated</param>
        /// <param name="fx">Function which the derivative is being estimated for</param>
        /// <param name="step">the step between the points</param>
        /// <returns></returns>
        public static double TwoPointForward(double x, Func<double, double> fx, double step)
        {
            return (fx(x + step) - fx(x)) / step;
        }
    }
}
