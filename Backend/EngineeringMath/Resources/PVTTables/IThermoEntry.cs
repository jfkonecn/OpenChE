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

    public class ThermoEntry : IThermoEntry
    {
        public ThermoEntry(Region region, double temperature, 
            double pressure, double specificVolume, double internalEnergy, double enthalpy, 
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporFraction = 0, double liquidFraction = 0, double solidFraction = 0)
        {
            // if the fraction is set, then we should throw an error to let people know that they made a mistake with the region
            if (region == Region.Solid && solidFraction == 0)
            {
                solidFraction = 1;
            }
            else if (region == Region.Liquid && liquidFraction == 0)
            {
                liquidFraction = 1;
            }
            else if (region == Region.Vapor && vaporFraction == 0)
            {
                vaporFraction = 1;
            }
            ValidateFractions(region, vaporFraction, liquidFraction, solidFraction);
            Region = region;
            VaporFraction = vaporFraction;
            LiquidFraction = liquidFraction;
            SolidFraction = solidFraction;
            Temperature = temperature;
            Pressure = pressure;
            SpecificVolume = specificVolume;
            InternalEnergy = internalEnergy;
            Enthalpy = enthalpy;
            Entropy = entropy;
            IsochoricHeatCapacity = isochoricHeatCapacity;
            IsobaricHeatCapacity = isobaricHeatCapacity;
            SpeedOfSound = speedOfSound;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="BadPhaseFractionComposition"></exception>
        private void ValidateFractions(Region region, double vaporFraction, double liquidFraction, double solidFraction)
        {
            if (vaporFraction < 0 || vaporFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(vaporFraction));
            }
            else if (liquidFraction < 0 || liquidFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(liquidFraction));
            }
            else if (solidFraction < 0 || solidFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(solidFraction));
            }

            double sumFraction = vaporFraction + liquidFraction + solidFraction;
            if (sumFraction != 0 && sumFraction != 1)
            {
                throw new BadPhaseFractionComposition();
            }

            Region temp = region;


            if(vaporFraction != 0 && liquidFraction != 0 && solidFraction != 0)
            {
                temp = Region.SolidLiquidVapor;
            }
            else if(vaporFraction != 0 && liquidFraction != 0 && solidFraction == 0)
            {
                temp = Region.LiquidVapor;
            }
            else if (vaporFraction != 0 && liquidFraction == 0 && solidFraction != 0)
            {
                temp = Region.SolidVapor;
            }
            else if (vaporFraction != 0 && liquidFraction == 0 && solidFraction == 0)
            {
                temp = Region.Vapor;
            }
            else if (vaporFraction == 0 && liquidFraction != 0 && solidFraction != 0)
            {
                temp = Region.SolidLiquid;
            }
            else if (vaporFraction == 0 && liquidFraction != 0 && solidFraction == 0)
            {
                temp = Region.Liquid;
            }
            else if (vaporFraction == 0 && liquidFraction == 0 && solidFraction != 0)
            {
                temp = Region.Solid;
            }
            if(temp != region)
            {
                throw new BadPhaseFractionComposition();
            }
        }

        public Region Region { get; }

        public double VaporFraction { get; }

        public double LiquidFraction { get; }

        public double SolidFraction { get; }

        public double Temperature { get; }

        public double Pressure { get; }

        public double InternalEnergy { get; }

        public double Enthalpy { get; }

        public double Entropy { get; }

        public double IsochoricHeatCapacity { get; }

        public double IsobaricHeatCapacity { get; }

        public double SpecificVolume { get; }

        public double SpeedOfSound { get; }

        public double Density { get { return 1.0 / SpecificVolume; } }
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
