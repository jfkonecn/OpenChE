using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class Entropy : AbstractUnit
    {
        /// <summary>
        /// (kJ/(kg*K))
        /// </summary>
        public static readonly Entropy kJkgK = new Entropy($"{Energy.kJ}/({Mass.kg}*{Temperature.K})", Energy.kJ.ConversionFactor / (Mass.kg.ConversionFactor * Temperature.K.ConversionFactor));


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kJkgK.ToString(), kJkgK },
        };

        /// <summary>
        /// Conversion factors are all determined by Mass / Volume
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Entropy(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Entropy)desiredUnit);
        }


        /// <summary>
        /// Converts between two different Entropy units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current SpecificVolume unit of "value"</param>
        /// <param name="desiredUnit">Desired SpecificVolume unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Entropy currentUnit, Entropy desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
