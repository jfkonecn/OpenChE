using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
    public interface IPVTTable
    {
        /// <summary>
        /// Gets ThermoEntry at passed pressure and passed temperature. Null when no entry found.
        /// </summary>
        /// <param name="temperature">Desired Temperture (K)</param>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <returns></returns>
        IThermoEntry GetThermoEntryAtTemperatureAndPressure(double temperature, double pressure);

        /// <summary>
        /// Gets ThermoEntry for saturated liquid or vapor at passed pressure. Null when no entry found.
        /// </summary>
        /// <param name="pressure">Desired Pressure (Pa)</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        IThermoEntry GetThermoEntryAtSatPressure(double pressure, SaturationRegion phase);

        /// <summary>
        /// Gets ThermoEntry and Pressure for saturated liquid or vapor at passed satTemp. Null when no entry found.
        /// </summary>
        /// <param name="satTemp">Desired saturation temperature</param>
        /// <param name="phase"></param>
        /// <returns></returns>
        IThermoEntry GetThermoEntryAtSatTemp(double satTemp, SaturationRegion phase);

        /// <summary>
        /// Gets the ThermoEntry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">J/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        IThermoEntry GetThermoEntryAtEntropyAndPressure(double entropy, double pressure);

        /// <summary>
        /// Gets the ThermoEntry which matches the entropy and pressure passed
        /// </summary>
        /// <param name="entropy">kJ/(kg*K)</param>
        /// <param name="pressure">Pa</param>
        /// <returns></returns>
        IThermoEntry GetThermoEntryAtEnthapyAndPressure(double enthalpy, double pressure);
        /// <summary>
        /// In K
        /// </summary>
        double CriticalTemperature { get; }
        /// <summary>
        /// In Pa
        /// </summary>
        double CriticalPressure { get; }
        /// <summary>
        /// Max temperature given pressure
        /// </summary>
        /// <returns></returns>
        double MaxTemperature { get; }
        /// <summary>
        /// Min temperature given pressure
        /// </summary>
        /// <returns></returns>
        double MinTemperature { get; }
        /// <summary>
        /// Max pressure given temperature
        /// </summary>
        /// <returns></returns>
        double MaxPressure { get; }
        /// <summary>
        /// Min pressure given temperature
        /// </summary>
        /// <returns></returns>
        double MinPressure { get; }
    }

    public enum SaturationRegion
    {
        /// <summary>
        /// Pressure is less than both the sublimation and vaporization curve and is below the critical temperature
        /// </summary>
        Vapor = Region.Vapor,
        /// <summary>
        /// Pressure is above the vaporization curve and the temperature is greater than the fusion curve and less than the critical temperature
        /// </summary>
        Liquid = Region.Liquid,
        /// <summary>
        /// Pressure is above the sublimation curve and temperature is less than the fusion curve
        /// </summary>
        Solid = Region.Solid
    }
}
