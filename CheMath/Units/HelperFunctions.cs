using System;
using System.Collections.Generic;
using System.Text;

namespace CheMath.Units
{
    internal class HelperFunctions
    {
        /// <summary>
        /// A generic conversion which all functions in the class reference
        /// <para>Used as value * desiredConversionFactor / currentConversionFactor</para>
        /// </summary>
        /// <param name="value">current value</param>
        /// <param name="currentConversionFactor">Current unit's conversion factor</param>
        /// <param name="desiredConversionFactor">Desired unit's conversion factor</param>
        /// <returns>Converted value</returns>
        internal static double Converter(double value, double currentConversionFactor, double desiredConversionFactor)
        {
            return value * desiredConversionFactor / currentConversionFactor;
        }
    }
}
