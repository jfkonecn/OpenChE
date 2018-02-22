using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Calculations.Components.Selectors
{
    /// <summary>
    /// Allows user to picker from a list of consecutive integers
    /// </summary>
    public class IntegerSpinner : SimplePicker<int>
    {
        internal IntegerSpinner(int low, int high, string title = null) : base( Enumerable.Range(low, high).ToDictionary(x => x.ToString(), x => x), title)
        {
            this.SelectedIndex = 0;
        }

        public override Type CastAs()
        {
            return typeof(SimplePicker<int>);
        }
    }
}
