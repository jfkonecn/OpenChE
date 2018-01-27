using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential
{
    /// <summary>
    /// Contains function which support delta functions
    /// </summary>
    public abstract class InletMinusOutput : SolveForFunction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultUnit"></param>
        /// <param name="fun">Calculation function is taken from one of the static functions in this class</param>
        public InletMinusOutput(AbstractUnit[] defaultUnit, CalcualtionFunction fun) : base(
            new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.inlet, LibraryResources.Inlet, defaultUnit, true),
                    new SimpleParameter((int)Field.outlet, LibraryResources.Outlet, defaultUnit, true),
                    new SimpleParameter((int)Field.delta, LibraryResources.InletMinusOutlet, defaultUnit, false)
                })
        {
            this.Title = LibraryResources.InletMinusOutlet;
            myCalculationFunction = fun;
        }

        internal enum Field
        {
            /// <summary>
            /// Inlet in units of T
            /// </summary>
            inlet,
            /// <summary>
            /// Outlet in units of T
            /// </summary>
            outlet,
            /// <summary>
            /// Inlet - Outlet
            /// </summary>
            delta
        };

        /// <summary>
        /// Inlet in units of T
        /// </summary>
        public double Inlet
        {
            get
            {
                return GetParameter((int)Field.inlet).Value;
            }

            set
            {
                GetParameter((int)Field.inlet).Value = value;
            }
        }

        /// <summary>
        /// Outlet in units of T
        /// </summary>
        public double Outlet
        {
            get
            {
                return GetParameter((int)Field.outlet).Value;
            }

            set
            {
                GetParameter((int)Field.outlet).Value = value;
            }
        }

        /// <summary>
        /// Inlet - Outlet
        /// </summary>
        public double Delta
        {
            get
            {
                return GetParameter((int)Field.delta).Value;
            }

            set
            {
                GetParameter((int)Field.delta).Value = value;
            }
        }

        /// <summary>
        /// Function to be used to perform calculations
        /// </summary>
        /// <param name="outputID"></param>
        /// <returns></returns>
        public delegate double CalcualtionFunction(int outputID, double inlet, double outlet, double delta);

        private CalcualtionFunction myCalculationFunction
        {
            get; set;
        }

        protected override SimpleParameter GetDefaultOutput()
        {
            return GetParameter((int)Field.delta);
        }

        protected override void Calculation()
        {
            myCalculationFunction(OutputSelection.SelectedObject.ID, 
                Inlet, 
                Outlet, 
                Delta);
        }

        /// <summary>
        /// Performs a simple inlet - output calculation
        /// </summary>
        /// <param name="outputID"></param>
        /// <param name="inlet"></param>
        /// <param name="outlet"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        internal static double InletMinusOutlet(int outputID, double inlet, double outlet, double delta)
        {
            switch ((Field)outputID)
            {
                case Field.inlet:
                    return outlet + delta;
                case Field.outlet:
                    return inlet - delta;
                case Field.delta:
                    return inlet - outlet;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Performs a inlet^2 - output^2 calculation
        /// </summary>
        /// <param name="outputID"></param>
        /// <param name="inlet"></param>
        /// <param name="outlet"></param>
        /// <param name="delta"></param>
        /// <returns></returns>
        internal static double InletSqMinusOutletSq(int outputID, double inlet, double outlet, double delta)
        {
            switch ((Field)outputID)
            {
                case Field.inlet:
                    return outlet * outlet + delta * delta;
                case Field.outlet:
                    return inlet * inlet - delta * delta;
                case Field.delta:
                    return inlet * inlet - outlet * outlet;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
