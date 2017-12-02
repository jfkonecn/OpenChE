using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class Area : AbstractUnit
    {
        /*
         * Since area is based on length, there is no conversion factor of 1
         */

        /// <summary>
        /// square meters
        /// </summary>
        public static readonly Area m2 = new Area("m\xB2", Math.Pow(Length.m.ConversionFactor, 2));

        /// <summary>
        /// square feet
        /// </summary>
        public static readonly Area ft2 = new Area("ft\xB2", Math.Pow(Length.ft.ConversionFactor, 2));

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { m2.ToString(), m2 },
            { ft2.ToString(), ft2 }
        };

        /// <summary>
        /// Based on length^3
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Area(string name, double conversionFactor) : base(name, conversionFactor) { }


        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">Type of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Area)desiredUnit);
        }


        /// <summary>
        /// Conversion factors are all determined by Length ^ 3
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current volume unit of "value"</param>
        /// <param name="desiredUnit">Desired volume unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Area currentUnit, Area desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
