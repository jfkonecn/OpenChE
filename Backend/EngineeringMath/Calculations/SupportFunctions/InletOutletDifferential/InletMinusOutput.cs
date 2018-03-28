using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;

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
        public InletMinusOutput(AbstractUnit[] defaultUnit, CalcualtionFunction fun)
        {
            Inlet = new SimpleParameter((int)Field.inlet, LibraryResources.Inlet, defaultUnit, true);
            Outlet = new SimpleParameter((int)Field.outlet, LibraryResources.Outlet, defaultUnit, true);
            Delta = new SimpleParameter((int)Field.delta, LibraryResources.InletMinusOutlet, defaultUnit, false);
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
        public readonly SimpleParameter Inlet;

        /// <summary>
        /// Outlet in units of T
        /// </summary>
        public readonly SimpleParameter Outlet;

        /// <summary>
        /// Inlet - Outlet
        /// </summary>
        public readonly SimpleParameter Delta;

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
                Inlet.Value, 
                Outlet.Value, 
                Delta.Value);
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

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.inlet:
                    return Inlet;
                case Field.outlet:
                    return Outlet;
                case Field.delta:
                    return Delta;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return Inlet;
            yield return Outlet;
            yield return Delta;
        }
    }
}
