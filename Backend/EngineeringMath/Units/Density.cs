﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Units
{
    public class Density : AbstractUnit
    {

        /*
        * Since density is based on Mass / Volume, there is no conversion factor of 1
        */

        /// <summary>
        /// kg / m3
        /// </summary>
        public static readonly Density kgm3 = new Density($"{Mass.kg}/{Volume.m3}", Mass.kg.ConversionFactor / Volume.m3.ConversionFactor);


        /// <summary>
        /// lbsm / ft3
        /// </summary>
        public static readonly Density lbsft3 = new Density($"{Mass.lbsm}/{Volume.ft3}", Mass.lbsm.ConversionFactor / Volume.ft3.ConversionFactor);


        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, AbstractUnit> StringToUnit = new Dictionary<string, AbstractUnit>
        {
            { kgm3.ToString(), kgm3 },
            { lbsft3.ToString(), lbsft3 }
        };

        /// <summary>
        /// Conversion factors are all determined by Mass / Volume
        /// </summary>
        /// <param name="name">string name of the unit</param>
        /// <param name="conversionFactor"></param>
        private Density(string name, double conversionFactor) : base(name, conversionFactor) { }



        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, AbstractUnit desiredUnit)
        {
            return Convert(curValue, this, (Density)desiredUnit);
        }


        /// <summary>
        /// Converts between two different density units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current density unit of "value"</param>
        /// <param name="desiredUnit">Desired density unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Density currentUnit, Density desiredUnit)
        {
            return HelperFunctions.Converter(value, currentUnit.ConversionFactor, desiredUnit.ConversionFactor);
        }
    }
}
