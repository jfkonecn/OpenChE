using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class Power : AbstractUnit
    {
        /// <summary>
        /// Kilowatt
        /// </summary>
        public static readonly Power kW = new Power("kW", 1);

        /// <summary>
        /// Watt
        /// </summary>
        public static readonly Power W = new Power("W", 1e3);

        /// <summary>
        /// Mechanical Horsepower
        /// </summary>
        public static readonly Power hp = new Power("hp", 1.34102);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kW.ToString(), kW },
            { W.ToString(), W },
            { hp.ToString(), hp }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Power(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">Type of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Power)desiredUnit);
        }


        /// <summary>
        /// Converts between two different mass units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current power unit of "value"</param>
        /// <param name="desiredUnit">Desired power unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Power currentUnit, Power desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
