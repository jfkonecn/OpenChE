using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential
{
    /// <summary>
    /// Contains function which support delta functions
    /// </summary>
    public abstract class InletMinusOutput : Function
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaultUnit"></param>
        /// <param name="fun">Calculation function is taken from one of the static functions in this class</param>
        public InletMinusOutput(AbstractUnit[] defaultUnit, CalcualtionFunction fun)
        {
            this.Title = LibraryResources.InletMinusOutlet;
            myCalculationFunction = fun;
            FieldDic = new List<Parameter>
            {
                { new Parameter((int)Field.inlet, LibraryResources.Inlet, defaultUnit, null, true) },
                { new Parameter((int)Field.outlet, LibraryResources.Outlet, defaultUnit, null, true) },
                { new Parameter((int)Field.delta, LibraryResources.InletMinusOutlet, defaultUnit, null, false) }
            }.ToDictionary(x => x.ID);
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
        /// Function to be used to perform calculations
        /// </summary>
        /// <param name="outputID"></param>
        /// <returns></returns>
        public delegate double CalcualtionFunction(int outputID, double inlet, double outlet, double delta);

        private CalcualtionFunction myCalculationFunction
        {
            get; set;
        }

        protected override double Calculation(int outputID)
        {
            return myCalculationFunction(outputID, 
                FieldDic[(int)Field.inlet].GetValue(), 
                FieldDic[(int)Field.outlet].GetValue(), 
                FieldDic[(int)Field.delta].GetValue());
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
