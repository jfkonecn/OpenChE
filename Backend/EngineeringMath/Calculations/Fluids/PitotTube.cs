using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace EngineeringMath.Calculations.Fluids
{
    public class PitotTube : Function
    {

        public PitotTube()
        {
            FieldDic = new List<Parameter>
            {
                { new Parameter((int)Field.correctionCo, LibraryResources.CorrectionCo, new AbstractUnit[] { Unitless.unitless }, null, true, 0, 1) },
                { new Parameter((int)Field.deltaH, LibraryResources.ManoDeltaH, new AbstractUnit[] { Length.m }, null, true, 0) },
                { new Parameter((int)Field.manoDensity, LibraryResources.ManoDensity, new AbstractUnit[] { Density.kgm3 }, null, true, 0) },
                { new Parameter((int)Field.fluidDensity, LibraryResources.FluidDensity, new AbstractUnit[] { Density.kgm3 }, null, true, 0) },
                { new Parameter((int)Field.velo, LibraryResources.FluidVelocity, new AbstractUnit[] { Length.m, Time.sec }, null, false, 0) }
            }.ToDictionary(x => x.ID);


#if DEBUG
            FieldDic[0].ValueStr = "1";
            FieldDic[1].ValueStr = "1";
            FieldDic[2].ValueStr = "10";
            FieldDic[3].ValueStr = "8";
            FieldDic[4].ValueStr = "10";
#endif
        }
        /// <summary>
        /// The acceration of gravity in m/s^2
        /// </summary>
        private readonly double GRAVITY = 9.81;

        public enum Field
        {
            /// <summary>
            /// Velocity (m/s)
            /// </summary>
            velo,
            /// <summary>
            /// Correction Coefficient (unitless)
            /// </summary>
            correctionCo,
            /// <summary>
            /// Change in height (m)
            /// </summary>
            deltaH,
            /// <summary>
            /// Density of manometer fluid (kg/m3)
            /// </summary>
            manoDensity,
            /// <summary>
            /// Density of fluid (kg/m3)
            /// </summary>
            fluidDensity
        };

        /// <summary>
        /// Perform Pitot Tube Calculation
        /// </summary>
        /// <param name="outputID">ID which represents the enum field</param>
        /// <returns></returns>
        protected override double Calculation(int outputID)
        {
            double velo = FieldDic[(int)Field.velo].GetValue(),
                correctionCo = FieldDic[(int)Field.correctionCo].GetValue(),
                deltaH = FieldDic[(int)Field.deltaH].GetValue(),
                manoDensity = FieldDic[(int)Field.manoDensity].GetValue(),
                fluidDensity = FieldDic[(int)Field.fluidDensity].GetValue();

            switch ((Field)outputID)
            {
                case Field.correctionCo:
                    return velo / Math.Sqrt((2 * GRAVITY * deltaH * (manoDensity - fluidDensity)) / fluidDensity);
                case Field.deltaH:
                    return Math.Pow(velo / correctionCo, 2) * ((fluidDensity) / (2 * GRAVITY * (manoDensity - fluidDensity)));
                case Field.fluidDensity:
                    return manoDensity / (Math.Pow(velo / correctionCo, 2) * (1 / (2 * GRAVITY * deltaH)) + 1);
                case Field.manoDensity:
                    return fluidDensity * (Math.Pow(velo / correctionCo, 2) * (1 / (2 * GRAVITY * deltaH)) + 1);
                case Field.velo:
                    return correctionCo * Math.Sqrt((2 * GRAVITY * deltaH * (manoDensity - fluidDensity)) / fluidDensity);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
