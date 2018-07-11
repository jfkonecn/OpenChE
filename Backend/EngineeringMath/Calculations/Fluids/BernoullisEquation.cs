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
    public class BernoullisEquation : SolveForFunction
    {
        public BernoullisEquation()
        {
            this.Title = LibraryResources.BernoullisEquation;


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
        public SimpleParameter InletVelocity
        {
            get
            {
                return GetParameter((int)Field.inletVelo);
            }
        }

        /// <summary>
        /// Outlet Velocity (m/s)
        /// </summary>
        public SimpleParameter OutletVelocity
        {
            get
            {
                return GetParameter((int)Field.outletVelo);
            }
        }

        /// <summary>
        /// Inlet Height (m)
        /// </summary>
        public SimpleParameter InletHeight
        {
            get
            {
                return GetParameter((int)Field.inletHeight);
            }
        }

        /// <summary>
        /// Outlet Height (m)
        /// </summary>
        public SimpleParameter OutletHeight
        {
            get
            {
                return GetParameter((int)Field.outletHeight);
            }
        }

        /// <summary>
        /// Inlet Pressure (Pa)
        /// </summary>
        public SimpleParameter InletPressure
        {
            get
            {
                return GetParameter((int)Field.inletP);
            }
        }

        /// <summary>
        /// Outlet Pressure (Pa)
        /// </summary>
        public SimpleParameter OutletPressure
        {
            get
            {
                return GetParameter((int)Field.outletP);
            }
        }

        /// <summary>
        /// Density of fluid (kg/m3)
        /// </summary>
        public SimpleParameter Density
        {
            get
            {
                return GetParameter((int)Field.density);
            }
        }


        protected override SimpleParameter GetDefaultOutput()
        {
            return Density;
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

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.inletVelo, LibraryResources.InletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0),
                new SimpleParameter((int)Field.outletVelo, LibraryResources.OutletVelo, new AbstractUnit[] { Length.m, Time.sec }, true, 0),
                new SimpleParameter((int)Field.inletHeight, LibraryResources.InletHeight, new AbstractUnit[] { Length.m }, true, 0),
                new SimpleParameter((int)Field.outletHeight, LibraryResources.OutletHeight, new AbstractUnit[] { Length.m }, true, 0),
                new SimpleParameter((int)Field.inletP, LibraryResources.InletP, new AbstractUnit[] { Pressure.Pa }, true, 0),
                new SimpleParameter((int)Field.outletP, LibraryResources.OutletP, new AbstractUnit[] { Pressure.Pa }, true, 0),
                new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, false, 0),
            };
        }
    }
}
