using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class Energy : AbstractUnit
    {
        /// <summary>
        /// Kilojoules
        /// </summary>
        public static readonly Energy kJ = new Energy("kJ", 1);

        /// <summary>
        /// Joules
        /// </summary>
        public static readonly Energy J = new Energy("J", 1000);

        /// <summary>
        /// Kilocalorie
        /// </summary>
        public static readonly Energy kCal = new Energy("kCal", 0.239006);

        /// <summary>
        /// British Thermal Unit
        /// </summary>
        public static readonly Energy BTU = new Energy("BTU", 0.947817);

        /// <summary>
        /// US Therm
        /// </summary>
        public static readonly Energy Therm = new Energy("US Therm", 9.4804e-6);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kJ.ToString(), kJ },
            { J.ToString(), J },
            { kCal.ToString(), kCal },
            { BTU.ToString(), BTU },
            { Therm.ToString(), Therm }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Energy(string name, double conversionFactor) : base(name, conversionFactor) { }


        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">Type of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Energy)desiredUnit);
        }


        /// <summary>
        /// Converts between two different volume units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current volume unit of "value"</param>
        /// <param name="desiredUnit">Desired volume unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Energy currentUnit, Energy desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
