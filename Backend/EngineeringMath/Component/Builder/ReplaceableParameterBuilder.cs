using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.Builder
{
    public static class ReplaceableParameterBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="para">Parameter which can be replaced</param>
        /// <returns></returns>
        public static ReplaceableParameter AreaParameter(Parameter para)
        {
            return new ReplaceableParameter(
                para,
                new FunctionLeaf[] 
                {
                    new FunctionLeaf("$dia ^ 2 * PI / 4", $"{ReplaceableParameter.ReplacingStr}",
                    string.Format(LibraryResources.SolveUsingFunction, LibraryResources.AreaOfCircle))
                    {
                        Parameters =
                        {
                            new SIUnitParameter($"{para.DisplayName}: {LibraryResources.Diameter}", "dia", LibraryResources.Length, 0)
                        }
                    },
                    new FunctionLeaf($"{ReplaceableParameter.ReplacingStr} ^ 2 * PI / 4", "dia", 
                    string.Format(LibraryResources.SolveUsingFunction, LibraryResources.AreaOfCircle))
                    {
                        Parameters =
                        {
                            new SIUnitParameter($"{para.DisplayName}: {LibraryResources.Diameter}", "dia", LibraryResources.Length, 0)
                        }
                    }
                });
        }
    }
}
