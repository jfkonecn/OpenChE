using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface IParameterContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Parameter GetParameter(string name);
    }
}
