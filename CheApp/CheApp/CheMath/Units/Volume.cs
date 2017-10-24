using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all volume unit functions
    /// </summary>
    public class Volume : AbstractUnit
    {
        /// <summary>
        /// cubic meters
        /// </summary>
        public static readonly Volume m3 = new Volume("m\xB3", 1);

        /// <summary>
        /// liters
        /// </summary>
        public static readonly Volume liter = new Volume("l", 1000);

        /// <summary>
        /// milliliters
        /// </summary>
        public static readonly Volume ml = new Volume("ml", 1e+6);

        /// <summary>
        /// cubic feet
        /// </summary>
        public static readonly Volume ft3 = new Volume("ft\xB3", 35.3147);

        /// <summary>
        /// US Gallon
        /// </summary>
        public static readonly Volume USGallon = new Volume("US Gallon", 264.172);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Volume> StringToUnit = new Dictionary<string, Volume>
        {
            { ft3.ToString(), ft3 },
            { liter.ToString(), liter },
            { m3.ToString(), m3 },
            { ml.ToString(), ml },
            { USGallon.ToString(), USGallon }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Volume(string name, double conversionFactor)
        {
            this.ConversionFactor = conversionFactor;
            this.Name = name;
        }

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
        public static double Convert(double value, Volume currentUnit, Volume desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }

    }
}
