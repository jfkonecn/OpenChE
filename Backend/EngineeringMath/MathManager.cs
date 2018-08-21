using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath
{
    /// <summary>
    /// Class by which all actions start from with the UI
    /// </summary>
    public static class MathManager
    {



        
        public static UnitCategoryCollection AllUnits
        {
            get
            {
                return UnitCategoryCollection.AllUnits;
            }
        }
        

        public static FunctionCategoryCollection AllFunctions
        {
            get
            {
                return FunctionCategoryCollection.AllFunctions;
            }
        }
    }
}
