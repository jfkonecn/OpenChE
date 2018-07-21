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
using System.Collections.ObjectModel;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : SolveForFunction
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public OrificePlate() : base()
        {

            this.Title = LibraryResources.OrificePlate;
            BuildComponentCollection();

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
        public SimpleParameter DischargeCoefficient
        {
            get
            {
                return GetParameter((int)Field.disCo);
            }
        }

        /// <summary>
        /// Density of fluid going through orifice plate (kg/m3)
        /// </summary>
        public SimpleParameter Density
        {
            get
            {
                return GetParameter((int)Field.density);
            }
        }

        /// <summary>
        /// Inlet pipe area (m2)
        /// </summary>
        public SimpleParameter InletPipeArea
        {
            get
            {
                return GetParameter((int)Field.pArea);
            }
        }

        /// <summary>
        /// Orifice area (m2)
        /// </summary>
        public SimpleParameter OrificeArea
        {
            get
            {
                return GetParameter((int)Field.oArea);
            }
        }

        /// <summary>
        /// The DROP (p1 - p2) in pressure accross the orifice plate (Pa)
        /// </summary>
        public SimpleParameter PressureDrop
        {
            get
            {
                return GetParameter((int)Field.deltaP);
            }
        }

        /// <summary>
        /// Volumetric flow rate (m3/s)
        /// </summary>
        public SimpleParameter VolumetricFlowRate
        {
            get
            {
                return GetParameter((int)Field.volFlow);
            }
        }


        protected override SimpleParameter GetDefaultOutput()
        {
            return VolumetricFlowRate;
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

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.disCo, LibraryResources.DischargeCoefficient, new AbstractUnit[] { Unitless.unitless }, true, 0, 1.0),
                new SimpleParameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Units.Density.kgm3 }, true, 0),
                new SubFunctionParameter((int)Field.pArea, LibraryResources.InletPipeArea, new AbstractUnit[] { Units.Area.m2 },
                new Dictionary<string, ComponentFactory.SolveForFactoryData>
                {
                    { LibraryResources.CircularPipe, new ComponentFactory.SolveForFactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                }, true, 0),
                new SubFunctionParameter((int)Field.oArea, LibraryResources.OrificeArea, new AbstractUnit[] { Units.Area.m2 },
                new Dictionary<string, ComponentFactory.SolveForFactoryData>
                {
                    { LibraryResources.CircularPipe, new ComponentFactory.SolveForFactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                }, true, 0),
                new SubFunctionParameter((int)Field.deltaP, LibraryResources.PDAcrossOP, new AbstractUnit[] { Pressure.Pa },
                new Dictionary<string, ComponentFactory.SolveForFactoryData>
                {
                    { LibraryResources.DeltaP, new ComponentFactory.SolveForFactoryData(typeof(DeltaP), (int)DeltaP.Field.delta) }
                }, true, 0.0),
                new SimpleParameter((int)Field.volFlow, LibraryResources.VolumetricFlowRate, new AbstractUnit[] { Volume.m3, Time.sec }, false, 0.0)
            };
        }
    }
}
