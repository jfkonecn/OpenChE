using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Resources.LookupTables
{
    internal class Interpolation
    {
        /// <summary>
        /// Performs a linear interpolation between two reference points 
        /// </summary>
        /// <param name="y2">reference point two's y coordinate</param>
        /// <param name="y1">reference point one's y coordinate</param>
        /// <param name="x2">reference point two's x coordinate</param>
        /// <param name="x1">reference point one's x coordinate</param>
        /// <param name="x">Desired x coordinate</param>
        /// <returns>Desired y coordinate</returns>
        internal static double LinearInterpolation(
            double x,
            double x1,
            double y1,
            double x2,
            double y2)
        {
            return ((y2 - y1) / (x2 - x1)) * (x - x1) + y1;
        }
    }
}
