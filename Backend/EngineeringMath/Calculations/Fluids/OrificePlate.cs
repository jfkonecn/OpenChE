using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.GenericObject;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.Fluids
{
    public class OrificePlate : Function
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disCoTitle">Title of discharge coefficient field</param>
        /// <param name="densityTitle">Title of density field</param>
        /// <param name="pDiaTitle">Title of pipe diameter field</param>
        /// <param name="oDiaTitle">Title of orifice diameter field</param>
        /// <param name="deltaPTitle">Title of change in pressure across the orifice plate</param>
        /// <param name="volFlowTitle">Title of</param>
        public OrificePlate(string disCoTitle, 
            string densityTitle, string pDiaTitle, 
            string oDiaTitle, string deltaPTitle, string volFlowTitle)
        {
            fieldDic = new Dictionary<int, Parameter>
            {
                { (int)Field.disCo, new Parameter((int)Field.disCo, disCoTitle, new AbstractUnit[] { Unitless.unitless }, true, 0, 1.0) },
                { (int)Field.density, new Parameter((int)Field.density, densityTitle, new AbstractUnit[] { Density.kgm3 }, true, 0) },
                { (int)Field.pDia, new Parameter((int)Field.pDia, pDiaTitle, new AbstractUnit[] { Length.m }, true, 0) },
                { (int)Field.oDia, new Parameter((int)Field.oDia, oDiaTitle, new AbstractUnit[] { Length.m }, true, 0) },
                { (int)Field.deltaP, new Parameter((int)Field.deltaP, deltaPTitle, new AbstractUnit[] { Pressure.Pa }, true, 0.0) },
                { (int)Field.volFlow, new Parameter((int)Field.volFlow, volFlowTitle, new AbstractUnit[] { Volume.m3, Time.sec }, false, 0.0) }
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
            /// Inlet pipe diameter (m)
            /// </summary>
            pDia,
            /// <summary>
            /// Orifice diameter (m)
            /// </summary>
            oDia,
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
                    return  fieldDic[(int)Field.volFlow].GetValue() / 
                        (PipeArea(fieldDic[(int)Field.pDia].GetValue()) *
                            Math.Sqrt(
                                (2 * fieldDic[(int)Field.deltaP].GetValue()) /
                                (fieldDic[(int)Field.density].GetValue() *
                                (Math.Pow(PipeArea(fieldDic[(int)Field.pDia].GetValue()), 2) / Math.Pow(PipeArea(fieldDic[(int)Field.oDia].GetValue()), 2) - 1))
                                ));
                case Field.density:
                    throw new NotImplementedException();
                case Field.pDia:
                    throw new NotImplementedException();
                case Field.oDia:
                    throw new NotImplementedException();
                case Field.deltaP:
                    throw new NotImplementedException();
                case Field.volFlow:
                    return fieldDic[(int)Field.disCo].GetValue() * PipeArea(fieldDic[(int)Field.pDia].GetValue()) *
                        Math.Sqrt(
                            (2 * fieldDic[(int)Field.deltaP].GetValue()) /
                            (fieldDic[(int)Field.density].GetValue() *
                            (Math.Pow(PipeArea(fieldDic[(int)Field.pDia].GetValue()), 2) / Math.Pow(PipeArea(fieldDic[(int)Field.oDia].GetValue()), 2) - 1))
                            );
                default:
                    throw new NotImplementedException();
            }




        }

        /// <summary>
        /// calculates the area of a pipe given its diameter
        /// </summary>
        /// <param name="pipeDia">pipe diameter (m)</param>
        /// <returns>pipe area (m2)</returns>
        private static double PipeArea(double pipeDia)
        {
            return Math.PI / 4 * pipeDia * pipeDia;
        }
    }
}
