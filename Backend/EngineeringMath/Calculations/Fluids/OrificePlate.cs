using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : SolveForFunction
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public OrificePlate() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.disCo, LibraryResources.DischargeCoefficient, new AbstractUnit[] { Unitless.unitless }, true, 0, 1.0),
                    new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, true, 0),
                    new SubFunctionParameter((int)Field.pArea, LibraryResources.CircularPipe, new AbstractUnit[] { Units.Area.m2 },
                        new Dictionary<string, FunctionFactory.FactoryData>
                        {
                            { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                        } , true, 0),
                    new SubFunctionParameter((int)Field.oArea, LibraryResources.OrificeArea, new AbstractUnit[] { Units.Area.m2 },
                        new Dictionary<string, FunctionFactory.FactoryData>
                        {
                            { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                        }, true, 0),
                    new SubFunctionParameter((int)Field.deltaP, LibraryResources.PDAcrossOP, new AbstractUnit[] { Pressure.Pa },
                        new Dictionary<string, FunctionFactory.FactoryData>
                        {
                            { LibraryResources.DeltaP, new FunctionFactory.FactoryData(typeof(DeltaP), (int)DeltaP.Field.delta) }
                        }, true, 0.0),
                    new SimpleParameter((int)Field.volFlow, LibraryResources.VolumetricFlowRate, new AbstractUnit[] { Volume.m3, Time.sec }, false, 0.0)
                }
            )
        {
            this.Title = LibraryResources.OrificePlate;


#if DEBUG
            DischargeCoefficient = 1;
            Density = 1000;
            InletPipeArea = 10;
            OrificeArea = 8;
            PressureDrop = 10;
            VolumetricFlowRate = 0;
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
        public double DischargeCoefficient
        {
            get
            {
                return GetParameter((int)Field.disCo).Value;
            }

            set
            {
                GetParameter((int)Field.disCo).Value = value;
            }
        }

        /// <summary>
        /// Density of fluid going through orifice plate (kg/m3)
        /// </summary>
        public double Density
        {
            get
            {
                return GetParameter((int)Field.density).Value;
            }

            set
            {
                GetParameter((int)Field.density).Value = value;
            }
        }

        /// <summary>
        /// Inlet pipe area (m2)
        /// </summary>
        public double InletPipeArea
        {
            get
            {
                return GetParameter((int)Field.pArea).Value;
            }

            set
            {
                GetParameter((int)Field.pArea).Value = value;
            }
        }

        /// <summary>
        /// Orifice area (m2)
        /// </summary>
        public double OrificeArea
        {
            get
            {
                return GetParameter((int)Field.oArea).Value;
            }

            set
            {
                GetParameter((int)Field.oArea).Value = value;
            }
        }

        /// <summary>
        /// The DROP (p1 - p2) in pressure accross the orifice plate (Pa)
        /// </summary>
        public double PressureDrop
        {
            get
            {
                return GetParameter((int)Field.deltaP).Value;
            }

            set
            {
                GetParameter((int)Field.deltaP).Value = value;
            }
        }

        /// <summary>
        /// Volumetric flow rate (m3/s)
        /// </summary>
        public double VolumetricFlowRate
        {
            get
            {
                return GetParameter((int)Field.volFlow).Value;
            }

            set
            {
                GetParameter((int)Field.volFlow).Value = value;
            }
        }


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
                    DischargeCoefficient = VolumetricFlowRate /
                        (InletPipeArea *
                            Math.Sqrt(
                                (2 * PressureDrop) /
                                (Density *
                                (Math.Pow(InletPipeArea, 2) / Math.Pow(OrificeArea, 2) - 1))
                                ));
                    break;
                case Field.density:
                    Density = (2 * PressureDrop) /
                        ((Math.Pow(VolumetricFlowRate /
                        (DischargeCoefficient * InletPipeArea), 2)
                        ) *
                        ((Math.Pow(InletPipeArea, 2) / Math.Pow(OrificeArea, 2)) - 1));
                    break;
                case Field.pArea:
                    InletPipeArea = Math.Sqrt(1 / (
                        (1 / Math.Pow(OrificeArea, 2)) -
                        ((2 * PressureDrop * Math.Pow(DischargeCoefficient, 2)) /
                        (Math.Pow(VolumetricFlowRate, 2) * Density)
                        )));
                    break;
                case Field.oArea:
                    OrificeArea = Math.Sqrt(1 / (
                        (1 / Math.Pow(InletPipeArea, 2)) +
                        ((2 * PressureDrop * Math.Pow(DischargeCoefficient, 2)) /
                        (Math.Pow(VolumetricFlowRate, 2) * Density)
                        )));
                    break;
                case Field.deltaP:
                    PressureDrop = (Math.Pow(VolumetricFlowRate /
                        (DischargeCoefficient * InletPipeArea), 2) *
                        (Density *
                            (Math.Pow(InletPipeArea, 2) / Math.Pow(OrificeArea, 2) - 1))
                            ) / 2;
                    break;
                case Field.volFlow:
                    VolumetricFlowRate = DischargeCoefficient * InletPipeArea *
                        Math.Sqrt(
                            (2 * PressureDrop) /
                            (Density *
                            (Math.Pow(InletPipeArea, 2) / Math.Pow(OrificeArea, 2) - 1))
                            );
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
