using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.Fluids
{
    public class BernoullisEquation : Function
    {
        public BernoullisEquation()
        {
            FieldDic = new List<Parameter>
            {
                { new Parameter((int)Field.inletVelo, LibraryResources.InletVelo, new AbstractUnit[] { Length.m, Time.sec }, null, true, 0) },
                { new Parameter((int)Field.outletVelo, LibraryResources.OutletVelo, new AbstractUnit[] { Length.m, Time.sec }, null, true, 0) },
                { new Parameter((int)Field.inletHeight, LibraryResources.InletHeight, new AbstractUnit[] { Length.m }, null, true, 0) },
                { new Parameter((int)Field.outletHeight, LibraryResources.OutletHeight, new AbstractUnit[] { Length.m }, null, true, 0) },
                { new Parameter((int)Field.inletP, LibraryResources.InletP, new AbstractUnit[] { Pressure.Pa }, null, true, 0) },
                { new Parameter((int)Field.outletP, LibraryResources.OutletP, new AbstractUnit[] { Pressure.Pa }, null, true, 0) },
                { new Parameter((int)Field.density, LibraryResources.Density, new AbstractUnit[] { Density.kgm3 }, null, false, 0) }
            }.ToDictionary(x => x.ID);


#if DEBUG
            FieldDic[0].ValueStr = "1";
            FieldDic[1].ValueStr = "1000";
            FieldDic[2].ValueStr = "10";
            FieldDic[3].ValueStr = "8";
            FieldDic[4].ValueStr = "10";
            FieldDic[5].ValueStr = "100";
            FieldDic[6].ValueStr = "1000";
#endif
        }

        /// <summary>
        /// The acceration of gravity in m/s^2
        /// </summary>
        private readonly double GRAVITY = 9.81;

        public enum Field
        {
            /// <summary>
            /// Inlet Velocity (m/s)
            /// </summary>
            inletVelo,
            /// <summary>
            /// Outlet Velocity (m/s)
            /// </summary>
            outletVelo,
            /// <summary>
            /// Inlet Height (m)
            /// </summary>
            inletHeight,
            /// <summary>
            /// Outlet Height (m)
            /// </summary>
            outletHeight,
            /// <summary>
            /// Inlet Pressure (Pa)
            /// </summary>
            inletP,
            /// <summary>
            /// Outlet Pressure (Pa)
            /// </summary>
            outletP,
            /// <summary>
            /// Density of fluid (kg/m3)
            /// </summary>
            density
        };

        /// <summary>
        /// Perform Bernoulli's Equation Calculation
        /// </summary>
        /// <param name="outputID">ID which represents the enum field</param>
        /// <returns></returns>
        protected override double Calculation(int outputID)
        {
            double inletVeloSqu = Math.Pow(FieldDic[(int)Field.inletVelo].GetValue(), 2),
                outletVeloSqu = Math.Pow(FieldDic[(int)Field.outletVelo].GetValue(), 2),
                inletHeight = FieldDic[(int)Field.inletHeight].GetValue(),
                outletHeight = FieldDic[(int)Field.outletHeight].GetValue(),
                inletP = FieldDic[(int)Field.inletP].GetValue(),
                outletP = FieldDic[(int)Field.outletP].GetValue(),
                density = FieldDic[(int)Field.density].GetValue();


            switch ((Field)outputID)
            {
                case Field.inletVelo:
                    return Math.Sqrt(
                        2 * ((outletVeloSqu / 2 + GRAVITY * outletHeight + outletP / density) -
                        (GRAVITY * inletHeight + inletP / density))
                        );
                case Field.outletVelo:
                    return Math.Sqrt(
                        2 * ((inletVeloSqu / 2 + GRAVITY * inletHeight + inletP / density) -
                        (GRAVITY * outletHeight + outletP / density))
                        );
                case Field.inletHeight:
                    return (1 / GRAVITY) * ((outletVeloSqu / 2 + GRAVITY * outletHeight + outletP / density) -
                        (inletVeloSqu / 2 + inletP / density));
                case Field.outletHeight:
                    return (1 / GRAVITY) * ((inletVeloSqu / 2 + GRAVITY * inletHeight + inletP / density) -
                        (outletVeloSqu / 2 + outletP / density));
                case Field.inletP:
                    return density * ((outletVeloSqu / 2 + GRAVITY * outletHeight + outletP / density) -
                        (inletVeloSqu / 2 + GRAVITY * inletHeight));
                case Field.outletP:
                    return density * ((inletVeloSqu / 2 + GRAVITY * inletHeight + inletP / density) -
                        (outletVeloSqu / 2 + GRAVITY * outletHeight));
                case Field.density:
                    return (outletP - inletP) / 
                        ((inletVeloSqu / 2 + GRAVITY * inletHeight) -
                        (outletVeloSqu / 2 + GRAVITY * outletHeight));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
