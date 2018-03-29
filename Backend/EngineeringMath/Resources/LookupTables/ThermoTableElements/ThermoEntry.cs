using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.NumericalMethods;

namespace EngineeringMath.Resources.LookupTables.ThermoTableElements
{
    /// <summary>
    /// Specific Volume, Enthalpy and Entropy at a given temperature
    /// </summary>
    public class ThermoEntry
    {
        /// <summary>
        /// Used with interpolation
        /// </summary>
        /// <param name="temperature">Temperture this entry is recorded at (in C)</param>
        /// <param name="pressure">Pressure (Pa)</param>
        /// <param name="v">Specific Volume (m3/kg)</param>
        /// <param name="h">Enthalpy (kJ/kg)</param>
        /// <param name="s">Entropy (kJ/(kg*K))</param>
        /// <param name="phase">Phase of the entry</param>
        /// <param name="isSaturated">True if saturated</param>
        /// <param name="beta">Volume Expansivity or Coefficient of thermal expansion (1/K)</param>
        /// <param name="kappa">Isothermal Compressibility (1/Pa)</param>
        /// <param name="cp">Heat Capacity Constant Pressure (kJ/(kg * K))</param>
        /// <param name="cv">Heat Capacity Constant Volume (kJ/(kg * K))</param>
        private ThermoEntry(double temperature, double pressure,
            double v, double h, double s, Phase phase, bool isSaturated,
            double beta, double kappa,
            double cp, double cv)
        {
            Temperature = temperature;
            Pressure = pressure;
            V = v;
            H = h;
            S = s;
            EntryPhase = phase;
            IsSaturated = isSaturated;
            Beta = beta;
            Kappa = kappa;
            Cp = cp;
            Cv = cv;
        }


        /// <summary>
        /// USE ONLY WHEN BUILDING A THERMO TABLE
        /// <para>CALL this.FinishUp WHEN ALL THERMO ENTRIES ARE ENTERED INTO THE TABLE</para>
        /// </summary>
        /// <param name="temperature">Temperture this entry is recorded at (C)</param>
        /// <param name="pressure">Pressure (Pa)</param>
        /// <param name="v">Specific Volume (m3/kg)</param>
        /// <param name="h">Enthalpy (kJ/kg)</param>
        /// <param name="s">Entropy (kJ/(kg*K))</param>
        /// <param name="phase">phase of this entry</param>
        /// <param name="isSaturated">True if saturated</param>
        /// <param name="beta">Volume Expansivity (1/K)</param>
        /// <param name="kappa">Isothermal Compressibility (1/Pa)</param>
        /// <param name="cp">Heat Capacity Constant Pressure (kJ/(kg * K))</param>
        /// <param name="cv">Heat Capacity Constant Volume (kJ/(kg * K))</param>
        public ThermoEntry(double temperature, double pressure,
            double v, double h, double s, Phase phase, bool isSaturated) : 
            this(temperature, pressure, v, h, s, phase, isSaturated,
                double.NaN, double.NaN, double.NaN, double.NaN)
        {

        }

        /// <summary>
        /// finds beta, kappa, cp and cv for this thermo entry
        /// <para>Call after creating all entries in the table which is entry is located in</para>
        /// </summary>
        /// <param name="dVdTp">Change in volume (m3/kg) over change in temperature (K) at constant pressure at this entry</param>
        /// <param name="dVdPt">Change in volume (m3/kg) over change in pressure (Pa) at constant temperature at this entry</param>
        /// <param name="dHdTp">Change in enthalpy (kJ/kg) over change in temperature (K) at constant pressure at this entry</param>
        internal void FinishUp(double dVdTp, double dVdTt, double dHdTp)
        {
            // make sure the correct constructor was used
            if (!(double.IsNaN(Beta) 
                && double.IsNaN(Kappa) 
                && double.IsNaN(Cp) 
                && double.IsNaN(Cv)))
            {
                throw new Exception("Wrong Constructor Used");
            }

            Beta = dVdTp / V;
            Kappa = -dVdTt / V;
            Cp = dHdTp;

            // temperature must be on an absolute scale
            double num = (V * (Temperature + 273.15) * (Math.Pow(Beta, 2) / (Kappa * 1e3)));

            if(num >= Cp || Kappa == 0 || double.IsInfinity(num))
            {
                // Heat Capcity cannot be less than or equal to zero, Beta and Kappa are probably too small
                Cv = Cp;
            }
            else
            {
                Cv = Cp - num;
            }

            
            
        }

