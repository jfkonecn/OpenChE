using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Thermo.Cycles
{
    public class RegenerativeCycle : RankineCycle
    {
        public RegenerativeCycle() : base()
        {
            this.Title = LibraryResources.RegenerativeCycle;
        }


        /// <summary>
        /// The spinner which has the total number of Regeneration Stages
        /// </summary>
        public IntegerSpinner RegenerationStagesSelector = new IntegerSpinner(1, 10, LibraryResources.RegenerationStages);

        /// <summary>
        /// The total number of Regeneration Stages
        /// </summary>
        public int TotalRegenerationStages
        {
            get { return RegenerationStagesSelector.SelectedObject; }
            set { RegenerationStagesSelector.SelectedObject = value; }
        }


        public override IEnumerator GetEnumerator()
        {
            yield return RegenerationStagesSelector;
            foreach (AbstractComponent obj in ParameterCollection())
            {
                yield return obj;
            }
        }
    }
}
