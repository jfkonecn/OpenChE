﻿using EngineeringMath.Calculations;
using EngineeringMath.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineeringMath.Calculations.Area
{
    class Circle : Function
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cirDiaTitle">Title of circle diameter field</param>
        public Circle(string cirDiaTitle, string cirAreaTitle)
        {
            fieldDic = new Dictionary<int, Parameter>
            {
                { (int)Field.cirDia, new Parameter((int)Field.cirDia, "Diameter", new AbstractUnit[] { Length.m }, null, true, 0, 1.0) },
                { (int)Field.cirArea, new Parameter((int)Field.cirArea, "Area", new AbstractUnit[] { Units.Area.m2 }, null, false, 0.0) }
            };
        }

        public enum Field
        {
            /// <summary>
            /// Ciricle diameter (m)
            /// </summary>
            cirDia,
            /// <summary>
            /// Ciricle area (m2)
            /// </summary>
            cirArea
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
                case Field.cirDia:
                    return Math.PI / 4 * Math.Pow( fieldDic[(int)Field.cirDia].GetValue(), 2);
                case Field.cirArea:
                    return Math.Sqrt((4d * fieldDic[(int)Field.cirArea].GetValue()) / Math.PI );
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
