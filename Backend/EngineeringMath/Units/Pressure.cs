using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Units
{
    /// <summary>
    /// Contains all pressure unit functions
    /// </summary>
    public class Pressure : AbstractUnit
    {

        /// <summary>
        /// Pascal
        /// </summary>
        public static readonly Pressure Pa = new Pressure("Pa", 1);
        /// <summary>
        /// kilopascals
        /// </summary>
        public static readonly Pressure kPa = new Pressure("kPa", 0.001);
        /// <summary>
        /// pounds per square inch
        /// </summary>
        public static readonly Pressure psia = new Pressure("psi\u2090", 0.000145038);
        /// <summary>
        /// atmospheres
        /// </summary>
        public static readonly Pressure atm = new Pressure("atm", 9.8692e-6);
        /// <summary>
        /// bar
        /// </summary>
        public static readonly Pressure bar = new Pressure("bar", 1e-5);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kPa.ToString(), kPa },
            { Pa.ToString(), Pa },
            { psia.ToString(), psia },
            { atm.ToString(), atm },
            { bar.ToString(), bar }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Pressure(string name, double conversionFactor) : base(name, conversionFactor) { }

        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">Type of desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Pressure)desiredUnit);
        }


        /// <summary>
        /// Converts between two different pressure units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current pressure unit of "value"</param>
        /// <param name="desiredUnit">Desired pressure unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Pressure currentUnit, Pressure desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }



    }
}
