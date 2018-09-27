using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
    public interface IThermoEntry
    {
        Region Region { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        double VaporFraction { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        double LiquidFraction { get; }
        /// <summary>
        /// between 0 and 1
        /// </summary>
        double SolidFraction { get; }
        /// <summary>
        /// In Kelvin
        /// </summary>
        double Temperature { get; }
        /// <summary>
        /// In Pa
        /// </summary>
        double Pressure { get; }
        /// <summary>
        /// m3/kg
        /// </summary>
        double SpecificVolume { get; }
        /// <summary>
        /// In J/kg
        /// </summary>
        double InternalEnergy { get; }
        /// <summary>
        /// In J/kg
        /// </summary>
        double Enthalpy { get; }
        /// <summary>
        /// J/(kg*K)
        /// </summary>
        double Entropy { get; }
        /// <summary>
        /// Cv, Heat Capacity at constant volume (J/(kg*K))
        /// </summary>
        double IsochoricHeatCapacity { get; }
        /// <summary>
        /// Cp, Heat Capacity at constant pressure (J/(kg*K))
        /// </summary>
        double IsobaricHeatCapacity { get; }
        /// <summary>
        /// m/s
        /// </summary>
        double SpeedOfSound { get; }
        /// <summary>
        /// In kg/m3
        /// </summary>
        double Density { get; }
    }

    public class BadPhaseFractionComposition : ArgumentException { }

    public enum Region
    {
        /// <summary>
        /// Temperature and pressure is above the critical point
        /// </summary>
        SupercriticalFluid,
        /// <summary>
        /// Above critical temperature, but is below the critical pressure
        /// </summary>
        Gas,
        /// <summary>
        /// Pressure is less than both the sublimation and vaporization curve and is below the critical temperature
        /// </summary>
        Vapor,
        /// <summary>
        /// Pressure is above the vaporization curve and the temperature is greater than the fusion curve and less than the critical temperature
        /// </summary>
        Liquid,
        /// <summary>
        /// Pressure is above the sublimation curve and temperature is less than the fusion curve
        /// </summary>
        Solid,
        SolidLiquid,
        LiquidVapor,
        SolidVapor,
        SolidLiquidVapor
    }
}
