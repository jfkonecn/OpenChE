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
        /// Field which the function
        /// </summary>
        protected int STANDARD_OUTPUT;

        /// <summary>
        /// Store all paramter
        /// </summary>
        public Dictionary<int, Parameter> fieldDic;

        /// <summary>
        /// Solves function based on what the current output parameter is
        /// </summary>
        public void Solve()
        {
            int outputField = 0;

            foreach (int key in fieldDic.Keys)
            {
                if (fieldDic[key].isOutput)
                {
                    outputField = key;
                    break;
                }
            }

            if (STANDARD_OUTPUT == outputField)
            {
                fieldDic[STANDARD_OUTPUT].SetValue(Calculation());
            }
            else
            {
                fieldDic[outputField].SetValue(
                    Solver.NewtonsMethod(fieldDic[STANDARD_OUTPUT].GetValue(), delegate (double x)
                {
                    fieldDic[outputField].SetValue(x);
                    return Calculation();
                }, minValueDbl: fieldDic[outputField].LowerLimit, maxValueDbl: fieldDic[outputField].UpperLimit)
                );
            }

        }

        /// <summary>
        /// Performs the calculation this function object represents using the current state of the parameter objects
        /// </summary>
        /// <returns></returns>
        protected abstract double Calculation();


    }
}
