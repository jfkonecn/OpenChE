using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;
using EngineeringMath.Calculations;

using Xamarin.Forms;

namespace CheApp.FluidsPages
{

    /// <summary>
    /// Performs orifice plate calculations
    /// </summary>
    public class OrificePlate : BasicPage
    {
        private static readonly Function PAGE_FUN = FunctionFactory.BuildFunction(typeof(EngineeringMath.Calculations.Fluids.OrificePlate));

        public OrificePlate() : base(PAGE_FUN)
        {
#if DEBUG            
            myFun.FieldDic[0].ValueStr = "1";
            myFun.FieldDic[1].ValueStr = "1000";
            myFun.FieldDic[2].ValueStr = "10";
            myFun.FieldDic[3].ValueStr = "8";
            myFun.FieldDic[4].ValueStr = "10";
#endif
            myFun.FieldDic[5].ValueStr = "0.0";
        }



 
    }
}