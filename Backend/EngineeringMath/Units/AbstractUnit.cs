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
        /// Converts from "this" object to the one represented by the unit object
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnit">The type of unit which we are converting to</param>
        /// <returns></returns>
        public abstract double ConvertTo(double curValue, AbstractUnit desiredUnit);

        /// <summary>
        /// Converts from unit represented by the string curUnitName to "this" object
        /// </summary>
        /// <param name="curValue">The value in curUnit units</param>
        /// <param name="curUnit">The type of unit which we converting from</param>
        /// <returns></returns>
        public double ConvertFrom(double curValue, AbstractUnit curUnit)
        {
            return curUnit.ConvertTo(curValue, this);
        }
    }
}
