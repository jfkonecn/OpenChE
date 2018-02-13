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
        /// Data structure used to store all required data to build a function with Parameter links
        /// </summary>
        public class ParameterLinksFactoryData
        {

            public ParameterLinksFactoryData(Type funType, ParameterLink links) : this(funType, new ParameterLink[] { links })
            {

            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="funType"></param>
            /// <param name="links"></param>
            public ParameterLinksFactoryData(Type funType, ParameterLink[] links)
            {
                FunType = funType;
                HomeIDtoSubID = links.ToDictionary(x => x.HomeID, x => x.SubID);
                SubIDtoHomeID = links.ToDictionary(x => x.SubID, x => x.HomeID);
            }

            /// <summary>
            /// Gets the home id given the sub id
            /// </summary>
            private Dictionary<int, int> SubIDtoHomeID;

            /// <summary>
            /// Gets the sub id given the home id
            /// </summary>
            private Dictionary<int, int> HomeIDtoSubID;
            /// <summary>
            /// The type of function which will be created
            /// </summary>
            public Type FunType { get; private set; }

            /// <summary>
            /// Gets the subID given the homeID of parameter link
            /// </summary>
            /// <param name="homeID"></param>
            /// <returns></returns>
            public int GetSubID(int homeID)
            {
                return HomeIDtoSubID[homeID];
            }

            /// <summary>
            /// Gets the homeID given the homeID of parameter link
            /// </summary>
            /// <param name="homeID"></param>
            /// <returns></returns>
            public int GetHomeID(int subID)
            {
                return SubIDtoHomeID[subID];
            }

        }

        /// <summary>
        /// Stores the IDs of two parameters in different functions which represent the same value
        /// Example: The ID for the density of a gas in an office plate could be matched up with the density calculated by the ideal gas law
        /// </summary>
        public class ParameterLink
        {

            ParameterLink(int homeID, int subID)
            {
                HomeID = homeID;
                SubID = subID;
            }
            /// <summary>
            /// The ID of the parameter in the home function
            /// </summary>
            public int HomeID { get; private set; }
            /// <summary>
            /// The ID of the parameter in the subfunction
            /// </summary>
            public int SubID { get; private set; }
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
