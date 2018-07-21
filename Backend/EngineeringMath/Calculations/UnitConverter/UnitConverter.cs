using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.UnitConverter
{
    /// <summary>
    /// Create a type of unit converter function
    /// </summary>
    public abstract class UnitConverter : SimpleFunction
    {
        internal UnitConverter(AbstractUnit[] units) : base()
        {
            Units = units;
            BuildComponentCollection();
        }

        protected override void Calculation()
        {
            Output.Value = Input.Value;
        }

        private readonly AbstractUnit[] Units;

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.input, LibraryResources.Input, Units.ToArray(), true),
                new SimpleParameter((int)Field.output, LibraryResources.Output, Units.ToArray(), false)
            };
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
            get
            {
                return GetParameter((int)Field.input);
            }
        }

        /// <summary>
        /// Output
        /// </summary>
        public SimpleParameter Output
        {
            get
            {
                return GetParameter((int)Field.output);
            }
        }
    }
}
