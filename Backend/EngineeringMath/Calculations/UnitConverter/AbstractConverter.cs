using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.UnitConverter
{
    /// <summary>
    /// Create a type of unit converter function
    /// </summary>
    public class AbstractConverter : SimpleFunction
    {
        internal AbstractConverter(AbstractUnit[] units)
        {
            Input = new SimpleParameter((int)Field.input, LibraryResources.Input, units.ToArray(), true);
            Output = new SimpleParameter((int)Field.output, LibraryResources.Output, units.ToArray(), false);
        }

        protected override void Calculation()
        {
            Output.Value = Input.Value;
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.input:
                    return Input;
                case Field.output:
                    return Output;
                default:
                    throw new NotImplementedException();

            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return Input;
            yield return Output;
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
        public SimpleParameter Input
        {
            get; private set;
        }

        /// <summary>
        /// Output
        /// </summary>
        public SimpleParameter Output
        {
            get; private set;
        }
    }
}
