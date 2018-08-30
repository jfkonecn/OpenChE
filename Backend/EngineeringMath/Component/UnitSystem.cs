using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    public class UnitSystem
    {

        public static class Metric
        {
            public static readonly string FullName = LibraryResources.MetricSystem;
            public static readonly UnitSystem
                /// <summary>
                /// International System of Units 
                /// </summary>
                SI = new UnitSystem()
                {
                    FullName = LibraryResources.SIFullName,
                    AbbrevName = LibraryResources.SIAbbrev,
                    SystemTagName = nameof(Metric),
                    FindCommonUnits = (x) =>
                    {
                        if (x.SystemTagName.Equals(BaselineSystem.SystemTagName))
                        {
                            return x;
                        }
                        return null;
                    }
                },
                BaselineSystem = new UnitSystem()
                {
                    FullName = string.Empty,
                    AbbrevName = string.Empty,
                    SystemTagName = nameof(Metric),
                    FindCommonUnits = (x) =>
                    {
                        if (x.SystemTagName.Equals(BaselineSystem.SystemTagName))
                        {
                            return BaselineSystem;
                        }
                        return null;
                    }
                };
        }

        public static class Imperial
        {
            public static readonly string FullName = LibraryResources.ImperialSystem;
            public static readonly UnitSystem
                /// <summary>
                /// United States customary system
                /// </summary>
                USCS = new UnitSystem()
                {
                    FullName = LibraryResources.USCSFullName,
                    AbbrevName = LibraryResources.USCSAbbrev,
                    SystemTagName = nameof(Imperial),
                    FindCommonUnits = (x) =>
                    {
                        if (x.SystemTagName.Equals(BaselineSystem.SystemTagName))
                        {
                            return x;
                        }
                        return null;
                    }
                },
                BaselineSystem = new UnitSystem()
                {
                    FullName = string.Empty,
                    AbbrevName = string.Empty,
                    SystemTagName = nameof(Imperial),
                    FindCommonUnits = (x) =>
                    {
                        if (x.SystemTagName.Equals(BaselineSystem.SystemTagName))
                        {
                            return BaselineSystem;
                        }
                        return null;
                    }
                };
        }

        public static class ImperialAndMetric
        {
            public static readonly string FullName = $"{Metric.FullName}/{Imperial.FullName}";
            public static readonly UnitSystem
                /// <summary>
                /// United States customary system
                /// </summary>
                SI_USCS = new UnitSystem()
                {
                    FullName = $"{Metric.SI.FullName}/{Imperial.USCS.FullName}",
                    AbbrevName = $"{Metric.SI.AbbrevName}/{Imperial.USCS.AbbrevName}",
                    SystemTagName = nameof(ImperialAndMetric),
                    FindCommonUnits = (x) =>
                    {
                        if (x.Equals(Metric.SI) ||
                        x.Equals(Imperial.USCS) ||
                        x.Equals(SI_USCS) ||
                        x.Equals(BaselineSystem))
                        {
                            return x;
                        }
                        return null;
                    }
                },
                BaselineSystem = new UnitSystem()
                {
                    FullName = string.Empty,
                    AbbrevName = string.Empty,
                    SystemTagName = nameof(ImperialAndMetric),
                    FindCommonUnits = (x) =>
                    {
                        return ExtractBaselineSystem(x);
                    } 
                };
        }

        /// <summary>
        /// Finds and returns the baseline system for the passed unit system. Ex: The SI unit system would return the metric.BaselineSystem.
        /// Passing a basline system will result in the same object being returned
        /// </summary>
        /// <param name="unitSystem"></param>
        /// <returns>null if the system cannot be found</returns>
        private static UnitSystem ExtractBaselineSystem(UnitSystem unitSystem)
        {
            string tagName = unitSystem.SystemTagName;
            if (tagName.Equals(Imperial.BaselineSystem.SystemTagName))
            {
                return Imperial.BaselineSystem;
            }
            else if (tagName.Equals(Metric.BaselineSystem.SystemTagName))
            {
                return Metric.BaselineSystem;
            }
            else if (tagName.Equals(ImperialAndMetric.BaselineSystem.SystemTagName))
            {
                return ImperialAndMetric.BaselineSystem;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Compares two unit system, then determines the common unit system they share.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="result">null if nothing is in common</param>
        /// <returns>true if a co</returns>
        public static bool TryToFindCommonUnitSystem(UnitSystem left, UnitSystem right, out UnitSystem result)
        {
            result = left.FindCommonUnits(right);
            if (result == null)
                result = right.FindCommonUnits(left);
            return result != null;
        }


        public string FullName { get; private set; }

        public string AbbrevName { get; private set; }

        /// <summary>
        /// Used internally to find groups of systems i.e. imperial or metric
        /// </summary>
        internal string SystemTagName { get; private set; }


        private Func<UnitSystem, UnitSystem> FindCommonUnits { get; set; }

        public class NoCommonUnitSystemFoundException : ArgumentException { }
    }
}
