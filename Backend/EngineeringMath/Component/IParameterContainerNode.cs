using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameterContainerNode
    {
        /// <summary>
        /// Finds the parameter with the name, paraVarName
        /// </summary>
        /// <param name="paraVarName">Name of the parameter</param>
        /// <returns></returns>
        Parameter FindParameter(string paraVarName);


        /// <summary>
        /// Performs the actual calculation for this function object
        /// </summary>
        void Calculate();

        /// <summary>
        /// Forces parameters and settings in the entire tree to be updated
        /// </summary>
        void Reset();

        void SettingAdded(ISetting setting);
        void SettingRemoved(ISetting setting);
        void SettingRemoved(IList<ISetting> settings);

        void ParameterAdded(Parameter parameter);
        void ParameterRemoved(Parameter parameter);
        void ParameterRemoved(IList<Parameter> parameters);
    }
}
