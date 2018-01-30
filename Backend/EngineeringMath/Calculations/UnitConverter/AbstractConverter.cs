using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.UnitConverter
{
    /// <summary>
    /// Create a type of unit converter function
    /// </summary>
    public class AbstractConverter : SimpleFunction
    {
        internal AbstractConverter(AbstractUnit[] units) : base(
                            new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.input, LibraryResources.Inlet, units.ToArray(), true) ,
                    new SimpleParameter((int)Field.output, LibraryResources.Outlet, units.ToArray(), false)
                }
            )
        {

        }

        protected override void Calculation()
        {
            Ouput = Input;
        }

        public enum Field
        {
            /// <summary>
            /// input
            /// </summary>
            input,
            /// <summary>
            /// output
            /// </summary>
            output
        };

        /// <summary>
        /// Input
        /// </summary>
        public double Input
        {
            get
            {
                return GetParameter((int)Field.input).Value;
            }
            set
            {
                GetParameter((int)Field.input).Value = value;
            }
        }

        /// <summary>
        /// Output
        /// </summary>
        public double Ouput
        {
            get
            {
                return GetParameter((int)Field.output).Value;
            }
            set
            {
                GetParameter((int)Field.output).Value = value;
            }
        }
    }
}
