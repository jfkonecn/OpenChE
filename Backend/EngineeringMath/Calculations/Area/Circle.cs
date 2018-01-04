using EngineeringMath.Calculations;
using EngineeringMath.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;

namespace EngineeringMath.Calculations.Area
{
    public class Circle : Function
    {

        /// <summary>
        /// 
        /// </summary>
        public Circle()
        {
            FieldDic = new Dictionary<int, Parameter>
            {
                { (int)Field.cirDia, new Parameter((int)Field.cirDia, LibraryResources.Diameter, new AbstractUnit[] { Length.m }, null, true, 0) },
                { (int)Field.cirArea, new Parameter((int)Field.cirArea, LibraryResources.Area, new AbstractUnit[] { Units.Area.m2 }, null, false, 0) }
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
                    return Math.Sqrt((4d * FieldDic[(int)Field.cirArea].GetValue()) / Math.PI);
                case Field.cirArea:
                    return Math.PI / 4 * Math.Pow(FieldDic[(int)Field.cirDia].GetValue(), 2);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
