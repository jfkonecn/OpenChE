using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations
{
    public class FunctionFactory
    {
        /// <summary>
        /// Data structure used to store all required data to build a function
        /// </summary>
        public class SolveForFactoryData
        {
            /// <summary>
            /// The ID when none is given
            /// </summary>
            internal static readonly int NULL_ID = -1;

            public SolveForFactoryData(Type funType) : this(funType, NULL_ID)
            {
            }

            public SolveForFactoryData(Type funType, int outputID)
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
        /// <param name="funType">The desired function type (which inherits the SimpleFunction class)</param>
        /// <returns></returns>
        public static SimpleFunction BuildFunction(Type funType)
        {
            // Function to be returned
            SimpleFunction myFun = (SimpleFunction)Activator.CreateInstance(funType);

            return myFun;
        }


    }
}
