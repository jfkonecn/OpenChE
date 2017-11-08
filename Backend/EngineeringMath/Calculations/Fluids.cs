using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Calculations
{
    // TODO: make it so that functions have an object that set high and low parameter inputs
    // TODO: creat object to throw in case of an exception so that the basic page can know what parameter was the reason for the error
    public class Fluids
    {
        const string GREATER_THAN = "Parameter must greater than 0";

        /// <summary>
        /// Calculates the volumetric flow rate of an orifice plate
        /// </summary>
        /// <param name="disCo">Discharge coefficient (unitless)</param>
        /// <param name="density">Density of fluid going through orifice plate (kg/m3)</param>
        /// <param name="pipeDia">Inlet pipe diameter (m)</param>
        /// <param name="orificeDia">Orifice diameter (m)</param>
        /// <param name="pIn">Absolute inlet pressure (pa)</param>
        /// <param name="pOut">Absolute pressure at Outlet/Orifice (pa)</param>
        /// <returns>Volumetric flow rate (m3/s)</returns>
        public static double OrificePlate(
            double disCo, double density,
            double pipeDia, double orificeDia,
            double pIn, double pOut)
        {
            //parameter check
            if (pIn <= 0)
            {
                throw new System.ArgumentOutOfRangeException(GREATER_THAN, "density");
            }
            else if (pOut <= 0)
            {
                throw new System.ArgumentOutOfRangeException(GREATER_THAN, "density");
            }
            else if (pOut >= pIn)
            {
                throw new System.ArgumentOutOfRangeException("Outlet pressure out must be less than inlet pressure", "pOut");
            }

            return OrificePlate(disCo, density, pipeDia, orificeDia, pIn - pOut);
        }

        /// <summary>
        /// Calculates the volumetric flow rate of an orifice plate
        /// </summary>
        /// <param name="disCo">Discharge coefficient (unitless)</param>
        /// <param name="density">Density of fluid going through orifice plate (kg/m3)</param>
        /// <param name="pipeDia">Inlet pipe diameter (m)</param>
        /// <param name="orificeDia">Orifice diameter (m)</param>
        /// <param name="deltaP">The DROP (p1 - p2) in pressure accross the orifice plate (Pa)</param>
        /// <returns>Volumetric flow rate (m3/s)</returns>
        public static double OrificePlate(
           double disCo, double density,
           double pipeDia, double orificeDia,
           double deltaP)
        {
            
            if (disCo <= 0 || disCo > 1)
            {
                throw new System.ArgumentOutOfRangeException("Parameter must greater than 0 and less than or equal to 1", "disCo");
            }
            else if (density <= 0)
            {
                throw new System.ArgumentOutOfRangeException(GREATER_THAN, "density");
            }
            else if (pipeDia <= 0)
            {
                throw new System.ArgumentOutOfRangeException(GREATER_THAN, "pipeDia");
            }
            else if (orificeDia <= 0)
            {
                throw new System.ArgumentOutOfRangeException(GREATER_THAN, "orificeDia");
            }
            else if (pipeDia <= orificeDia)
            {
                throw new System.ArgumentOutOfRangeException("Pipe Diameter must be greater than the orifice diameter", "orificeDia");
            }


            return disCo * PipeArea(pipeDia) *
                Math.Sqrt(
                    (2 * deltaP) /
                    (density *
                    (Math.Pow(PipeArea(pipeDia), 2) / Math.Pow(PipeArea(orificeDia), 2) - 1))
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
