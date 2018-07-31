using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class UnitCategory : NotifyPropertySortedList<string, Unit>, ISortedListItem<string>
    {
        protected UnitCategory() : base()
        {

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
    }
}
