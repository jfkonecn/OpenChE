using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all time unit functions
    /// </summary>
    public class Time : AbstractUnit
    {
        /// <summary>
        /// seconds
        /// </summary>
        public static readonly Time sec = new Time("sec", 86400);

        /// <summary>
        /// minutes
        /// </summary>
        public static readonly Time min = new Time("min", 1440);

        /// <summary>
        /// hours
        /// </summary>
        public static readonly Time hr = new Time("hr", 24);

        /// <summary>
        /// days
        /// </summary>
        public static readonly Time day = new Time("day", 1);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Time> StringToUnit = new Dictionary<string, Time>
        {
            { sec.ToString(), sec },
            { min.ToString(), min },
            { hr.ToString(), hr },
            { day.ToString(), day }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Time(string name, double conversionFactor) : base(name, conversionFactor) { }

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
