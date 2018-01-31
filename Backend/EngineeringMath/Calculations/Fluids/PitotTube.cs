using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.Fluids
{
    public class PitotTube : SolveForFunction
    {

        public PitotTube() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.correctionCo, LibraryResources.CorrectionCo, new AbstractUnit[] { Unitless.unitless }, true, 0, 1),
                    new SimpleParameter((int)Field.deltaH, LibraryResources.ManoDeltaH, new AbstractUnit[] { Length.m }, true, 0),
                    new SimpleParameter((int)Field.manoDensity, LibraryResources.ManoDensity, new AbstractUnit[] { Density.kgm3 }, true, 0) ,
                    new SimpleParameter((int)Field.fluidDensity, LibraryResources.FluidDensity, new AbstractUnit[] { Density.kgm3 }, true, 0) ,
                    new SimpleParameter((int)Field.velo, LibraryResources.FluidVelocity, new AbstractUnit[] { Length.m, Time.sec }, false, 0) 
                }
            )
        {

            this.Title = LibraryResources.PitotTube;
#if DEBUG
            CorrectionCoefficient = 1;
            DeltaH = 1;
            ManometerDensity = 10;
            FluidDensity = 8;
            Velocity = 10;
#endif
        }
        /// <summary>
        /// The acceration of gravity in m/s^2
        /// </summary>
        private readonly double GRAVITY = 9.81;

        public enum Field
        {
            /// <summary>
            /// Velocity (m/s)
            /// </summary>
            velo,
            /// <summary>
            /// Correction Coefficient (unitless)
            /// </summary>
            correctionCo,
            /// <summary>
            /// Change in height (m)
            /// </summary>
            deltaH,
            /// <summary>
            /// Density of manometer fluid (kg/m3)
            /// </summary>
            manoDensity,
            /// <summary>
            /// Density of fluid (kg/m3)
            /// </summary>
            fluidDensity
        };

        /// <summary>
        /// Velocity (m/s)
        /// </summary>
        public double Velocity
        {
            get
            {
                return GetParameter((int)Field.velo).Value;
            }

            set
            {
                GetParameter((int)Field.velo).Value = value;
            }
        }

        /// <summary>
        /// Correction Coefficient (unitless)
        /// </summary>
        public double CorrectionCoefficient
        {
            get
            {
                return GetParameter((int)Field.correctionCo).Value;
            }

            set
            {
                GetParameter((int)Field.correctionCo).Value = value;
            }
        }

        /// <summary>
        /// Change in height (m)
        /// </summary>
        public double DeltaH
        {
            get
            {
                return GetParameter((int)Field.deltaH).Value;
            }

            set
            {
                GetParameter((int)Field.deltaH).Value = value;
            }
        }


        /// <summary>
        /// Density of manometer fluid (kg/m3)
        /// </summary>
        public double ManometerDensity
        {
            get
            {
                return GetParameter((int)Field.manoDensity).Value;
            }

            set
            {
                GetParameter((int)Field.manoDensity).Value = value;
            }
        }
        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public double FluidDensity
        {
            get
            {
                return GetParameter((int)Field.fluidDensity).Value;
            }

            set
            {
                GetParameter((int)Field.fluidDensity).Value = value;
            }
        }

        protected override SimpleParameter GetDefaultOutput()
        {
            return GetParameter((int)Field.velo);
        }

        /// <summary>
        /// Perform Pitot Tube Calculation
        /// </summary>
        protected override void Calculation()
        {
            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.correctionCo:
                    CorrectionCoefficient = Velocity / Math.Sqrt((2 * GRAVITY * DeltaH * (ManometerDensity - FluidDensity)) / FluidDensity);
                    break;
                case Field.deltaH:
                    DeltaH = Math.Pow(Velocity / CorrectionCoefficient, 2) * ((FluidDensity) / (2 * GRAVITY * (ManometerDensity - FluidDensity)));
                    break;
                case Field.fluidDensity:
                    FluidDensity = ManometerDensity / (Math.Pow(Velocity / CorrectionCoefficient, 2) * (1 / (2 * GRAVITY * DeltaH)) + 1);
                    break;
                case Field.manoDensity:
                    ManometerDensity = FluidDensity * (Math.Pow(Velocity / CorrectionCoefficient, 2) * (1 / (2 * GRAVITY * DeltaH)) + 1);
                    break;
                case Field.velo:
                    Velocity = CorrectionCoefficient * Math.Sqrt((2 * GRAVITY * DeltaH * (ManometerDensity - FluidDensity)) / FluidDensity);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
