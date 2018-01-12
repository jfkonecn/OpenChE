using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Resources.LookupTables.ThermoTableElements
{
    /// <summary>
    /// Specific Volume, Enthalpy and Entropy at a given temperature
    /// </summary>
    public class ThermoEntry
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="temperature">Temperture this entry is recorded at (in C)</param>
        /// <param name="v">Specific Volume (m3/kg)</param>
        /// <param name="h">Enthalpy (kJ/kg)</param>
        /// <param name="s">Entropy (kJ/(kg*K))</param>
        public ThermoEntry(double temperature, double v, double h, double s)
        {
            Temperature = temperature;
            V = v;
            H = h;
            S = s;
        }

        /// <summary>Temperture this entry is recorded at (in C)</summary>
        public readonly double Temperature;
        /// <summary>
        /// Specific Volume (m3/kg)
        /// </summary>
        public readonly double V;
        /// <summary>
        /// Enthalpy (kJ/kg)
        /// </summary>
        public readonly double H;
        /// <summary>
        /// Entropy (kJ/(kg*K))
        /// </summary>
        public readonly double S;

        internal class Interpolation<T>
        {
            /// <summary>
            /// Returns the value to be interpolated from object of type T
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            internal delegate double ExtractInterpolatedValue(T obj);
            /// <summary>
            /// Converts object T to ThermoEntry 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            internal delegate ThermoEntry ObjectToThermoEntry(T obj);

            /// <summary>
            /// Interpolates a list of T objects to create a ThermoEntry. Null when no entry found.
            /// </summary>
            /// <param name="num">The double which is to interpolated to</param>
            /// <param name="ObjCollection">Collect of objection which will be used to create an interpolated ThermoEntry</param>
            /// <param name="getInterValue">Gets a double out of object T which will be compaired to num</param>
            /// <param name="converter">Converts a T object into its ThermoEntry</param>
            /// <returns></returns>
            internal static ThermoEntry InterpolationThermoEntryFromList(double num, 
                List<T> ObjCollection, 
                ExtractInterpolatedValue getInterValue, 
                ObjectToThermoEntry converter)
            {
                T curObj = ObjCollection.FirstOrDefault(x => getInterValue(x) == num);
                
                if (curObj != null)
                {
                    return converter(curObj);
                }
                else
                {
                    // else interpolate
                    // Interpolated Specific Volume
                    double interV,
                        // Interpolated Enthalpy
                        interH,
                        // Interpolated Entropy
                        interS,
                        // Interpolated Temperature
                        interT,
                        lowValue = double.MinValue,
                        highValue = double.MaxValue;

                    // find the two closest entries in
                    T lowValueObject = default(T),
                        highValueObject = default(T);

                    foreach (T obj in ObjCollection)
                    {
                        double curValue = getInterValue(obj);
                        if (curValue > lowValue && curValue < num)
                        {
                            lowValue = curValue;
                            lowValueObject = obj;
                        }
                        else if (curValue < highValue && curValue > num)
                        {
                            highValue = curValue;
                            highValueObject = obj;
                        }
                    }


                    if (lowValueObject != null && highValueObject != null)
                    {
                        ThermoEntry lowValueThermoEntry = converter(lowValueObject),
                            highValueThermoEntry = converter(highValueObject);

                        interT = Interpolation.LinearInterpolation(num,
                            lowValue, lowValueThermoEntry.Temperature,
                            highValue, highValueThermoEntry.Temperature);

                        interV = Interpolation.LinearInterpolation(num,
                            lowValue, lowValueThermoEntry.V,
                            highValue, highValueThermoEntry.V);

                        interH = Interpolation.LinearInterpolation(num,
                            lowValue, lowValueThermoEntry.H,
                            highValue, highValueThermoEntry.H);

                        interS = Interpolation.LinearInterpolation(num,
                            lowValue, lowValueThermoEntry.S,
                            highValue, highValueThermoEntry.S);

                        return new ThermoEntry(interT, interV, interH, interS);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
