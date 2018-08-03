using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class ParameterContainer : NotifyPropertyChangedExtension
    {
        /// <summary>
        /// Gets the value of paraName used to perform calculations
        /// </summary>
        /// <param name="paraName">Name of the parameter</param>
        /// <returns></returns>
        public abstract double GetBaseUnitValue(string paraName);


        public abstract string EquationExpression { get; }
    }
}
