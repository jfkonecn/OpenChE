using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
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


            if (vaporFraction != 0 && liquidFraction != 0 && solidFraction != 0)
            {
                temp = Region.SolidLiquidVapor;
            }
            else if (vaporFraction != 0 && liquidFraction != 0 && solidFraction == 0)
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
            if (temp != region)
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
}
