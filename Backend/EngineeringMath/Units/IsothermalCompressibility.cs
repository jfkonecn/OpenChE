using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    public class IsothermalCompressibility : AbstractUnit
    {
        /// <summary>
        /// 1 / Pa
        /// </summary>
        public static readonly IsothermalCompressibility PaInv = new IsothermalCompressibility($"1/{Pressure.Pa}", 1 / Pressure.Pa.ConversionFactor);

        /// <summary>
        /// 1 / psia
        /// </summary>
        public static readonly IsothermalCompressibility PsiaInv = new IsothermalCompressibility($"1/{Pressure.psia}", 1 / Pressure.psia.ConversionFactor);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { PaInv.ToString(), PaInv },
            { PsiaInv.ToString(), PsiaInv }
        };

        /// <summary>
        /// Conversion factors are all determined by Mass / Volume
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        protected IsothermalCompressibility(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (IsothermalCompressibility)desiredUnit);
        }


        /// <summary>
        /// Converts between two different Isothermal Compressibility units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current Isothermal Compressibility unit of "value"</param>
        /// <param name="desiredUnit">Desired Isothermal Compressibility unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, IsothermalCompressibility currentUnit, IsothermalCompressibility desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
