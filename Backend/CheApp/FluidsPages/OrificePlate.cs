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


        public OrificePlate() : base()
        {



            this.PageSetup(new EngineeringMath.Calculations.Fluids.OrificePlate(
                "Discharge Coefficient",
                "Density",
                "Inlet Pipe Diameter",
                "Orifice Diameter",
                "Drop in Pressure (pIn - pOut) Across Orifice Plate",
                "Volumetric Flow Rate"));
#if DEBUG            
            myFun.fieldDic[0].ValueStr = "1";
            myFun.fieldDic[1].ValueStr = "1000";
            myFun.fieldDic[2].ValueStr = "10";
            myFun.fieldDic[3].ValueStr = "8";
            myFun.fieldDic[4].ValueStr = "10";
#endif
            myFun.fieldDic[5].ValueStr = "0.0";
        }



 
    }
}