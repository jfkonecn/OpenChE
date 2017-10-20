using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all pressure unit functions
    /// </summary>
    public class Pressure
    {

        /// <summary>
        /// Pascal
        /// </summary>
        public static readonly Pressure Pa = new Pressure(1);
        /// <summary>
        /// kilopascals
        /// </summary>
        public static readonly Pressure kPa = new Pressure(0.001);
        /// <summary>
        /// pounds per square inch
        /// </summary>
        public static readonly Pressure psi = new Pressure(0.000145038);
        /// <summary>
        /// atmospheres
        /// </summary>
        public static readonly Pressure atm = new Pressure(9.8692e-6);
        /// <summary>
        /// bar
        /// </summary>
        public static readonly Pressure bar = new Pressure(1e-5);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Pressure> StringToUnit = new Dictionary<string, Pressure>
        {
            { "kPa", kPa },
            { "Pa", Pa },
            { "psi", psi },
            { "atm", atm },
            { "bar", bar }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="conversionFactor"></param>
        private Pressure(double conversionFactor)
        {
            this.ConversionFactor = conversionFactor;
            
        }


        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// <para>Example: If 1 ft is the standard then 12 would be the inch's conversionFactor and 1 would be the foot's conversionFactor</para>
        /// <para>Note that the standard is picked within the class which inherits this class </para>
        /// </summary>
        private double ConversionFactor { set; get; }

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