        /// <summary>
        /// Creates a wet vapor entry
        /// </summary>
        /// <param name="satVapor">saturation conditions for vapor phase</param>
        /// <param name="satLiquid">saturation conditions for liquid phase</param>
        /// <param name="vaporQuality">fraction of vapor in the stream</param>
        /// <returns></returns>
        internal static ThermoEntry WetVapor(ThermoEntry satVapor, ThermoEntry satLiquid, double vaporQuality)
        {
            if(satVapor.Pressure != satLiquid.Pressure ||
                satVapor.Temperature != satLiquid.Temperature ||
                !satVapor.IsSaturated || !satLiquid.IsSaturated)
            {
                throw new Exception("must be at the same entry point!");
            }
            else if(vaporQuality > 1 || vaporQuality < 0)
            {
                throw new Exception("Vapor must be between 0 and 1!");
            }

            double liquidQuality = 1 - vaporQuality;

            double temperature = satVapor.Temperature * vaporQuality + satLiquid.Temperature * liquidQuality;

            return new ThermoEntry(
                satVapor.Temperature * vaporQuality + satLiquid.Temperature * liquidQuality,
                satVapor.Pressure * vaporQuality + satLiquid.Pressure * liquidQuality,
                satVapor.V * vaporQuality + satLiquid.V * liquidQuality,
                satVapor.H * vaporQuality + satLiquid.H * liquidQuality,
                satVapor.S * vaporQuality + satLiquid.S * liquidQuality, 
                Phase.vapor, 
                true,
                satVapor.Beta * vaporQuality + satLiquid.Beta * liquidQuality,
                satVapor.Kappa * vaporQuality + satLiquid.Kappa * liquidQuality,
                satVapor.Cp * vaporQuality + satLiquid.Cp * liquidQuality,
                satVapor.Cv * vaporQuality + satLiquid.Cv * liquidQuality);
        }


        /// <summary>
        /// Pressure (Pa)
        /// </summary>
        public readonly double Pressure;
        /// <summary>
        /// Temperture this entry is recorded at (in C)
        /// </summary>
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
        /// <summary>
        /// current phase of this entry
        /// </summary>
        public readonly Phase EntryPhase;
        ///<summary > 
        ///True if saturated
        ///</summary>
        public readonly bool IsSaturated;

        /// <summary>
        /// Volume Expansivity or Coefficient of thermal expansion  (1/K)
        /// </summary>
        public double Beta { get; private set; }
        /// <summary>
        /// Isothermal Compressibility (1/Pa)
        /// </summary>
        public double Kappa { get; private set; }
        /// <summary>
        /// Heat Capacity Constant Pressure (kJ/(kg * K))
        /// </summary>
        public double Cp { get; private set; }
        /// <summary>
        /// Heat Capacity Constant Volume (kJ/(kg * K))
        /// </summary>
        public double Cv { get; private set; }



        public enum Phase
        {
            solid,
            liquid,
            vapor
        }


        internal class Interpolation<T>
        {
            /// <summary>
            /// Returns the value to be interpolated from entry
            /// </summary>
            /// <param name="entry"></param>
            /// <returns></returns>
            internal delegate double ExtractInterpolatedValue(ThermoEntry entry);
            /// <summary>
            /// Converts object T to ThermoEntry 
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            internal delegate ThermoEntry ObjectToThermoEntry(T obj);

