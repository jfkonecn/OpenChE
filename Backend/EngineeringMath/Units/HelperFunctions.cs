using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Units
{
    public class HelperFunctions
    {
        /// <summary>
        /// A generic conversion which all functions in the class reference
        /// <para>Used as value * desiredConversionFactor / currentConversionFactor</para>
        /// </summary>
        /// <param name="value">current value</param>
        /// <param name="currentConversionFactor">Current unit's conversion factor</param>
        /// <param name="desiredConversionFactor">Desired unit's conversion factor</param>
        /// <returns>Converted value</returns>
        public static double Converter(double value, double currentConversionFactor, double desiredConversionFactor)
        {
            return value * desiredConversionFactor / currentConversionFactor;
        }

        /// <summary>
        /// Converts from curUnits to the one represented by the desiredUnitNames
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="curUnits">Current units</param>
        /// <param name="desiredUnits">Type of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public static double ConvertTo(double curValue, AbstractUnit[] curUnits, AbstractUnit[] desiredUnits)
        {
            if(curUnits.Length != desiredUnits.Length || curUnits.Length > 2)
            {
                throw new Exception("Array out of range");
            }

            // check if the units are already the same
            // if they are do not do any unit convertions (to avoid rounding errors)
            bool allEqual = true;
            for (int i = 0; i < curUnits.Length; i++)
            {
                if (!curUnits[i].Equals(desiredUnits[i]))
                {
                    allEqual = false;
                    break;
                }
            }
            if (allEqual)
            {
                return curValue;
            }


            curValue = curUnits[0].ConvertTo(curValue, desiredUnits[0]);

            if (curUnits.Length == 2)
            {
                curValue = 1 / curValue;
                curValue = curUnits[1].ConvertTo(curValue, desiredUnits[1]);
                curValue = 1 / curValue;
            }

            return curValue;
        }

        /// <summary>
        /// Converts from curUnitNames to the one represented by desiredUnits
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnits">Desired units</param>
        /// <param name="curUnits">Type of current units</param>
        /// <returns>The curValue in the desired units</returns>
        public static double ConvertFrom(double curValue, AbstractUnit[] desiredUnits, AbstractUnit[] curUnits)
        {
            return ConvertTo(
                curValue,
                curUnits,
                desiredUnits);
        }


        /// <summary>
        /// Converts from curUnits to the one represented by the desiredUnitNames
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="curUnits">Current units</param>
        /// <param name="desiredUnitNames">String names of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public static double ConvertTo(double curValue, AbstractUnit[] curUnits, string[] desiredUnitNames)
        {

            AbstractUnit[] desiredUnits = new AbstractUnit[desiredUnitNames.Length];

            // do in two loops to catch an array mismatch in the ConvertTo function
            for (int i = 0; i < desiredUnitNames.Length; i++)
            {
                desiredUnits[i] = StaticUnitProperties.AllUnits[curUnits[i].GetType()][desiredUnitNames[i]];
            }

            return ConvertTo(curValue, curUnits, desiredUnits);
        }

        /// <summary>
        /// Converts from curUnitNames to the one represented by desiredUnits
        /// <para>If the arrays have 2 elements assumes units of element[0] per element[1]</para>
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnits">Desired units</param>
        /// <param name="curUnitNames">String names of current units</param>
        /// <returns>The curValue in the desired units</returns>
        public static double ConvertFrom(double curValue, AbstractUnit[] desiredUnits, string[] curUnitNames)
        {
            AbstractUnit[] curUnits = new AbstractUnit[curUnitNames.Length];

            // do in two loops to catch an array mismatch in the ConvertTo function
            for (int i = 0; i < curUnitNames.Length; i++)
            {
                curUnits[i] = StaticUnitProperties.AllUnits[desiredUnits[i].GetType()][curUnitNames[i]];
            }


            return ConvertFrom(curValue, desiredUnits, curUnits);
        }
    }
}
