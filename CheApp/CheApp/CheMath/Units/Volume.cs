using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all volume unit functions
    /// </summary>
    public class Volume
    {
        /// <summary>
        /// cubic meters
        /// </summary>
        public static readonly Volume m3 = new Volume(1);

        /// <summary>
        /// liters
        /// </summary>
        public static readonly Volume liter = new Volume(1000);

        /// <summary>
        /// milliliters
        /// </summary>
        public static readonly Volume ml = new Volume(1e+6);

        /// <summary>
        /// cubic feet
        /// </summary>
        public static readonly Volume ft3 = new Volume(35.3147);

        /// <summary>
        /// US Gallon
        /// </summary>
        public static readonly Volume USGallon = new Volume(264.172);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Volume> StringToUnit = new Dictionary<string, Volume>
        {
            { "ft\xB3", ft3 },
            { "l", liter },
            { "m\xB3", m3 },
            { "ml", ml },
            { "US Gallon", USGallon }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="conversionFactor"></param>
        private Volume(double conversionFactor)
        {
            this.ConversionFactor = conversionFactor;

        }


        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// <para>Example: If 1 ft is the standard then 12 would be the inch's conversionFactor and 1 would be the foot's conversionFactor</para>
        /// <para>Note that the standard is picked within the class which inherits this class </para>
        /// </summary>
        private double ConversionFactor { set; get; }

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
