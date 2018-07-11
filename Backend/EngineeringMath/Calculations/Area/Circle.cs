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
using System.Collections.ObjectModel;
using EngineeringMath.Calculations.Components;

namespace EngineeringMath.Calculations.Area
{
    public class Circle : SolveForFunction
    {

        public Circle() : base()
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
        public SimpleParameter CircleDiameter
        {
            get
            {
                return GetParameter((int)Field.cirDia);
            }
        }

        /// <summary>
        /// Ciricle area (m2)
        /// </summary>
        public SimpleParameter CircleArea
        {
            get
            {
                return GetParameter((int)Field.cirArea);
            }
        }
        



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

        protected override ObservableCollection<AbstractComponent> CreateRemainingDefaultComponentCollection()
        {
            return new ObservableCollection<AbstractComponent>
            {
                new SimpleParameter((int)Field.cirDia, LibraryResources.Diameter, new AbstractUnit[] { Length.m }, false, 0),
                new SimpleParameter((int) Field.cirArea, LibraryResources.Area, new AbstractUnit[] { Units.Area.m2 }, false, 0)
            };
        }
    }
}
