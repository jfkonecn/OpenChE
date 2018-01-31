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
            this.Title = LibraryResources.UnitConverter;
        }


        internal static readonly Dictionary<string, FunctionFactory.SolveForFactoryData> AllUnitFunctionData =
            new Dictionary<string, FunctionFactory.SolveForFactoryData>
            {
                        { LibraryResources.Pressure, new FunctionFactory.SolveForFactoryData(typeof(PressureFun)) },
                        { LibraryResources.VolumetricFlowRate, new FunctionFactory.SolveForFactoryData(typeof(VolumetricFlowFun)) },
                        { LibraryResources.Temperature, new FunctionFactory.SolveForFactoryData(typeof(TemperatureFun)) },
                        { LibraryResources.MassFlowRate, new FunctionFactory.SolveForFactoryData(typeof(MassFlowFun)) },
                        { LibraryResources.Mass, new FunctionFactory.SolveForFactoryData(typeof(MassFun)) },
                        { LibraryResources.Volume, new FunctionFactory.SolveForFactoryData(typeof(VolumeFun)) },
                        { LibraryResources.Energy, new FunctionFactory.SolveForFactoryData(typeof(EnergyFun)) }
            }.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

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

        public class TemperatureFun : AbstractConverter
        {
            public TemperatureFun() : base(new AbstractUnit[] { Temperature.C }) { }
        }

        public class MassFlowFun : AbstractConverter
        {
            public MassFlowFun() : base(new AbstractUnit[] { Mass.g }) { }
        }

        public class MassFun : AbstractConverter
        {
            public MassFun() : base(new AbstractUnit[] { Mass.g }) { }
        }

        public class VolumeFun : AbstractConverter
        {
            public VolumeFun() : base(new AbstractUnit[] { Volume.m3 }) { }
        }

        public class EnergyFun : AbstractConverter
        {
            public EnergyFun() : base(new AbstractUnit[] { Energy.kJ }) { }
        }
    }
}
