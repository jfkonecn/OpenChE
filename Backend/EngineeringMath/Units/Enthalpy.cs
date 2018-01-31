using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class Enthalpy : AbstractUnit
    {
        /// <summary>
        /// kJ / kg
        /// </summary>
        public static readonly Enthalpy kJkg = new Enthalpy($"{Energy.kJ}/{Mass.kg}", Energy.kJ.ConversionFactor / Mass.kg.ConversionFactor);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kJkg.ToString(), kJkg },
        };

        /// <summary>
        /// Conversion factors are all determined by Mass / Volume
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Enthalpy(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Enthalpy)desiredUnit);
        }


        /// <summary>
        /// Converts between two different Enthalpy units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current Enthalpy unit of "value"</param>
        /// <param name="desiredUnit">Desired Enthalpy unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Enthalpy currentUnit, Enthalpy desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
