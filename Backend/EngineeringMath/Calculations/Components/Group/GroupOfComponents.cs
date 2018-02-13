using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations.Components.Group
{
    /// <summary>
    /// Makes user pick from a group of functions to use
    /// </summary>
    public class GroupOfComponents : AbstractComponent, IEnumerable
    {

        internal GroupOfComponents(List<AbstractComponent> components)
        {
            AllComponents = components.ToArray();
        }


        protected AbstractComponent[] AllComponents;



        public override Type CastAs()
        {
            return typeof(GroupOfComponents);
        }

        /// <summary>
        /// Iterate through each abstract components in this collection in order
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            foreach(AbstractComponent comp in AllComponents)
            {
                yield return AllComponents;
            }
        }
    }
}
