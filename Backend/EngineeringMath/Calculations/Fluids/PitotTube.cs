using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;

namespace EngineeringMath.Calculations.Fluids
{
    public class PitotTube : SolveForFunction
    {

        public PitotTube()
        {
            CorrectionCoefficient = new SimpleParameter((int)Field.correctionCo, LibraryResources.CorrectionCo, new AbstractUnit[] { Unitless.unitless }, true, 0, 1);
            DeltaH = new SimpleParameter((int)Field.deltaH, LibraryResources.ManoDeltaH, new AbstractUnit[] { Length.m }, true, 0);
            ManometerDensity = new SimpleParameter((int)Field.manoDensity, LibraryResources.ManoDensity, new AbstractUnit[] { Density.kgm3 }, true, 0);
            FluidDensity = new SimpleParameter((int)Field.fluidDensity, LibraryResources.FluidDensity, new AbstractUnit[] { Density.kgm3 }, true, 0);
            Velocity = new SimpleParameter((int)Field.velo, LibraryResources.FluidVelocity, new AbstractUnit[] { Length.m, Time.sec }, false, 0);
            this.Title = LibraryResources.PitotTube;
#if DEBUG
            CorrectionCoefficient.Value = 1;
            DeltaH.Value = 1;
            ManometerDensity.Value = 10;
            FluidDensity.Value = 8;
            Velocity.Value = 10;
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
        public readonly SimpleParameter Velocity;

        /// <summary>
        /// Correction Coefficient (unitless)
        /// </summary>
        public readonly SimpleParameter CorrectionCoefficient;

        /// <summary>
        /// Change in height (m)
        /// </summary>
        public readonly SimpleParameter DeltaH;


        /// <summary>
        /// Density of manometer fluid (kg/m3)
        /// </summary>
        public readonly SimpleParameter ManometerDensity;

        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public readonly SimpleParameter FluidDensity;

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
                    CorrectionCoefficient.Value = Velocity.Value / Math.Sqrt((2 * GRAVITY * DeltaH.Value * (ManometerDensity.Value - FluidDensity.Value)) / FluidDensity.Value);
                    break;
                case Field.deltaH:
                    DeltaH.Value = Math.Pow(Velocity.Value / CorrectionCoefficient.Value, 2) * ((FluidDensity.Value) / (2 * GRAVITY * (ManometerDensity.Value - FluidDensity.Value)));
                    break;
                case Field.fluidDensity:
                    FluidDensity.Value = ManometerDensity.Value / (Math.Pow(Velocity.Value / CorrectionCoefficient.Value, 2) * (1 / (2 * GRAVITY * DeltaH.Value)) + 1);
                    break;
                case Field.manoDensity:
                    ManometerDensity.Value = FluidDensity.Value * (Math.Pow(Velocity.Value / CorrectionCoefficient.Value, 2) * (1 / (2 * GRAVITY * DeltaH.Value)) + 1);
                    break;
                case Field.velo:
                    Velocity.Value = CorrectionCoefficient.Value * Math.Sqrt((2 * GRAVITY * DeltaH.Value * (ManometerDensity.Value - FluidDensity.Value)) / FluidDensity.Value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.correctionCo:
                    return CorrectionCoefficient;
                case Field.deltaH:
                    return DeltaH;
                case Field.fluidDensity:
                    return FluidDensity;
                case Field.manoDensity:
                    return ManometerDensity;
                case Field.velo:
                    return Velocity;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return CorrectionCoefficient;
            yield return DeltaH;
            yield return FluidDensity;
            yield return ManometerDensity;
            yield return Velocity;
        }
    }
}
