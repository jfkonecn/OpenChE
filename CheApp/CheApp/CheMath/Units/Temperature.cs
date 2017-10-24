using System;
using System.Collections.Generic;
using System.Text;

namespace CheApp.CheMath.Units
{
    /// <summary>
    /// Contains all temperature unit functions
    /// </summary>
    public class Temperature : AbstractUnit
    {

        /// <summary>
        /// Kelvin
        /// </summary>
        public static readonly Temperature K = new Temperature("K", 1);
        /// <summary>
        /// Rankine
        /// </summary>
        public static readonly Temperature R = new Temperature("°R", 1.8);

        /// <summary>
        /// Celsius
        /// </summary>
        // Celsius is NOT on an absolute scale!!!!!!! Which means there is no proper "conversion factor"
        // This entry should never be used
        // leave this negative infinity to make the error extremely clear
        public static readonly Temperature C = new Temperature("°C", -Double.MaxValue);

        /// <summary>
        /// Fahrenheit
        /// </summary>
        // Fahrenheit is NOT on an absolute scale!!!!!!! Which means there is no proper "conversion factor"
        // This entry should never be used
        // leave this negative infinity to make the error extremely clear
        public static readonly Temperature F = new Temperature("°F", - Double.MaxValue);

        /// <summary>
        /// Relates all units to a string representation
        /// </summary>
        public static readonly Dictionary<string, Temperature> StringToUnit = new Dictionary<string, Temperature>
        {
            { C.ToString(), C },
            { F.ToString(), F },
            { K.ToString(), K },
            { R.ToString(), R }
        };

        /// <summary>
        /// The equivalent of 1 unit equal to the standard. (The standard's Conversion Factor is equal to 1)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="conversionFactor"></param>
        private Temperature(string name, double conversionFactor)
        {
            this.ConversionFactor = conversionFactor;
            this.Name = name;
        }


        /// <summary>
        /// Converts from "this" object to the one represented by the string
        /// </summary>
        /// <param name="curValue">The value in "this" units</param>
        /// <param name="desiredUnitName">String name od desired unit</param>
        /// <returns>The curValue in the desired units</returns>
        public override double ConvertTo(double curValue, string desiredUnitName)
        {
            return Convert(curValue, this, StringToUnit[desiredUnitName]);
        }



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