            /// <summary>
            /// Gets a thermodynamic entry at a saturation point which shares either the passed entry's pressure or temperature
            /// </summary>
            /// <param name="entry">This entry will share the either the temperature or pressure of the saturation point</param>
            /// <param name="phase"></param>
            /// <returns></returns>
            internal delegate ThermoEntry SaturationFun(ThermoEntry entry, ThermoEntry.Phase phase);

            /// <summary>
            /// Interpolates a list of T objects to create a ThermoEntry. Null when no entry found.
            /// </summary>
            /// <param name="num">The double which is to interpolated to</param>
            /// <param name="ObjCollection">Collect of objection which will be used to create an interpolated ThermoEntry</param>
            /// <param name="getInterValue">Gets a double out of entry which will be compaired to num</param>
            /// <param name="converter">Converts a T object into its ThermoEntry</param>
            /// <param name="SaturationFun">Returns a new saturated entry inbetween two elements in the ObjCollection incase there is a phase change</param>
            /// <returns></returns>
            internal static ThermoEntry InterpolationThermoEntryFromList(double num, 
                List<T> ObjCollection, 
                ExtractInterpolatedValue getInterValue, 
                ObjectToThermoEntry converter,
                SaturationFun SaturationFun)
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
                    // Interpolated Pressure
                    interP,
                    // Interpolated Beta
                    interBeta,
                    // Interpolated Kappa
                    interKappa,
                    // Interpolated Cp
                    interCp,
                    // Interpolated Cv
                    interCv,
                    lowValue = double.MinValue,
                    highValue = double.MaxValue;

                // find the two closest entries in
                T lowValueObject = default(T),
                    highValueObject = default(T);

                foreach (T obj in ObjCollection)
                {
                    ThermoEntry entry = converter(obj);
                    if (entry != null)
                    {
                        double curValue = getInterValue(entry);
                        if (curValue == num)
                        {
                            return entry;
                        }
                        else if (curValue > lowValue && curValue < num)
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
                }


                if (lowValueObject != null && highValueObject != null)
                {
                    ThermoEntry lowValueThermoEntry = converter(lowValueObject),
                        highValueThermoEntry = converter(highValueObject);

                    // check if a phase change occured
                    if (lowValueThermoEntry.EntryPhase != highValueThermoEntry.EntryPhase)
                    {
                        ThermoEntry tempEntry = SaturationFun(lowValueThermoEntry, lowValueThermoEntry.EntryPhase);
                        if (num > getInterValue(tempEntry))
                        {
                            lowValueThermoEntry = tempEntry;
                        }
                        else
                        {
                            // make sure the phase is correct
                            highValueThermoEntry = SaturationFun(highValueThermoEntry, highValueThermoEntry.EntryPhase);
                        }
                    }


                    interP = Interpolation.LinearInterpolation(num,
                        lowValue, lowValueThermoEntry.Pressure,
                        highValue, highValueThermoEntry.Pressure);

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

                    interBeta = Interpolation.LinearInterpolation(num,
                        lowValue, lowValueThermoEntry.Beta,
                        highValue, highValueThermoEntry.Beta);

                    interKappa = Interpolation.LinearInterpolation(num,
                        lowValue, lowValueThermoEntry.Kappa,
                        highValue, highValueThermoEntry.Kappa);

                    interCp = Interpolation.LinearInterpolation(num,
                        lowValue, lowValueThermoEntry.Cp,
                        highValue, highValueThermoEntry.Cp);

                    interCv = Interpolation.LinearInterpolation(num,
                        lowValue, lowValueThermoEntry.Cv,
                        highValue, highValueThermoEntry.Cv);

                    // assume not saturated
                    return new ThermoEntry(interT, interP, interV, interH, interS, lowValueThermoEntry.EntryPhase,
                        lowValueThermoEntry.IsSaturated && highValueThermoEntry.IsSaturated,
                        interBeta, interKappa, interCp, interCv);
                }
                else
                {
                    return null;
                }

            }
        }
    }
}
