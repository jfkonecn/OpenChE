using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Selectors;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// A function where any of the outputs can be solved for
    /// </summary>
    public abstract class SolveForFunction : SimpleFunction
    {
        internal SolveForFunction(SimpleParameter[] allParameters) : base(allParameters)
        {

            foreach (SimpleParameter obj in ParameterCollection())
            {
                obj.OnMadeOuput += UpdateAllParametersInputOutput;
            }

            OutputSelection = new PickerSelection<SimpleParameter>(
                ParameterCollection().ToDictionary(x => x.Title, x => x)
                );
            OutputSelection.OnSelectedIndexChanged += OutputSelection_OnSelectedIndexChanged;
            OutputSelection.Title = LibraryResources.SolveFor;
            OutputSelection.SelectedObject = GetDefaultOutput();
        }

        /// <summary>
        /// Gets default output parameter
        /// </summary>
        protected abstract SimpleParameter GetDefaultOutput();

        private void OutputSelection_OnSelectedIndexChanged()
        {
            OutputSelection.SelectedObject.isOutput = true;
            foreach (SimpleParameter obj in ParameterCollection())
            {
                obj.OnReset();
            }
        }

        /// <summary>
        /// Stores the output parameter the user selected (intended to be binded with a picker)
        /// </summary>
        public PickerSelection<SimpleParameter> OutputSelection;

        /// <summary>
        /// Makes every parameter an input except for the parameter with the current outputID 
        /// </summary>
        /// <param name="outputID">ID of parameter which is the new output</param>
        private void UpdateAllParametersInputOutput(int outputID)
        {
            foreach (SimpleParameter obj in ParameterCollection())
            {
                if (obj.ID != outputID)
                {
                    obj.isInput = true;
                }
            }
        }

        public override Type CastAs()
        {
            return typeof(SolveForFunction);
        }

        /// <summary>
        /// Iterate through each abstract components in this collection in order
        /// </summary>
        /// <returns></returns>
        public override IEnumerator GetEnumerator()
        {
            yield return OutputSelection;
            foreach (AbstractComponent obj in ParameterCollection())
            {
                yield return obj;
            }
        }
    }
}
