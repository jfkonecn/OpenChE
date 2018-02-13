using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Functions;
using System.Collections;
using EngineeringMath.Resources.LookupTables;
using EngineeringMath.Resources.LookupTables.ThermoTableElements;

namespace EngineeringMath.Calculations.Thermo
{
    public class RankineCycle : SimpleFunction
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public RankineCycle() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.steamP, LibraryResources.SteamPressure, new AbstractUnit[] { Pressure.Pa }, true, SteamTable.Table.MinTablePressure, SteamTable.Table.MaxTablePressure),
                    new SimpleParameter((int)Field.steamTemp, LibraryResources.SteamTemp, new AbstractUnit[] { Temperature.C }, true, SteamTable.Table.MinTableTemperature, SteamTable.Table.MaxTableTemperature),
                    new SimpleParameter((int)Field.condenserP, LibraryResources.CondenserPressure, new AbstractUnit[] { Pressure.Pa}, true, SteamTable.Table.MinTablePressure, SteamTable.Table.MaxTablePressure),
                    new SimpleParameter((int)Field.condenserSQ, LibraryResources.CondenserSQ, new AbstractUnit[] { Unitless.unitless}, false, 0, 1),
                    new SimpleParameter((int)Field.condenserH, LibraryResources.CondenserH, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.IdealTurbineQ, LibraryResources.IdealTurbineWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.RealTurbineQ, LibraryResources.RealTurbineWork, new AbstractUnit[] { Enthalpy.kJkg}, false),
                    new SimpleParameter((int)Field.thermoEff, LibraryResources.ThermalEfficiency, new AbstractUnit[] { Unitless.unitless}, false)
                }
            )
        {
            this.Title = LibraryResources.RankineCycle;
            

#if DEBUG
            SteamPressure = 8600e3;
            SteamTemperature = 500;
            CondenserPressure = 10e3;
#endif

        }

        public enum Field
        {
            /// <summary>
            /// Steam Pressure (Pa)
            /// </summary>
            steamP,
            /// <summary>
            /// Steam Temperature (C)
            /// </summary>
            steamTemp,
            /// <summary>
            /// Condenser Pressure (Pa)
            /// </summary>
            condenserP,
            /// <summary>
            /// Condenser Steam Quality Fraction (Unitless)
            /// </summary>
            condenserSQ,
            /// <summary>
            /// Condenser Enthalpy (kJ/kg)
            /// </summary>
            condenserH,
            /// <summary>
            /// Ideal work done by the turbine (kJ/kg)
            /// </summary>
            IdealTurbineQ,
            /// <summary>
            /// Real work done by the turbine (kJ/kg)
            /// </summary>
            RealTurbineQ,
            /// <summary>
            /// Thermal Efficiency Fraction (Unitless)
            /// </summary>
            thermoEff
        };


        /// <summary>
        /// Steam Pressure (Pa)
        /// </summary>
        public double SteamPressure
        {
            get
            {
                return GetParameter((int)Field.steamP).Value;
            }

            set
            {
                GetParameter((int)Field.steamP).Value = value;
            }
        }

        /// <summary>
        /// Steam Temperature (C)
        /// </summary>
        public double SteamTemperature
        {
            get
            {
                return GetParameter((int)Field.steamTemp).Value;
            }

            set
            {
                GetParameter((int)Field.steamTemp).Value = value;
            }
        }

        /// <summary>
        /// Condenser Pressure (Pa)
        /// </summary>
        public double CondenserPressure
        {
            get
            {
                return GetParameter((int)Field.condenserP).Value;
            }

            set
            {
                GetParameter((int)Field.condenserP).Value = value;
            }
        }

        /// <summary>
        /// Condenser Steam Quality (Unitless)
        /// </summary>
        public double CondenserSteamQuality
        {
            get
            {
                return GetParameter((int)Field.condenserSQ).Value;
            }

            set
            {
                GetParameter((int)Field.condenserSQ).Value = value;
            }
        }

        /// <summary>
        /// Condenser Steam Quality (Unitless)
        /// </summary>
        public double CondenserEnthalpy
        {
            get
            {
                return GetParameter((int)Field.condenserH).Value;
            }

            set
            {
                GetParameter((int)Field.condenserH).Value = value;
            }
        }


        /// <summary>
        /// Ideal Turbine Work (kJ/kg)
        /// </summary>
        public double IdealTurbineWork
        {
            get
            {
                return GetParameter((int)Field.IdealTurbineQ).Value;
            }

            set
            {
                GetParameter((int)Field.IdealTurbineQ).Value = value;
            }
        }


        /// <summary>
        /// Real Turbine Work (kJ/kg)
        /// </summary>
        public double RealTurbineWork
        {
            get
            {
                return GetParameter((int)Field.RealTurbineQ).Value;
            }

            set
            {
                GetParameter((int)Field.RealTurbineQ).Value = value;
            }
        }

        /// <summary>
        /// Thermal Efficiency (Unitless)
        /// </summary>
        public double ThermalEfficiency
        {
            get
            {
                return GetParameter((int)Field.thermoEff).Value;
            }

            set
            {
                GetParameter((int)Field.thermoEff).Value = value;
            }
        }

        protected override void Calculation()
        {
            ThermoEntry steamConditions = SteamTable.Table.GetThermoEntryAtTemperatureAndPressure(SteamTemperature, SteamPressure),
                condenserLiquidConditions = SteamTable.Table.GetThermoEntrySatLiquidAtPressure(CondenserPressure),
                condenserVaporConditions = SteamTable.Table.GetThermoEntrySatVaporAtPressure(CondenserPressure);

            CondenserSteamQuality = (steamConditions.S - condenserLiquidConditions.S) 
                / (condenserVaporConditions.S - condenserLiquidConditions.S);

            CondenserEnthalpy = condenserLiquidConditions.H + CondenserSteamQuality * (condenserVaporConditions.H - condenserLiquidConditions.H);

            double CondenserWork = condenserLiquidConditions.H - CondenserEnthalpy;

            double IsentropicPumpWork = (condenserLiquidConditions.V * (SteamPressure - CondenserPressure)) * 1e-6;

            double BoilerEnthalpy = condenserLiquidConditions.H + IsentropicPumpWork;

            double BoilerWork = steamConditions.H - BoilerEnthalpy;

            IdealTurbineWork = -BoilerWork - CondenserWork;

            ThermalEfficiency = Math.Abs(IdealTurbineWork) / BoilerWork;
       
        }
    }
}
