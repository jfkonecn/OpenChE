using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Units
{
    /// <summary>
    /// All units inherit from this class in order to group different units into the same data structure 
    /// </summary>
    public abstract class AbstractUnit
    {
        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        protected AbstractUnit(string name, double conversionFactor)
        {
            this.ConversionFactor = conversionFactor;
            this.Name = name;
        }

        protected string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// <para>Example: If 1 ft is the standard then 12 would be the inch's conversionFactor and 1 would be the foot's conversionFactor</para>
        /// <para>Note that the standard is picked within the class which inherits this class </para>
        /// </summary>
        public double ConversionFactor { protected set; get; }

        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnitName">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public abstract double ConvertTo(double curValue, string desiredUnitName);

        /// <summary>
        /// Converts from unit represented by the string curUnitName to "this" object
        /// </summary>
        /// <param name="curValue">The value in curUnitName units</param>
        /// <param name="curUnitName">The current unit</param>
        /// <returns>The curValue in the desired units</returns>
        public double ConvertFrom(double curValue, string curUnitName)
        {
            return StaticUnitProperties.AllUnits[this.GetType()][curUnitName].ConvertTo(curValue, this.ToString());
        }

    }
}
