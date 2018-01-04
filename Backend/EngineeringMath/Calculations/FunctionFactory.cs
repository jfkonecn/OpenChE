using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Calculations
{
    public class FunctionFactory
    {
        /// <summary>
        /// Data structure used to store all required data to build a function
        /// </summary>
        public class FactoryData
        {
            public FactoryData(Type funType, int outputID)
            {
                FunType = funType;
                OuputID = outputID;                
            }

            /// <summary>
            /// The type of function which will be created
            /// </summary>
            public Type FunType { get; private set; }

            /// <summary>
            /// The parameter ID of the function which will be the output
            /// </summary>
            public int OuputID { get; private set; }
        }


        
        /// <summary>
        /// Builds a function and then returns it to the user
        /// <para>Note: The default output will be used for the function</para>
        /// </summary>
        /// <param name="funType">The desired function type (which inherits the function class)</param>
        /// <returns></returns>
        public static Function BuildFunction(Type funType)
        {
            // Function to be returned
            Function myFun = (Function)Activator.CreateInstance(funType);

            myFun.FinishSetup();

            return myFun;
        }

        /// <summary>
        /// Builds a function and then returns it to the user
        /// </summary>
        /// <param name="funType">The desired function type (which inherits the function class)</param>
        /// <param name="outputID">Output Id number which will  </param>
        public static Function BuildFunction(Type funType, int outputID)
        {
            Function myFun = BuildFunction(funType);
            myFun.FieldDic[outputID].isOutput = true;
            return myFun;
        }
        /// <summary>
        /// Builds a function and then returns it to the user
        /// </summary>
        /// <param name="data">Factory data required to build a function</param>
        /// <returns></returns>
        public static Function BuildFunction(FactoryData data)
        {
            return BuildFunction(data.FunType, data.OuputID);
        }

    }
}
