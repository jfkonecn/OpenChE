using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Selectors;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// An object which allows a user to choose different functions to use via a picker
    /// </summary>
    public abstract class FunctionSubber : AbstractComponent
    {
        internal FunctionSubber(Dictionary<string, FunctionFactory.SolveForFactoryData> funData)
        {
            AllFunctions = new FunctionPickerSelection(funData);
            AllFunctions.PropertyChanged += _Field_PropertyChanged;
            AllFunctions.SelectedIndex = 0;
        }

        public FunctionPickerSelection AllFunctions;

        public override Type CastAs()
        {
            return typeof(FunctionSubber);
        }
    }
}
