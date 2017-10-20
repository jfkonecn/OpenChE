using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all time unit functions
    /// </summary>
    public class Time
    {
        /// <summary>
        /// seconds
        /// </summary>
        public static readonly Time sec = new Time(86400);

        /// <summary>
        /// minutes
        /// </summary>
        public static readonly Time min = new Time(1440);

        /// <summary>
        /// hours
        /// </summary>
        public static readonly Time hr = new Time(24);

        /// <summary>
        /// days
        /// </summary>
        public static readonly Time day = new Time(1);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Time> StringToUnit = new Dictionary<string, Time>
        {
            { "sec", sec },
            { "min", min },
            { "hr", hr },
            { "day", day }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="conversionFactor"></param>
        private Time(double conversionFactor)
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
        /// Converts between two different time units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current time unit of "value"</param>
        /// <param name="desiredUnit">Desired time unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Time currentUnit, Time desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
