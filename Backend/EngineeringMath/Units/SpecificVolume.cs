using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class SpecificVolume : AbstractUnit
    {
        /// <summary>
        /// m3 / kg
        /// </summary>
        public static readonly SpecificVolume m3kg = new SpecificVolume($"{Volume.m3}/{Mass.kg}", Volume.m3.ConversionFactor / Mass.kg.ConversionFactor);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { m3kg.ToString(), m3kg },
        };

        /// <summary>
        /// Conversion factors are all determined by Energy / (kg * K)
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private SpecificVolume(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (SpecificVolume)desiredUnit);
        }


        /// <summary>
        /// Converts between two different SpecificVolume units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current SpecificVolume unit of "value"</param>
        /// <param name="desiredUnit">Desired SpecificVolume unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, SpecificVolume currentUnit, SpecificVolume desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
