using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.SupportFunctions.InletOutletDifferential;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : Function
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        public OrificePlate()
        {
            this.Title = LibraryResources.OrificePlate;

            FieldDic = new List<Parameter>
            {
                { new Parameter((int)Field.disCo, LibraryResources.DischargeCoefficient, new AbstractUnit[] { Unitless.unitless }, null, true, 0, 1.0) },
                { new Parameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Density.kgm3 }, null, true, 0) },
                { new Parameter((int)Field.pArea, LibraryResources.CircularPipe, new AbstractUnit[] { Units.Area.m2 },
                    new Dictionary<string, FunctionFactory.FactoryData>
                    {
                        { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                    } , true, 0)
                },
                { new Parameter((int)Field.oArea, LibraryResources.OrificeArea, new AbstractUnit[] { Units.Area.m2 },
                    new Dictionary<string, FunctionFactory.FactoryData>
                    {
                        { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                    }, true, 0)
                },
                { new Parameter((int)Field.deltaP, LibraryResources.PDAcrossOP, new AbstractUnit[] { Pressure.Pa }, 
                    new Dictionary<string, FunctionFactory.FactoryData>
                    {
                        { LibraryResources.DeltaP, new FunctionFactory.FactoryData(typeof(DeltaP), (int)DeltaP.Field.delta) }
                    }, true, 0.0) },
                { new Parameter((int)Field.volFlow, LibraryResources.VolumetricFlowRate, new AbstractUnit[] { Volume.m3, Time.sec }, null, false, 0.0) }
            }.ToDictionary(x => x.ID);

#if DEBUG
            FieldDic[0].ValueStr = "1";
            FieldDic[1].ValueStr = "1000";
            FieldDic[2].ValueStr = "10";
            FieldDic[3].ValueStr = "8";
            FieldDic[4].ValueStr = "10";
            FieldDic[5].ValueStr = "0.0";
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
        /// Perform Orifice Plate Calculation
        /// </summary>
        /// <param name="outputID">ID which represents the enum field</param>
        /// <returns></returns>
        protected override double Calculation(int outputID)
        {

            switch ((Field)outputID)
            {
                case Field.disCo:
                    return  FieldDic[(int)Field.volFlow].GetValue() / 
                        (FieldDic[(int)Field.pArea].GetValue() *
                            Math.Sqrt(
                                (2 * FieldDic[(int)Field.deltaP].GetValue()) /
                                (FieldDic[(int)Field.density].GetValue() *
                                (Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) / Math.Pow(FieldDic[(int)Field.oArea].GetValue(), 2) - 1))
                                ));
                case Field.density:
                    return (2 * FieldDic[(int)Field.deltaP].GetValue()) / 
                        ((Math.Pow(FieldDic[(int)Field.volFlow].GetValue() / 
                        (FieldDic[(int)Field.disCo].GetValue() * FieldDic[(int)Field.pArea].GetValue()), 2)
                        ) * 
                        ((Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) / Math.Pow(FieldDic[(int)Field.oArea].GetValue(), 2)) - 1));
                case Field.pArea:
                    return Math.Sqrt(1 / (
                        (1 / Math.Pow(FieldDic[(int)Field.oArea].GetValue(), 2)) -
                        ((2 * FieldDic[(int)Field.deltaP].GetValue() * Math.Pow(FieldDic[(int)Field.disCo].GetValue(), 2) ) /
                        (Math.Pow(FieldDic[(int)Field.volFlow].GetValue(), 2) * FieldDic[(int)Field.density].GetValue())
                        )));
                case Field.oArea:
                    return Math.Sqrt(1 / (
                        (1 / Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2)) +
                        ((2 * FieldDic[(int)Field.deltaP].GetValue() * Math.Pow(FieldDic[(int)Field.disCo].GetValue(), 2)) /
                        (Math.Pow(FieldDic[(int)Field.volFlow].GetValue(), 2) * FieldDic[(int)Field.density].GetValue())
                        )));
                case Field.deltaP:
                    return (Math.Pow(FieldDic[(int)Field.volFlow].GetValue() /
                        (FieldDic[(int)Field.disCo].GetValue() * FieldDic[(int)Field.pArea].GetValue()), 2) *
                        (FieldDic[(int)Field.density].GetValue() *
                            (Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) / Math.Pow(FieldDic[(int)Field.oArea].GetValue(), 2) - 1))
                            ) / 2;
                case Field.volFlow:
                    return FieldDic[(int)Field.disCo].GetValue() * FieldDic[(int)Field.pArea].GetValue() *
                        Math.Sqrt(
                            (2 * FieldDic[(int)Field.deltaP].GetValue()) /
                            (FieldDic[(int)Field.density].GetValue() *
                            (Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) / Math.Pow(FieldDic[(int)Field.oArea].GetValue(), 2) - 1))
                            );
                default:
                    throw new NotImplementedException();
            }




        }
    }
}
