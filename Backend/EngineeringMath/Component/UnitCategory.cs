using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    public class UnitCategory : NotifyPropertySortedList<string, Unit>, ISortedListItem<string>
    {
        protected UnitCategory() : base()
        {

        }

        /// <summary>
        /// Use this construtor when you want to make a unit category which is made up of other unit category
        /// NOTE: This construtor uses UnitCategoryCollection.AllUnits so make sure that all referenced units are already in the collection
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arr"></param>
        /// <param name="isUserDefined"></param>
        public UnitCategory(string name, CompositeUnitElement[] arr, bool isUserDefined = false) : this(name, isUserDefined)
        {
            arr.OrderBy(x => x.CategoryName).OrderBy(x => x.IsInverse);

            foreach (UnitSystem.UnitSystemBaseUnit sysUnit in Enum.GetValues(typeof(UnitSystem.UnitSystemBaseUnit)))
            {
                // None will not be represented
                if (sysUnit == UnitSystem.UnitSystemBaseUnit.None)
                    continue;

                string unitSymbol = string.Empty,
                    unitFullName = string.Empty;
                double convertToBaseFactor = 1,
                    convertFromBaseFactor = 1;
                foreach (CompositeUnitElement ele in arr)
                {
                    UnitCategory cat = UnitCategoryCollection.AllUnits.GetUnitCategoryByName(ele.CategoryName);
                    Unit baseUnit = cat.GetUnitByFullName(cat.GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemBaseUnit.SI)),
                        curUnit = cat.GetUnitByFullName(cat.GetUnitFullNameByUnitSystem(sysUnit));

                    double localConvertFromBaseFactor = cat.ConvertUnit(baseUnit.FullName, curUnit.FullName, 1),
                        localConvertToBaseFactor = cat.ConvertUnit(curUnit.FullName, baseUnit.FullName, 1);


                    string invStr = ele.IsInverse ? "\u207B" : string.Empty;

                    string pwStr = string.Empty,
                        localUnitName = string.Empty;
                    switch (ele.power)
                    {
                        case ToPowerOf.Two:
                            pwStr = "\u00B2";
                            localUnitName = string.Format(LibraryResources.UnitSquared, curUnit.FullName);
                            localConvertFromBaseFactor = Math.Pow(localConvertFromBaseFactor, 2);
                            localConvertToBaseFactor = Math.Pow(localConvertToBaseFactor, 2);
                            break;
                        case ToPowerOf.Three:
                            pwStr = "\u00B3";
                            localUnitName = string.Format(LibraryResources.UnitCubed, curUnit.FullName);
                            localConvertFromBaseFactor = Math.Pow(localConvertFromBaseFactor, 3);
                            localConvertToBaseFactor = Math.Pow(localConvertToBaseFactor, 3);
                            break;
                        case ToPowerOf.One:
                        default:
                            localUnitName = string.Copy(curUnit.FullName);
                            break;
                    }

                    if (ele.IsInverse)
                    {
                        convertFromBaseFactor /= localConvertFromBaseFactor;
                        convertToBaseFactor /= localConvertToBaseFactor;
                    }
                    else
                    {
                        convertFromBaseFactor *= localConvertFromBaseFactor;
                        convertToBaseFactor *= localConvertToBaseFactor;
                    }


                    localUnitName = ele.IsInverse ? string.Format(LibraryResources.UnitInversed, localUnitName) : localUnitName;
                    unitFullName = string.Concat(unitFullName, unitFullName.Equals(string.Empty) ? string.Empty : "-", localUnitName);
                    unitSymbol = string.Concat(unitSymbol, $"{curUnit.Symbol}{invStr}{pwStr}");
                }

                

                if(sysUnit == UnitSystem.UnitSystemBaseUnit.SI)
                {
                    this.Add(new Unit(unitFullName, unitSymbol, "", "", sysUnit, true, isUserDefined));
                }
                else
                {
                    this.Add(new Unit(
                        unitFullName,
                        unitSymbol, $"curUnit * {convertToBaseFactor}", $"baseUnit * {convertFromBaseFactor}", sysUnit, false, isUserDefined));
                }

            }


        }

        public UnitCategory(string name, bool isUserDefined = false)
        {
            Name = name;
            IsUserDefined = isUserDefined;
        }

        /// <summary>
        /// Converts current units into desired units
        /// </summary>
        /// <param name="curUnitFullName">The unit the value is currently in</param>
        /// <param name="desiredUnitFullName">The desired new units of the passed value</param>
        /// <param name="curUnitValue">current value</param>
        /// <returns></returns>
        public double ConvertUnit(string curUnitFullName, string desiredUnitFullName, double curUnitValue)
        {
            Unit curUnit = GetUnitByFullName(curUnitFullName),
                desiredUnit = GetUnitByFullName(desiredUnitFullName);

            return desiredUnit.ConvertFromBase(curUnit.ConvertToBase(curUnitValue));
        }

        public Unit GetUnitByFullName(string unitFullName)
        {
            IEnumerable<Unit> temp = from unit in this
                                       where unit.FullName.Equals(unitFullName)
                                       select unit;
            if (temp.Count() == 0)
            {
                throw new UnitNotFoundException(unitFullName);
            }
            else if (temp.Count() > 1)
            {
                throw new UnitsWithSameNameException(unitFullName);
            }
            return temp.ElementAt(0);
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }


        public bool IsUserDefined { get; set; }


        public string BaseUnitFullName
        {
            get
            {
                IEnumerable<string> temp = from unit in this
                       where unit.IsBaseUnit == true
                       select unit.FullName;
                if(temp.Count() == 0)
                {
                    throw new NoBaseUnitsException();
                }
                else if (temp.Count() > 1)
                {
                    throw new MoreThanOneBaseUnitException();
                }
                return temp.ElementAt(0);
            }
        }

        public string Key
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// Returns an empty string if "None" type is searched for since it is not required to have exactly one in a category
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public string GetUnitFullNameByUnitSystem(UnitSystem.UnitSystemBaseUnit system)
        {
            if(system == UnitSystem.UnitSystemBaseUnit.None)
            {
                return string.Empty;
            }

            IEnumerable<string> temp = from unit in this
                                       where unit.UnitSystem == system
                                       select unit.FullName;

            if (temp.Count() == 0)
            {
                throw new NoUnitSystemTypeException(system);
            }
            else if (temp.Count() > 1)
            {
                throw new MoreThanOneUnitSystemTypeException(system);
            }
            return temp.ElementAt(0);
        }



        public class NoBaseUnitsException : Exception { }
        public class MoreThanOneBaseUnitException : Exception { }

        public class NoUnitSystemTypeException : ArgumentException
        {
            public NoUnitSystemTypeException(UnitSystem.UnitSystemBaseUnit system) : base(system.ToString()) { }
        }
        public class MoreThanOneUnitSystemTypeException : ArgumentException
        {
            public MoreThanOneUnitSystemTypeException(UnitSystem.UnitSystemBaseUnit system) : base(system.ToString()) { }
        }

        public class UnitNotFoundException : ArgumentException
        {
            public UnitNotFoundException(string unitName) : base(unitName) { }
        }
        public class UnitsWithSameNameException : ArgumentException
        {
            public UnitsWithSameNameException(string unitName) : base(unitName) { }
        }

        /// <summary>
        /// Used to build a collection of units which have no name, but are made up of several units (ex: enthalpy)
        /// </summary>
        public struct CompositeUnitElement
        {
            /// <summary>
            /// Category Name of the type of unit to be used
            /// </summary>
            public string CategoryName;
            /// <summary>
            /// The power the predefined unit should be raise to
            /// </summary>
            public ToPowerOf power;
            /// <summary>
            /// true when this unit will be raise to the negative power
            /// </summary>
            public bool IsInverse;
        }

        /// <summary>
        /// Used only by the CompositeUnitElement
        /// </summary>
        public enum ToPowerOf
        {
            One,
            Two,
            Three
        }
    }
}
