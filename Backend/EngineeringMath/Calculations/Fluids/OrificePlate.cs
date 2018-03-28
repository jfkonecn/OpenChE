using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : SolveForFunction
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public OrificePlate()
        {

            DischargeCoefficient = new SimpleParameter((int)Field.disCo, LibraryResources.DischargeCoefficient, new AbstractUnit[] { Unitless.unitless }, true, 0, 1.0);
            Density = new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, true, 0);
            InletPipeArea = new SubFunctionParameter((int)Field.pArea, LibraryResources.CircularPipe, new AbstractUnit[] { Units.Area.m2 },
                new Dictionary<string, FunctionFactory.SolveForFactoryData>
                {
                    { LibraryResources.CircularPipe, new FunctionFactory.SolveForFactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                }, true, 0);
            OrificeArea = new SubFunctionParameter((int)Field.oArea, LibraryResources.OrificeArea, new AbstractUnit[] { Units.Area.m2 },
                new Dictionary<string, FunctionFactory.SolveForFactoryData>
                {
                    { LibraryResources.CircularPipe, new FunctionFactory.SolveForFactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                }, true, 0);
            PressureDrop = new SubFunctionParameter((int)Field.deltaP, LibraryResources.PDAcrossOP, new AbstractUnit[] { Pressure.Pa },
                new Dictionary<string, FunctionFactory.SolveForFactoryData>
                {
                    { LibraryResources.DeltaP, new FunctionFactory.SolveForFactoryData(typeof(DeltaP), (int)DeltaP.Field.delta) }
                }, true, 0.0);
            VolumetricFlowRate = new SimpleParameter((int)Field.volFlow, LibraryResources.VolumetricFlowRate, new AbstractUnit[] { Volume.m3, Time.sec }, false, 0.0);





            this.Title = LibraryResources.OrificePlate;


#if DEBUG
            DischargeCoefficient.Value = 1;
            Density.Value = 1000;
            InletPipeArea.Value = 10;
            OrificeArea.Value = 8;
            PressureDrop.Value = 10;
            VolumetricFlowRate.Value = 0;
#endif

        }

        public enum Field
        {
            /// <summary>
            /// Discharge coefficient (unitless)
            /// </summary>
            disCo,
            /// <summary>
            /// Density of fluid going through orifice plate (kg/m3)
            /// </summary>
            density,
            /// <summary>
            /// Inlet pipe area (m2)
            /// </summary>
            pArea,
            /// <summary>
            /// Orifice area (m2)
            /// </summary>
            oArea,
            /// <summary>
            /// The DROP (p1 - p2) in pressure accross the orifice plate (Pa)
            /// </summary>
            deltaP,
            /// <summary>
            /// Volumetric flow rate (m3/s)
            /// </summary>
            volFlow
        };


        /// <summary>
        /// Discharge coefficient (unitless)
        /// </summary>
        public readonly SimpleParameter DischargeCoefficient;

        /// <summary>
        /// Density of fluid going through orifice plate (kg/m3)
        /// </summary>
        public readonly SimpleParameter Density;

        /// <summary>
        /// Inlet pipe area (m2)
        /// </summary>
        public readonly SimpleParameter InletPipeArea;

        /// <summary>
        /// Orifice area (m2)
        /// </summary>
        public readonly SimpleParameter OrificeArea;

        /// <summary>
        /// The DROP (p1 - p2) in pressure accross the orifice plate (Pa)
        /// </summary>
        public readonly SimpleParameter PressureDrop;

        /// <summary>
        /// Volumetric flow rate (m3/s)
        /// </summary>
        public readonly SimpleParameter VolumetricFlowRate;


        protected override SimpleParameter GetDefaultOutput()
        {
            return GetParameter((int)Field.volFlow);
        }

        /// <summary>
        /// Perform Orifice Plate Calculation
        /// </summary>
        protected override void Calculation()
        {
            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.disCo:
                    DischargeCoefficient.Value = VolumetricFlowRate.Value /
                        (InletPipeArea.Value *
                            Math.Sqrt(
                                (2 * PressureDrop.Value) /
                                (Density.Value *
                                (Math.Pow(InletPipeArea.Value, 2) / Math.Pow(OrificeArea.Value, 2) - 1))
                                ));
                    break;
                case Field.density:
                    Density.Value = (2 * PressureDrop.Value) /
                        ((Math.Pow(VolumetricFlowRate.Value /
                        (DischargeCoefficient.Value * InletPipeArea.Value), 2)
                        ) *
                        ((Math.Pow(InletPipeArea.Value, 2) / Math.Pow(OrificeArea.Value, 2)) - 1));
                    break;
                case Field.pArea:
                    InletPipeArea.Value = Math.Sqrt(1 / (
                        (1 / Math.Pow(OrificeArea.Value, 2)) -
                        ((2 * PressureDrop.Value * Math.Pow(DischargeCoefficient.Value, 2)) /
                        (Math.Pow(VolumetricFlowRate.Value, 2) * Density.Value)
                        )));
                    break;
                case Field.oArea:
                    OrificeArea.Value = Math.Sqrt(1 / (
                        (1 / Math.Pow(InletPipeArea.Value, 2)) +
                        ((2 * PressureDrop.Value * Math.Pow(DischargeCoefficient.Value, 2)) /
                        (Math.Pow(VolumetricFlowRate.Value, 2) * Density.Value)
                        )));
                    break;
                case Field.deltaP:
                    PressureDrop.Value = (Math.Pow(VolumetricFlowRate.Value /
                        (DischargeCoefficient.Value * InletPipeArea.Value), 2) *
                        (Density.Value *
                            (Math.Pow(InletPipeArea.Value, 2) / Math.Pow(OrificeArea.Value, 2) - 1))
                            ) / 2;
                    break;
                case Field.volFlow:
                    VolumetricFlowRate.Value = DischargeCoefficient.Value * InletPipeArea.Value *
                        Math.Sqrt(
                            (2 * PressureDrop.Value) /
                            (Density.Value *
                            (Math.Pow(InletPipeArea.Value, 2) / Math.Pow(OrificeArea.Value, 2) - 1))
                            );
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.disCo:
                    return DischargeCoefficient;
                case Field.density:
                    return Density;
                case Field.pArea:
                    return InletPipeArea;
                case Field.oArea:
                    return OrificeArea;
                case Field.deltaP:
                    return PressureDrop;
                case Field.volFlow:
                    return VolumetricFlowRate;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return DischargeCoefficient;
            yield return Density;
            yield return InletPipeArea;
            yield return OrificeArea;
            yield return PressureDrop;
            yield return VolumetricFlowRate;
        }
    }
}
