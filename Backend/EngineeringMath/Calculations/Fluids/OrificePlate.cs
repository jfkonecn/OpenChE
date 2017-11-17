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
            STANDARD_OUTPUT = (int)Field.volFlow;

            fieldDic = new Dictionary<int, Parameter>
            {
                { (int)Field.disCo, new Parameter(disCoTitle, new AbstractUnit[] { Unitless.unitless }, true, double.Epsilon, 1.0) },
                { (int)Field.density, new Parameter(densityTitle, new AbstractUnit[] { Density.kgm3 }, true, double.Epsilon) },
                { (int)Field.pDia, new Parameter(pDiaTitle, new AbstractUnit[] { Length.m }, true, double.Epsilon) },
                { (int)Field.oDia, new Parameter(oDiaTitle, new AbstractUnit[] { Length.m }, true, double.Epsilon) },
                { (int)Field.deltaP, new Parameter(deltaPTitle, new AbstractUnit[] { Pressure.Pa }, true, 0.0) },
                { (int)Field.volFlow, new Parameter(volFlowTitle, new AbstractUnit[] { Volume.m3, Time.sec }, false, 0.0) }
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
        /// <returns></returns>
        protected override double Calculation()
        {

            if (fieldDic[(int)Field.pDia].GetValue() <= fieldDic[(int)Field.oDia].GetValue())
            {
                throw new System.ArgumentOutOfRangeException("Pipe Diameter must be greater than the orifice diameter", "orificeDia");
            }


            return fieldDic[(int)Field.disCo].GetValue() * PipeArea(fieldDic[(int)Field.pDia].GetValue()) *
                Math.Sqrt(
                    (2 * fieldDic[(int)Field.deltaP].GetValue()) /
                    (fieldDic[(int)Field.density].GetValue() *
                    (Math.Pow(PipeArea(fieldDic[(int)Field.pDia].GetValue()), 2) / Math.Pow(PipeArea(fieldDic[(int)Field.oDia].GetValue()), 2) - 1))
                    );
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
