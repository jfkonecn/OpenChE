using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : Function
    {
        /// <summary>
        /// Create orifice plate function
        /// <para>Note: The default output is volFlow</para>
        /// </summary>
        internal OrificePlate()
        {
            this.Title = LibraryResources.OrificePlate;
            
            FieldDic = new Dictionary<int, Parameter>
            {
                { (int)Field.disCo, new Parameter((int)Field.disCo, LibraryResources.DischargeCoefficient, new AbstractUnit[] { Unitless.unitless }, null, true, 0, 1.0) },
                { (int)Field.density, new Parameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Density.kgm3 }, null, true, 0) },
                { (int)Field.pArea, new Parameter((int)Field.pArea, LibraryResources.CircularPipe, new AbstractUnit[] { Units.Area.m2 },
                    new Dictionary<string, FunctionFactory.FactoryData>
                    {
                        { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                    } , true, 0)
                },
                { (int)Field.oArea, new Parameter((int)Field.oArea, LibraryResources.OrificeArea, new AbstractUnit[] { Units.Area.m2 },
                    new Dictionary<string, FunctionFactory.FactoryData>
                    {
                        { LibraryResources.CircularPipe, new FunctionFactory.FactoryData(typeof(Area.Circle), (int)Area.Circle.Field.cirArea) }
                    }, true, 0)
                },
                { (int)Field.deltaP, new Parameter((int)Field.deltaP, LibraryResources.PDAcrossOP, new AbstractUnit[] { Pressure.Pa }, null, true, 0.0) },
                { (int)Field.volFlow, new Parameter((int)Field.volFlow, LibraryResources.VolumetricFlowRate, new AbstractUnit[] { Volume.m3, Time.sec }, null, false, 0.0) }
            };

        }

        enum Field
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
                                (Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) / Math.Pow(FieldDic[(int)Field.pArea].GetValue(), 2) - 1))
                                ));
                case Field.density:
                    throw new NotImplementedException();
                case Field.pArea:
                    throw new NotImplementedException();
                case Field.oArea:
                    throw new NotImplementedException();
                case Field.deltaP:
                    throw new NotImplementedException();
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
