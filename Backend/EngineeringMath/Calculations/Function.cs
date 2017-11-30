using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Calculations
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
            int outputID = getOutputID();
            fieldDic[outputID].SetValue(Calculation(outputID));
        }

        /// <summary>
        /// Call this function after creating the function to setup output change event on all of the parameters
        /// </summary>
        internal void SetupOutputChangeEvent()
        {
            foreach (Parameter obj in fieldDic.Values)
            {
                obj.OnMadeOuput += new Parameter.MadeOuputHandler(UpdateAllParametersInputOutput);
            }
        }

        /// <summary>
        /// Makes every parameter an input except for the parameter with the current outputID 
        /// </summary>
        /// <param name="outputID">ID of parameter which is the new output</param>
        private void UpdateAllParametersInputOutput(int outputID)
        {
            foreach (Parameter obj in fieldDic.Values)
            {
                if (obj.ID != outputID)
                {
                    obj.isInput = true;
                }
            }
        }

        /// <summary>
        /// Title of the function
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets the id of the parameter which is the output
        /// </summary>
        /// <returns>ID of the output parameter</returns>
        private int getOutputID()
        {
            foreach (Parameter obj in fieldDic.Values)
            {
                if (obj.isOutput)
                {
                    return obj.ID;
                }
            }

            throw new Exception("No Output Found in function!");
        }


        /// <summary>
        /// Performs the calculation this function object represents using the current state of the parameter objects
        /// </summary>
        /// <param name="outputID">ID of the parameter which is to be solved for</param>
        /// <returns></returns>
        protected abstract double Calculation(int outputID);


    }
}
