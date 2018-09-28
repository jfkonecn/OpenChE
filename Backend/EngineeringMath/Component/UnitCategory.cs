using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EngineeringMath.Resources;

namespace EngineeringMath.Component
{
    public class UnitCategory : Category<Unit>
    {
        protected UnitCategory() : base()
        {

        }

        /// <summary>
        /// Use this construtor when you want to make a unit category which is made up of other unit category
        /// NOTE: This construtor uses UnitCategoryCollection.AllUnits so make sure that all referenced units are already in the collection
        /// </summary>
        /// <param name="name">NOTE: assumes name is a property name in LibraryResources if this object is not user defined</param>
        /// <param name="arr"></param>
        /// <param name="isUserDefined"></param>
        public UnitCategory(string name, UnitCategoryElement[] arr, bool isUserDefined = false) : this(name, isUserDefined)
        {
            /*
             * aditi sharma 2
             * https://www.geeksforgeeks.org/combinations-from-n-arrays-picking-one-element-from-each-array/
             */
            int n = arr.Length;
            int[] indices = new int[n];
            for (int i = 0; i < n; i++)
                indices[i] = 0;

            while (true)
            {
                UnitComposite.UnitCompositeElement[] elements = new UnitComposite.UnitCompositeElement[n];
                for (int i = 0; i < n; i++)
                {
                    
                    elements[i] = new UnitComposite.UnitCompositeElement(arr[i].UnitCategory[indices[i]], arr[i].Power);
                }
                if(UnitComposite.TryBuildUnitComposite(out UnitComposite unitComp, elements))
                {
                    Children.Add(unitComp);
                }

                int next = n - 1;
                while (next >= 0 && (indices[next] + 1 >= arr[next].UnitCategory.Count))
                    next--;

                if (next < 0)
                    return;

                indices[next]++;

                for (int i = next + 1; i < n; i++)
                    indices[i] = 0;
            }
        }

        public UnitCategory(string name, bool isUserDefined = false) : base(name, isUserDefined)
        {

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
            Unit curUnit = GetItemByFullName(curUnitFullName),
                desiredUnit = GetItemByFullName(desiredUnitFullName);

            return desiredUnit.ConvertFromBase(curUnit.ConvertToBase(curUnitValue));
        }


        public string BaseUnitFullName
        {
            get
            {
                IEnumerable<string> temp = from unit in Children
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

        /// <summary>
        /// Returns an empty string if "NoSpecialSystem" type is searched for since it is not required to have exactly one in a category
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public string GetUnitFullNameByUnitSystem(UnitSystem system)
        {
            if(system.Equals(UnitSystem.Imperial.BaselineSystem) 
                || system.Equals(UnitSystem.Metric.BaselineSystem) 
                || system.Equals(UnitSystem.ImperialAndMetric.BaselineSystem))
            {
                return string.Empty;
            }

            IEnumerable<string> temp = from unit in Children
                                       where unit.UnitSystem.Equals(system) || unit.UnitSystem.Equals(UnitSystem.ImperialAndMetric.SI_USCS)
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

        public override string ToString()
        {
            return this.Name;
        }

        public double ConverterToSIUnit(string curUnitFullName, double curValue)
        {
            return ConvertUnit(curUnitFullName, GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI), curValue);
        }

        public double ConverterFromSIUnit(string desiredUnitFullName, double curValue)
        {
            return ConvertUnit(GetUnitFullNameByUnitSystem(UnitSystem.Metric.SI), desiredUnitFullName, curValue);
        }

        public bool IsBaseUnit { get; set; }


        public class NoBaseUnitsException : Exception { }
        public class MoreThanOneBaseUnitException : Exception { }

        public class NoUnitSystemTypeException : ArgumentException
        {
            public NoUnitSystemTypeException(UnitSystem system) : base(system.FullName) { }
        }
        public class MoreThanOneUnitSystemTypeException : ArgumentException
        {
            public MoreThanOneUnitSystemTypeException(UnitSystem system) : base(system.FullName) { }
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
        /// Category of units based on one or more units (ex: enthalpy or Area)
        /// </summary>
        public class UnitCategoryElement
        {

            public UnitCategoryElement()
            {
                FinishUp();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="categoryName">NOTE: assumes categoryName is a property name in LibraryResources if this object is not user defined</param>
            /// <param name="power"></param>
            /// <param name="isUserDefined"></param>
            public UnitCategoryElement(string categoryName, int power, bool isUserDefined = false)
            {
                CategoryName = categoryName;
                Power = power;
                IsUserDefined = isUserDefined;
                FinishUp();
            }

            public UnitCategoryElement(UnitCategory unitCategory, int power, bool isUserDefined = false) 
            {
                CategoryName = unitCategory.Name;
                Power = power;
                IsUserDefined = isUserDefined;
                UnitCategory = unitCategory;
            }

            private void FinishUp()
            {
                if (!IsUserDefined)
                    CategoryName = (string)typeof(LibraryResources).GetProperty(CategoryName).GetValue(null, null);
                UnitCategory = MathManager.AllUnits.GetCategoryByName(CategoryName);
            }
            /// <summary>
            /// Category Name of the type of unit to be used
            /// </summary>
            public string CategoryName { get; protected set; }
            /// <summary>
            /// The power the unit should be raise to
            /// </summary>
            public int Power { get; }

            public bool IsUserDefined { get; }
            [XmlIgnore]
            public UnitCategory UnitCategory { get; protected set; }
        }
    }
}
