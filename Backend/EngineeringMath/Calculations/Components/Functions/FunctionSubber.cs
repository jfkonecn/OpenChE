using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Group;
using EngineeringMath.Calculations.Components.Selectors;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// An object which allows a user to choose different functions to use via a picker
    /// </summary>
    public abstract class FunctionSubber : AbstractComponent
    {
        internal FunctionSubber(Dictionary<string, Type> funData)
        {
            

            AllFunctions = new FunctionPicker(funData)
            {
                SelectedIndex = 0
            };
            AllFunctions.OnFunctionCreatedEvent += AllFunctions_OnFunctionCreatedEvent;
        }

        private void AllFunctions_OnFunctionCreatedEvent(object sender, EventArgs e)
        {
            // leave in case we need it
        }

        public FunctionPicker AllFunctions { get; private set; }

    }
}
