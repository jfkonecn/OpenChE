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
    public class BernoullisEquation : SolveForFunction
    {
        public BernoullisEquation() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.inletVelo, LibraryResources.InletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0) ,
                    new SimpleParameter((int)Field.outletVelo, LibraryResources.OutletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0),
                    new SimpleParameter((int)Field.inletHeight, LibraryResources.InletHeight, new AbstractUnit[] { Length.m }, true, 0),
                    new SimpleParameter((int)Field.outletHeight, LibraryResources.OutletHeight, new AbstractUnit[] { Length.m }, true, 0),
                    new SimpleParameter((int)Field.inletP, LibraryResources.InletP, new AbstractUnit[] { Pressure.Pa }, true, 0),
                    new SimpleParameter((int)Field.outletP, LibraryResources.OutletP, new AbstractUnit[] { Pressure.Pa }, true, 0),
                    new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, false, 0)
                }
            )
        {
            this.Title = LibraryResources.BernoullisEquation;

#if DEBUG
            InletVelocity = 1;
            OutletVelocity = 2;
            InletHeight = 10;
            OutletVelocity = 5;
            InletPressure = 500;
            OutletPressure = 100;
            Density = 1000;
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
        public double InletVelocity
        {
            get
            {
                return GetParameter((int)Field.inletVelo).Value;
            }
            set
            {
                GetParameter((int)Field.inletVelo).Value = value;
            }
        }

        /// <summary>
        /// Outlet Velocity (m/s)
        /// </summary>
        public double OutletVelocity
        {
            get
            {
                return GetParameter((int)Field.outletVelo).Value;
            }
            set
            {
                GetParameter((int)Field.outletVelo).Value = value;
            }
        }

        /// <summary>
        /// Inlet Height (m)
        /// </summary>
        public double InletHeight
        {
            get
            {
                return GetParameter((int)Field.inletHeight).Value;
            }
            set
            {
                GetParameter((int)Field.inletHeight).Value = value;
            }
        }

        /// <summary>
        /// Outlet Height (m)
        /// </summary>
        public double OutletHeight
        {
            get
            {
                return GetParameter((int)Field.outletHeight).Value;
            }
            set
            {
                GetParameter((int)Field.outletHeight).Value = value;
            }
        }

        /// <summary>
        /// Inlet Pressure (Pa)
        /// </summary>
        public double InletPressure
        {
            get
            {
                return GetParameter((int)Field.inletP).Value;
            }
            set
            {
                GetParameter((int)Field.inletP).Value = value;
            }
        }

        /// <summary>
        /// Outlet Pressure (Pa)
        /// </summary>
        public double OutletPressure
        {
            get
            {
                return GetParameter((int)Field.outletP).Value;
            }
            set
            {
                GetParameter((int)Field.outletP).Value = value;
            }
        }

        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public double Density
        {
            get
            {
                return GetParameter((int)Field.density).Value;
            }
            set
            {
                GetParameter((int)Field.density).Value = value;
            }
        }


        protected override SimpleParameter GetDefaultOutput()
        {
            return GetParameter((int)Field.density);
        }

        /// <summary>
        /// Perform Bernoulli's Equation Calculation
        /// </summary>
        protected override void Calculation()
        {
            double inletVeloSqu = Math.Pow(InletVelocity, 2),
                outletVeloSqu = Math.Pow(OutletVelocity, 2);

            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.inletVelo:
                    InletVelocity = Math.Sqrt(
                        2 * ((outletVeloSqu / 2 + GRAVITY * OutletHeight + OutletPressure / Density) -
                        (GRAVITY * InletHeight + InletPressure / Density))
                        );
                    break;
                case Field.outletVelo:
                    OutletVelocity = Math.Sqrt(
                        2 * ((inletVeloSqu / 2 + GRAVITY * InletHeight + InletPressure / Density) -
                        (GRAVITY * OutletHeight + OutletPressure / Density))
                        );
                    break;
                case Field.inletHeight:
                    InletHeight = (1 / GRAVITY) * ((outletVeloSqu / 2 + GRAVITY * OutletHeight + OutletPressure / Density) -
                        (inletVeloSqu / 2 + InletPressure / Density));
                    break;
                case Field.outletHeight:
                    OutletHeight = (1 / GRAVITY) * ((inletVeloSqu / 2 + GRAVITY * InletHeight + InletPressure / Density) -
                        (outletVeloSqu / 2 + OutletPressure / Density));
                    break;
                case Field.inletP:
                    InletPressure = Density * ((outletVeloSqu / 2 + GRAVITY * OutletHeight + OutletPressure / Density) -
                        (inletVeloSqu / 2 + GRAVITY * InletHeight));
                    break;
                case Field.outletP:
                    OutletPressure = Density * ((inletVeloSqu / 2 + GRAVITY * InletHeight + InletPressure / Density) -
                        (outletVeloSqu / 2 + GRAVITY * OutletHeight));
                    break;
                case Field.density:
                    Density = (OutletPressure - InletPressure) /
                        ((inletVeloSqu / 2 + GRAVITY * InletHeight) -
                        (outletVeloSqu / 2 + GRAVITY * OutletHeight));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
