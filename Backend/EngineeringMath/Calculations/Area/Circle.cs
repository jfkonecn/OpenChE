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
        public Circle()
        {
            CircleDiameter = new SimpleParameter((int)Field.cirDia, LibraryResources.Diameter, new AbstractUnit[] { Length.m }, false, 0);
            CircleArea = new SimpleParameter((int)Field.cirArea, LibraryResources.Area, new AbstractUnit[] { Units.Area.m2 }, false, 0);
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
        public readonly SimpleParameter CircleDiameter;

        /// <summary>
        /// Ciricle area (m2)
        /// </summary>
        public readonly SimpleParameter CircleArea;



        protected override SimpleParameter GetDefaultOutput()
        {
           return CircleArea;
        }

        protected override void Calculation()
        {

            switch ((Field)OutputSelection.SelectedObject.ID)
            {
                case Field.cirDia:
                    CircleDiameter.Value = Math.Sqrt((4d * CircleArea.Value) / Math.PI);
                    break;
                case Field.cirArea:
                    CircleArea.Value = Math.PI / 4 * Math.Pow(CircleDiameter.Value, 2);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public override SimpleParameter GetParameter(int ID)
        {
            switch ((Field)ID)
            {
                case Field.cirDia:
                    return CircleDiameter;
                case Field.cirArea:
                    return CircleArea;
                default:
                    throw new NotImplementedException();
            }
        }

        internal override IEnumerable<SimpleParameter> ParameterCollection()
        {
            yield return CircleDiameter;
            yield return CircleArea;
        }
    }
}
