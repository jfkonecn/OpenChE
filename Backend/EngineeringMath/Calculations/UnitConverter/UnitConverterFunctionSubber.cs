using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.UnitConverter
{
    public class UnitConverterFunctionSubber : FunctionSubber
    {
        public UnitConverterFunctionSubber() : base(AllUnitFunctionData)
        {
           
        }


        internal static readonly Dictionary<string, FunctionFactory.SolveForFactoryData> AllUnitFunctionData =
            new Dictionary<string, FunctionFactory.SolveForFactoryData>
            {
                        { LibraryResources.Pressure, new FunctionFactory.SolveForFactoryData(typeof(PressureFun)) },
                        { LibraryResources.VolumetricFlowRate, new FunctionFactory.SolveForFactoryData(typeof(VolumetricFlowFun)) }
            };

        public class PressureFun : AbstractConverter
        {
            public PressureFun() : base(new AbstractUnit[] { Pressure.Pa })
            {

            }
        }

        public class VolumetricFlowFun : AbstractConverter
        {
            public VolumetricFlowFun() : base(new AbstractUnit[] { Volume.m3, Time.sec })
            {

            }
        }
    }
}
