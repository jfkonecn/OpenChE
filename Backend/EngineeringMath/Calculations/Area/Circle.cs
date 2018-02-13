using EngineeringMath.Calculations;
using EngineeringMath.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Parameter;

namespace EngineeringMath.Calculations.Area
{
    public class Circle : SolveForFunction
    {

        /// <summary>
        /// 
        /// </summary>
        public Circle() : base(
                new SimpleParameter[]
                {
                    new SimpleParameter((int)Field.cirDia, LibraryResources.Diameter, new AbstractUnit[] { Length.m }, false, 0),
                    new SimpleParameter((int)Field.cirArea, LibraryResources.Area, new AbstractUnit[] { Units.Area.m2 }, false, 0)
                }
            )
        {

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
        /// Ciricle diameter (m)
        /// </summary>
        public double CircleDiameter
        {
            get
            {
                return GetParameter((int)Field.cirDia).Value;
            }

            set
            {
                GetParameter((int)Field.cirDia).Value = value;
            }
        }

        /// <summary>
        /// Ciricle area (m2)
        /// </summary>
        public double CircleArea
        {
            get
            {
                return GetParameter((int)Field.cirArea).Value;
            }

            set
            {
                GetParameter((int)Field.cirArea).Value = value;
            }
        }



        protected override SimpleParameter GetDefaultOutput()
        {
           return GetParameter((int)Field.cirArea);
        }

        protected override void Calculation()
        {

            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.cirDia:
                    CircleDiameter = Math.Sqrt((4d * CircleArea) / Math.PI);
                    break;
                case Field.cirArea:
                    CircleArea = Math.PI / 4 * Math.Pow(CircleDiameter, 2);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
