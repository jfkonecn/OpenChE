using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EngineeringMath.Calculations.ComponentFactory;

namespace EngineeringMath.Calculations.Components.Selectors
{
    /// <summary>
    /// A picker which contains parameters links so that more than one parameter can be linked to a substitute function
    /// </summary>
    public class FunctionPickerParaLinks : FunctionPicker
    {
        internal FunctionPickerParaLinks(Dictionary<string, ParameterLinksFactoryData> funData) : 
            base(funData.ToDictionary(x => x.Key, x => x.Value.FunType))
        {
            TypeToFactoryData = funData.ToDictionary(x => x.Value.FunType, x => x.Value);
        }

        private Dictionary<Type, ParameterLinksFactoryData> TypeToFactoryData;


        /// <summary>
        /// Gets the subID given the homeID of parameter link
        /// </summary>
        /// <param name="homeID"></param>
        /// <returns></returns>
        public int GetSubID(int homeID)
        {
            return TypeToFactoryData[SelectedObject].GetSubID(homeID);
        }

        /// <summary>
        /// Gets the homeID given the homeID of parameter link
        /// </summary>
        /// <param name="homeID"></param>
        /// <returns></returns>
        public int GetHomeID(int subID)
        {
            return TypeToFactoryData[SelectedObject].GetHomeID(subID);
        }

    }
}
