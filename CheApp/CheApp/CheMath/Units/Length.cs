using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    class Length : AbstractUnit
    {
        /// <summary>
        /// meters
        /// </summary>
        public static readonly Length m = new Length("m", 1);

        /// <summary>
        /// cubic feet
        /// </summary>
        public static readonly Length ft = new Length("ft", 3.28084);

        /// <summary>
        /// cubic feet
        /// </summary>
        public static readonly Length inch = new Length("in", 3.28084);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Length> StringToUnit = new Dictionary<string, Length>
        {
            { m.ToString(), m },
            { ft.ToString(), ft },            
            { inch.ToString(), inch }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Length(string name, double conversionFactor) : base(name, conversionFactor) { }


        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnitName">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, string desiredUnitName)
        {
            return Convert(curValue, this, StringToUnit[desiredUnitName]);
        }


        /// <summary>
        /// Converts between two different volume units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current volume unit of "value"</param>
        /// <param name="desiredUnit">Desired volume unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Length currentUnit, Length desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
