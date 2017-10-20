using System;
using System.Collections.Generic;
using System.Text;

namespace CheMath.Units
{
    /// <summary>
    /// Contains all mass unit functions
    /// </summary>
    public class Mass
    {
        /// <summary>
        /// grams
        /// </summary>
        public static readonly Mass g = new Mass(1);
        /// <summary>
        /// pounds mass
        /// </summary>
        public static readonly Mass lbsm = new Mass(0.00220462);
        /// <summary>
        /// kilograms
        /// </summary>
        public static readonly Mass kg = new Mass(0.001);
        /// <summary>
        /// metric tons
        /// </summary>
        public static readonly Mass MetricTon = new Mass(1e-6);
        /// <summary>
        /// US tons
        /// </summary>
        public static readonly Mass USTon = new Mass(1.1023e-6);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Mass> StringToUnit = new Dictionary<string, Mass>
        {
            { "g", g },
            { "lbs\x208m", lbsm },
            { "kg", kg },
            { "Metric Ton", MetricTon },
            { "US Ton", USTon }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="conversionFactor"></param>
        private Mass(double conversionFactor)
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
        /// Converts between two different mass units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current mass unit of "value"</param>
        /// <param name="desiredUnit">Desired mass unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Mass currentUnit, Mass desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
