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


        private static readonly Dictionary<string, Type> AllUnitFunctionData =
            new Dictionary<string, Type>
            {
                        { LibraryResources.Pressure, typeof(PressureFun) },
                        { LibraryResources.VolumetricFlowRate, typeof(VolumetricFlowFun) },
                        { LibraryResources.Temperature, typeof(TemperatureFun) },
                        { LibraryResources.MassFlowRate, typeof(MassFlowFun) },
                        { LibraryResources.Mass, typeof(MassFun) },
                        { LibraryResources.Volume, typeof(VolumeFun) },
                        { LibraryResources.Energy, typeof(EnergyFun) }
            }.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        public class PressureFun : UnitConverter
        {
            public PressureFun() : base()
            {

            }


            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Pressure.Pa };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class VolumetricFlowFun : UnitConverter
        {
            public VolumetricFlowFun() : base()
            {

            }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Volume.m3, Time.sec };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class TemperatureFun : UnitConverter
        {
            public TemperatureFun() : base() { }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Temperature.C };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class MassFlowFun : UnitConverter
        {
            public MassFlowFun() : base() { }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Mass.g };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class MassFun : UnitConverter
        {
            public MassFun() : base() { }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Mass.g };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class VolumeFun : UnitConverter
        {
            public VolumeFun() : base() { }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Volume.m3 };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }

        public class EnergyFun : UnitConverter
        {
            public EnergyFun() : base() { }

            private readonly AbstractUnit[] _Units = new AbstractUnit[] { Energy.kJ };
            protected override AbstractUnit[] Units
            {
                get
                {
                    return _Units;
                }
            }
        }
    }
}
