using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all mass unit functions
    /// </summary>
    public class Mass : AbstractUnit
    {
        /// <summary>
        /// grams
        /// </summary>
        public static readonly Mass g = new Mass("g", 1);
        /// <summary>
        /// pounds mass
        /// </summary>
        public static readonly Mass lbsm = new Mass("lbs\x208m", 0.00220462);
        /// <summary>
        /// kilograms
        /// </summary>
        public static readonly Mass kg = new Mass("kg", 0.001);
        /// <summary>
        /// metric tons
        /// </summary>
        public static readonly Mass MetricTon = new Mass("Metric Ton", 1e-6);
        /// <summary>
        /// US tons
        /// </summary>
        public static readonly Mass USTon = new Mass("US Ton", 1.1023e-6);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Mass> StringToUnit = new Dictionary<string, Mass>
        {
            { g.ToString(), g },
            { lbsm.ToString(), lbsm },
            { kg.ToString(), kg },
            { MetricTon.ToString(), MetricTon },
            { USTon.ToString(), USTon }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Mass(string name, double conversionFactor)
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
