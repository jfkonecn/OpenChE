using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
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

        /// <summary>
        /// Converts from curUnits to the one represented by the desiredUnitNames
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="curUnits">Current units</param>
        /// <param name="desiredUnitNames">String names of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        internal static double ConvertTo(double curValue, AbstractUnit[] curUnits, string[] desiredUnitNames)
        {
            if(curUnits.Length != desiredUnitNames.Length || curUnits.Length > 2)
            {
                throw new Exception("Array out of range");
            }

            curValue = curUnits[0].ConvertTo(curValue, desiredUnitNames[0]);

            if (curUnits.Length == 2)
            {
                curValue = 1 / curValue;
                curValue = curUnits[1].ConvertTo(curValue, desiredUnitNames[1]);
                curValue = 1 / curValue;
            }

            return curValue;
        }

        /// <summary>
        /// Converts from curUnitNames to the one represented by desiredUnits
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="curUnits">Current units</param>
        /// <param name="desiredUnitNames">String names of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        internal static double ConvertFrom(double curValue, AbstractUnit[] desiredUnits, string[] curUnitNames)
        {
            AbstractUnit[] curUnits = new AbstractUnit[curUnitNames.Length];
            string[] desiredUnitNames = new string[desiredUnits.Length];

            // do in two loops to catch an array mismatch in the ConvertTo function
            for (int i = 0; i < curUnitNames.Length; i++)
            {
                curUnits[i] = StaticUnitProperties.AllUnits[desiredUnits[i].GetType()][curUnitNames[i]];
            }

            for (int i = 0; i < desiredUnitNames.Length; i++)
            {
                desiredUnitNames[i] = desiredUnits[i].ToString();
            }

            return ConvertTo(
                curValue,
                curUnits,
                desiredUnitNames);
        }
    }
}
