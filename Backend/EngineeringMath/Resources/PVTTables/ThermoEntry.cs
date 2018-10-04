using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EngineeringMath.Resources.PVTTables
{
    public class ThermoEntry : IThermoEntry
    {
        public ThermoEntry(Region region, double temperature,
            double pressure, double specificVolume, double internalEnergy, double enthalpy,
            double entropy, double isochoricHeatCapacity, double isobaricHeatCapacity,
            double speedOfSound,
            double vaporMassFraction = 0, double liquidMassFraction = 0, double solidMassFraction = 0)
        {
            // if the fraction is set, then we should throw an error to let people know that they made a mistake with the region
            if (region == Region.Solid && solidMassFraction == 0)
            {
                solidMassFraction = 1;
            }
            else if (region == Region.Liquid && liquidMassFraction == 0)
            {
                liquidMassFraction = 1;
            }
            else if (region == Region.Vapor && vaporMassFraction == 0)
            {
                vaporMassFraction = 1;
            }
            ValidateFractions(region, vaporMassFraction, liquidMassFraction, solidMassFraction);
            Region = region;
            VaporMassFraction = vaporMassFraction;
            LiquidMassFraction = liquidMassFraction;
            SolidMassFraction = solidMassFraction;
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

        private ThermoEntry(IThermoEntry vapEntry, IThermoEntry liqEntry, IThermoEntry solidEntry, 
            double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            if (vapEntry != null && vapEntry.Region != Region.Vapor)
                throw new ArgumentOutOfRangeException(nameof(vapEntry), "Must be vapor!");
            if (liqEntry != null && liqEntry.Region != Region.Liquid)
                throw new ArgumentOutOfRangeException(nameof(liqEntry), "Must be liquid!");
            if (solidEntry != null && solidEntry.Region != Region.Solid)
                throw new ArgumentOutOfRangeException(nameof(solidEntry), "Must be solid!");
            if (vaporMassFraction + liquidMassFraction + solidMassFraction != 1)
                throw new ArgumentException("Fractions do not up to 1!");

            VaporMassFraction = vaporMassFraction;
            LiquidMassFraction = liquidMassFraction;
            SolidMassFraction = solidMassFraction;

            if (vapEntry != null && liqEntry != null && solidEntry != null)
            {
                Region = Region.SolidLiquidVapor;
            }
            else if (vapEntry != null && liqEntry != null)
            {
                Region = Region.LiquidVapor;
            }
            else if (liqEntry != null && solidEntry != null)
            {
                Region = Region.SolidLiquid;
            }
            else if (vapEntry != null && solidEntry != null)
            {
                Region = Region.SolidVapor;
            }
            else if (vapEntry != null)
            {
                Region = Region.Vapor;
            }
            else if (liqEntry != null)
            {
                Region = Region.Liquid;
            }
            else if (solidEntry != null)
            {
                Region = Region.Solid;
            }
            else
            {
                throw new ArgumentException("All entries are null!");
            }
            InterpolateProperties(nameof(IThermoEntry.Temperature), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Pressure), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.SpecificVolume), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.InternalEnergy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Enthalpy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.Entropy), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.IsochoricHeatCapacity), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.IsobaricHeatCapacity), vapEntry, liqEntry, solidEntry);
            InterpolateProperties(nameof(IThermoEntry.SpeedOfSound), vapEntry, liqEntry, solidEntry);
        }

        private void InterpolateProperties(string propName,
            IThermoEntry vapEntry, IThermoEntry liqEntry, IThermoEntry solidEntry)
        {
            PropertyInfo propInfo = typeof(ThermoEntry).GetProperty(propName);
            double num = 0;
            if(vapEntry != null)
                num += (double)propInfo.GetValue(vapEntry) * VaporMassFraction;
            if (liqEntry != null)
                num += (double)propInfo.GetValue(liqEntry) * LiquidMassFraction;
            if (solidEntry != null)
                num += (double)propInfo.GetValue(solidEntry) * SolidMassFraction;
            propInfo.SetValue(this, num);
        }


        public static IThermoEntry BuildLiquidVaporEntry(IThermoEntry vapEntry, IThermoEntry liqEntry, double liquidFraction)
        {
            return new ThermoEntry(vapEntry, liqEntry, null, 1.0 - liquidFraction, liquidFraction, 0.0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="BadPhaseFractionComposition"></exception>
        private void ValidateFractions(Region region, double vaporMassFraction, double liquidMassFraction, double solidMassFraction)
        {
            if (vaporMassFraction < 0 || vaporMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(vaporMassFraction));
            }
            else if (liquidMassFraction < 0 || liquidMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(liquidMassFraction));
            }
            else if (solidMassFraction < 0 || solidMassFraction > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(solidMassFraction));
            }

            double sumFraction = vaporMassFraction + liquidMassFraction + solidMassFraction;
            if (sumFraction != 0 && sumFraction != 1)
            {
                throw new BadPhaseFractionComposition();
            }

            Region temp = region;


            if (vaporMassFraction != 0 && liquidMassFraction != 0 && solidMassFraction != 0)
            {
                temp = Region.SolidLiquidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction != 0 && solidMassFraction == 0)
            {
                temp = Region.LiquidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction == 0 && solidMassFraction != 0)
            {
                temp = Region.SolidVapor;
            }
            else if (vaporMassFraction != 0 && liquidMassFraction == 0 && solidMassFraction == 0)
            {
                temp = Region.Vapor;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction != 0 && solidMassFraction != 0)
            {
                temp = Region.SolidLiquid;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction != 0 && solidMassFraction == 0)
            {
                temp = Region.Liquid;
            }
            else if (vaporMassFraction == 0 && liquidMassFraction == 0 && solidMassFraction != 0)
            {
                temp = Region.Solid;
            }
            if (temp != region)
            {
                throw new BadPhaseFractionComposition();
            }
        }

        public Region Region { get; protected set; }

        public double VaporMassFraction { get; protected set; }

        public double LiquidMassFraction { get; protected set; }

        public double SolidMassFraction { get; protected set; }

        public double Temperature { get; protected set; }

        public double Pressure { get; protected set; }

        public double InternalEnergy { get; protected set; }

        public double Enthalpy { get; protected set; }

        public double Entropy { get; protected set; }

        public double IsochoricHeatCapacity { get; protected set; }

        public double IsobaricHeatCapacity { get; protected set; }

        public double SpecificVolume { get; protected set; }

        public double SpeedOfSound { get; protected set; }

        public double Density { get { return 1.0 / SpecificVolume; } }
    }
}
