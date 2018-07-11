using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Calculations.Components.Parameter;
using System.Collections.ObjectModel;

namespace EngineeringMath.Calculations.Components.Functions
{
    /// <summary>
    /// A function where any of the outputs can be solved for
    /// </summary>
    public abstract class SolveForFunction : SimpleFunction
    {
        internal SolveForFunction() : base()
        {


        }

        /// <summary>
        /// Gets default output parameter
        /// </summary>
        protected abstract SimpleParameter GetDefaultOutput();

        private void OutputSelection_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            OutputSelection.SelectedObject.IsOutput = true;
            OnReset();
        }

        /// <summary>
        /// Stores the output parameter the user selected (intended to be binded with a picker)
        /// </summary>
        public SimplePicker<SimpleParameter> OutputSelection;

        /// <summary>
        /// Makes every parameter an input except for the parameter with the current outputID 
        /// </summary>
        /// <param name="outputID">ID of parameter which is the new output</param>
        private void UpdateAllParametersInputOutput(int outputID)
        {
            foreach (SimpleParameter obj in AllParameters)
            {
                if (obj.ID != outputID)
                {
                    obj.IsInput = true;
                }
            }
        }




        protected override ObservableCollection<AbstractComponent> CreateDefaultComponentCollection()
        {
            // ComponentCollection must be set in order to use AllParameters
            ComponentCollection = base.CreateDefaultComponentCollection();

            OutputSelection = new SimplePicker<SimpleParameter>(
                AllParameters.ToDictionary(x => (x as SimpleParameter).Title, x => x));
            OutputSelection.OnSelectedIndexChanged += OutputSelection_OnSelectedIndexChanged;
            OutputSelection.Title = LibraryResources.SolveFor;
            OutputSelection.SelectedObject = GetDefaultOutput();

            ObservableCollection<AbstractComponent> temp = new ObservableCollection<AbstractComponent>
            {
                OutputSelection
            };

            foreach (AbstractComponent obj in ComponentCollection)
            {
                temp.Add(obj);
                if(obj as SimpleParameter != null)
                {
                    ((SimpleParameter)obj).OnMadeOuput += UpdateAllParametersInputOutput;
                }                
            }

            return temp;
        }
    }
}
