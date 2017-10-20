using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all temperature unit functions
    /// </summary>
    public class Temperature
    {

        /// <summary>
        /// Kelvin
        /// </summary>
        public static readonly Temperature K = new Temperature(1);
        /// <summary>
        /// Rankine
        /// </summary>
        public static readonly Temperature R = new Temperature(1.8);

        /// <summary>
        /// Celsius
        /// </summary>
        // Celsius is NOT on an absolute scale!!!!!!! Which means there is no proper "conversion factor"
        // This entry should never be used
        // leave this negative infinity to make the error extremely clear
        public static readonly Temperature C = new Temperature(-Double.MaxValue);

        /// <summary>
        /// Fahrenheit
        /// </summary>
        // Fahrenheit is NOT on an absolute scale!!!!!!! Which means there is no proper "conversion factor"
        // This entry should never be used
        // leave this negative infinity to make the error extremely clear
        public static readonly Temperature F = new Temperature(-Double.MaxValue);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Temperature> StringToUnit = new Dictionary<string, Temperature>
        {
            { "°C", C },
            { "°F", F },
            { "K", K },
            { "°R", R }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="conversionFactor"></param>
        private Temperature(double conversionFactor)
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
        /// Converts between two different temperature units
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current temperature unit of "value"</param>
        /// <param name="desiredUnit">Desired temperature unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public static double Convert(double value, Temperature currentUnit, Temperature desiredUnit)
        {

            // number which is added to Celsius to make it Kelvin
            const double CToK = 273.15;

            // number which is added to Fahrenheit to make it Rankine
            const double FToR = 459.67;



            if(C == currentUnit)
            {
                // convert to absolute scale, Kelvin
                currentUnit = K;
                value += CToK;
            }
            else if (F == currentUnit)
            {
                // convert to absolute scale, Rankine
                currentUnit = R;
                value += FToR;
            }


            // force a conversion between absolute temperature scales
            Temperature tempUnit = desiredUnit;
            if (C == desiredUnit)
            {
                // convert to absolute scale, Kelvin
                tempUnit = K;
            }
            else if (F == desiredUnit)
            {
                // convert to absolute scale, Rankine
                tempUnit = R;
            }

            value = HelperFunctions.Converter(value, currentUnit.ConversionFactor, tempUnit.ConversionFactor);

            // Convert back from absolute scale 
            // convert Kelvin to Celsius
            if (desiredUnit == C)
            {
                if (K != tempUnit)
                {
                    // something when wrong... should be Kelvin
                    throw new InvalidOperationException();
                }

                value -= CToK;
            }
            // convert Rankine to Fahrenheit
            else if (desiredUnit == F)
            {
                if (R != tempUnit)
                {
                    // something when wrong... should be Rankine
                    throw new InvalidOperationException();
                }

                value -= FToR;
            }

            return value;
        }
    }
}
