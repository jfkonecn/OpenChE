using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameterContainerNode
    {
        /// <summary>
        /// Gets the value of paraName used to perform calculations
        /// </summary>
        /// <param name="paraName">Name of the parameter</param>
        /// <returns></returns>
        double GetBaseUnitValue(string paraName);


        /// <summary>
        /// Sets the value of paraName used to perform calculations
        /// </summary>
        /// <param name="paraName">Name of the parameter</param>
        /// <returns></returns>
        void SetBaseUnitValue(string paraName, double num);

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
