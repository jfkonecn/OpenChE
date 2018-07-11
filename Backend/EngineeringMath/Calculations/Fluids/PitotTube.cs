using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;
using System.Collections.ObjectModel;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.Fluids
{
    public class PitotTube : SolveForFunction
    {

        public PitotTube() : base()
        {

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
        public SimpleParameter Velocity
        {
            get
            {
                return GetParameter((int)Field.velo);
            }
        }

        /// <summary>
        /// Correction Coefficient (unitless)
        /// </summary>
        public SimpleParameter CorrectionCoefficient
        {
            get
            {
                return GetParameter((int)Field.correctionCo);
            }
        }

        /// <summary>
        /// Change in height (m)
        /// </summary>
        public SimpleParameter DeltaH
        {
            get
            {
                return GetParameter((int)Field.deltaH);
            }
        }

        /// <summary>
        /// Density of manometer fluid (kg/m3)
        /// </summary>
        public SimpleParameter ManometerDensity
        {
            get
            {
                return GetParameter((int)Field.manoDensity);
            }
        }

        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public SimpleParameter FluidDensity
        {
            get
            {
                return GetParameter((int)Field.fluidDensity);
            }
        }

        protected override SimpleParameter GetDefaultOutput()
        {
            return Velocity;
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

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.correctionCo, LibraryResources.CorrectionCo, new AbstractUnit[] { Unitless.unitless }, true, 0, 1),
                new SimpleParameter((int)Field.deltaH, LibraryResources.ManoDeltaH, new AbstractUnit[] { Length.m }, true, 0),
                new SimpleParameter((int)Field.manoDensity, LibraryResources.ManoDensity, new AbstractUnit[] { Density.kgm3 }, true, 0),
                new SimpleParameter((int)Field.fluidDensity, LibraryResources.FluidDensity, new AbstractUnit[] { Density.kgm3 }, true, 0),
                new SimpleParameter((int)Field.velo, LibraryResources.FluidVelocity, new AbstractUnit[] { Length.m, Time.sec }, false, 0)
        };
        }
    }
}
