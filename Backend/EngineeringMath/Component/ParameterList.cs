using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class ParameterList : List<Parameter>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Parameter GetParameter(string name)
        {
            return Find(x => x.Name.Equals(name));
        }
    }
}
