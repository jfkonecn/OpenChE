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
        /*
         * Since volume is based on length, there is no conversion factor of 1
         */
        
        /// <summary>
        /// cubic meters
        /// </summary>
        public static readonly Volume m3 = new Volume("m\xB3", Math.Pow(Length.m.ConversionFactor, 3));

        /// <summary>
        /// liters
        /// </summary>
        public static readonly Volume liter = new Volume("l", m3.ConversionFactor * 1000);

        /// <summary>
        /// milliliters
        /// </summary>
        public static readonly Volume ml = new Volume("ml", m3.ConversionFactor * 1e+6);

        /// <summary>
        /// cubic feet
        /// </summary>
        public static readonly Volume ft3 = new Volume("ft\xB3", Math.Pow(Length.ft.ConversionFactor, 3));

        /// <summary>
        /// US Gallon
        /// </summary>
        public static readonly Volume USGallon = new Volume("US Gallon", m3.ConversionFactor * 264.172);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { m3.ToString(), m3 },
            { ft3.ToString(), ft3 },
            { liter.ToString(), liter },            
            { ml.ToString(), ml },
            { USGallon.ToString(), USGallon }
        };

        /// <summary>
        /// Based on length^3
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Volume(string name, double conversionFactor) : base(name, conversionFactor) { }


        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnitName">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, string desiredUnitName)
        {
            return Convert(curValue, this, (Volume)StringToUnit[desiredUnitName]);
        }


        /// <summary>
        /// Conversion factors are all determined by Length ^ 3
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
