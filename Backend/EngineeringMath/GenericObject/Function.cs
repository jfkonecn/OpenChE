using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations;
using EngineeringMath.GenericObject;

namespace EngineeringMath.GenericObject
{
    public abstract class Function
    {

        /// <summary>
        /// Store all paramter
        /// <para>int represents the id of the parameters</para>
        /// </summary>
        public Dictionary<int, Parameter> fieldDic;

        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve()
        {
            int outputID = 0;

            foreach (Parameter obj in fieldDic.Values)
            {
                if (obj.isOutput)
                {
                    outputID = obj.ID;
                    break;
                }
            }

            fieldDic[outputID].SetValue(Calculation(outputID));

        }

        /// <summary>
        /// Performs the calculation this function object represents using the current state of the parameter objects
        /// </summary>
        /// <param name="outputID">ID of the parameter which is to be solved for</param>
        /// <returns></returns>
        protected abstract double Calculation(int outputID);


    }
}
