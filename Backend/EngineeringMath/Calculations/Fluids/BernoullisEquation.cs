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
    public class BernoullisEquation : SolveForFunction
    {
        public BernoullisEquation()
        {
            this.Title = LibraryResources.BernoullisEquation;
            InletVelocity = new SimpleParameter((int)Field.inletVelo, LibraryResources.InletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0);
            OutletVelocity = new SimpleParameter((int)Field.outletVelo, LibraryResources.OutletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0);
            InletHeight = new SimpleParameter((int)Field.inletHeight, LibraryResources.InletHeight, new AbstractUnit[] { Length.m }, true, 0);
            OutletHeight = new SimpleParameter((int)Field.outletHeight, LibraryResources.OutletHeight, new AbstractUnit[] { Length.m }, true, 0);
            InletPressure= new SimpleParameter((int)Field.inletP, LibraryResources.InletP, new AbstractUnit[] { Pressure.Pa }, true, 0);
            OutletPressure = new SimpleParameter((int)Field.outletP, LibraryResources.OutletP, new AbstractUnit[] { Pressure.Pa }, true, 0);
            Density = new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, false, 0);

#if DEBUG
            InletVelocity.Value = 1;
            OutletVelocity.Value = 2;
            InletHeight.Value = 10;
            OutletHeight.Value = 10;
            OutletVelocity.Value = 5;
            InletPressure.Value = 500;
            OutletPressure.Value = 100;
            Density.Value = 1000;
#endif
        }

        /// <summary>
        /// The acceration of gravity in m/s^2
        /// </summary>
        private readonly double GRAVITY = 9.81;

        public enum Field
        {
            /// <summary>
            /// Inlet Velocity (m/s)
            /// </summary>
            inletVelo,
            /// <summary>
            /// Outlet Velocity (m/s)
            /// </summary>
            outletVelo,
            /// <summary>
            /// Inlet Height (m)
            /// </summary>
            inletHeight,
            /// <summary>
            /// Outlet Height (m)
            /// </summary>
            outletHeight,
            /// <summary>
            /// Inlet Pressure (Pa)
            /// </summary>
            inletP,
            /// <summary>
            /// Outlet Pressure (Pa)
            /// </summary>
            outletP,
            /// <summary>
            /// Density of fluid (kg/m3)
            /// </summary>
            density
        };

        /// <summary>
        /// Inlet Velocity (m/s)
        /// </summary>
        public readonly SimpleParameter InletVelocity;

        /// <summary>
        /// Outlet Velocity (m/s)
        /// </summary>
        public readonly SimpleParameter OutletVelocity;

        /// <summary>
        /// Inlet Height (m)
        /// </summary>
        public readonly SimpleParameter InletHeight;

        /// <summary>
        /// Outlet Height (m)
        /// </summary>
        public readonly SimpleParameter OutletHeight;

        /// <summary>
        /// Inlet Pressure (Pa)
        /// </summary>
        public readonly SimpleParameter InletPressure;

        /// <summary>
        /// Outlet Pressure (Pa)
        /// </summary>
        public readonly SimpleParameter OutletPressure;

        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public readonly SimpleParameter Density;


        protected override SimpleParameter GetDefaultOutput()
        {
            return GetParameter((int)Field.density);
        }

        /// <summary>
        /// Perform Bernoulli's Equation Calculation
        /// </summary>
        protected override void Calculation()
        {
            double inletVeloSqu = Math.Pow(InletVelocity.Value, 2),
                outletVeloSqu = Math.Pow(OutletVelocity.Value, 2);

            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.inletVelo:
                    InletVelocity.Value = Math.Sqrt(
                        2 * ((outletVeloSqu / 2 + GRAVITY * OutletHeight.Value + OutletPressure.Value / Density.Value) -
                        (GRAVITY * InletHeight.Value + InletPressure.Value / Density.Value))
                        );
                    break;
                case Field.outletVelo:
                    OutletVelocity.Value = Math.Sqrt(
                        2 * ((inletVeloSqu / 2 + GRAVITY * InletHeight.Value + InletPressure.Value / Density.Value) -
                        (GRAVITY * OutletHeight.Value + OutletPressure.Value / Density.Value))
                        );
                    break;
                case Field.inletHeight:
                    InletHeight.Value = (1 / GRAVITY) * ((outletVeloSqu / 2 + GRAVITY * OutletHeight.Value + OutletPressure.Value / Density.Value) -
                        (inletVeloSqu / 2 + InletPressure.Value / Density.Value));
                    break;
                case Field.outletHeight:
                    OutletHeight.Value = (1 / GRAVITY) * ((inletVeloSqu / 2 + GRAVITY * InletHeight.Value + InletPressure.Value / Density.Value) -
                        (outletVeloSqu / 2 + OutletPressure.Value / Density.Value));
                    break;
                case Field.inletP:
                    InletPressure.Value = Density.Value * ((outletVeloSqu / 2 + GRAVITY * OutletHeight.Value + OutletPressure.Value / Density.Value) -
                        (inletVeloSqu / 2 + GRAVITY * InletHeight.Value));
                    break;
                case Field.outletP:
                    OutletPressure.Value = Density.Value * ((inletVeloSqu / 2 + GRAVITY * InletHeight.Value + InletPressure.Value / Density.Value) -
                        (outletVeloSqu / 2 + GRAVITY * OutletHeight.Value));
                    break;
                case Field.density:
                    Density.Value = (OutletPressure.Value - InletPressure.Value) /
                        ((inletVeloSqu / 2 + GRAVITY * InletHeight.Value) -
                        (outletVeloSqu / 2 + GRAVITY * OutletHeight.Value));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.inletVelo:
                    return InletVelocity;
                case Field.outletVelo:
                    return OutletVelocity;
                case Field.inletHeight:
                    return InletHeight;
                case Field.outletHeight:
                    return OutletHeight;
                case Field.inletP:
                    return InletPressure;
                case Field.outletP:
                    return OutletPressure;
                case Field.density:
                    return Density;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return InletVelocity;
            yield return OutletVelocity;
            yield return InletHeight;
            yield return OutletHeight;
            yield return InletPressure;
            yield return OutletPressure;
            yield return Density;
        }
    }
}
