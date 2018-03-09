using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Units
{
    class VolumeExpansivity : AbstractUnit
    {
        /// <summary>
        /// 1 / K
        /// </summary>
        public static readonly VolumeExpansivity Kinv = new VolumeExpansivity($"1/{Temperature.K}", 1 / Temperature.K.ConversionFactor);

        /// <summary>
        /// 1 / R
        /// </summary>
        public static readonly VolumeExpansivity Rinv = new VolumeExpansivity($"1/{Temperature.R}", 1 / Temperature.R.ConversionFactor);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { Kinv.ToString(), Kinv },
            { Rinv.ToString(), Rinv }
        };

        /// <summary>
        /// Conversion factors are all determined by Mass / Volume
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        protected VolumeExpansivity(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (VolumeExpansivity)desiredUnit);
        }


        /// <summary>
        /// Converts between two different Volume Expansivity units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current Volume Expansivity unit of "value"</param>
        /// <param name="desiredUnit">Desired Volume Expansivity unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, VolumeExpansivity currentUnit, VolumeExpansivity desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
