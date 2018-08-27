using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class Unit : NotifyPropertyChangedExtension, IChildItem<Category<Unit>>, ICategoryItem, ISettingOption
    {

        protected Unit() : base()
        {
            
        }

        /// <summary>
        /// Internally used constructor
        /// </summary>
        /// <param name="fullName">as in meters cubed not as m3</param>
        /// <param name="symbol">as in m3 not meters cubed</param>
        /// <param name="convertToBaseEquation">        
        /// Equation which converts from this unit into the base unit
        /// Note: Only valid variable inside string is "curUnit"(without quotes)
        /// </param>
        /// <param name="convertFromBaseEquation">
        /// Equation which converts from base unit to this unit.
        /// Note: Only valid variable inside string is "baseUnit"(without quotes)
        /// </param>
        /// <param name="unitSystem"></param>
        /// <param name="isBaseUnit"></param>
        /// <param name="isUserDefined"></param>
        /// <param name="absoluteScaleUnit">true if unit is on an absolute scale (i.e. meters) as opposed to Celsius</param>
        private Unit(string fullName, string symbol,
            string convertToBaseEquation,
            string convertFromBaseEquation,
            UnitSystem unitSystem,
            bool isBaseUnit,
            bool isUserDefined,
            bool absoluteScaleUnit) : this()
        {
            if (unitSystem == Component.UnitSystem.Metric.SI &&
                !absoluteScaleUnit)
            {
                throw new UnitSIUnitNotOnAbsoluteScaleException(fullName);
            }
            FullName = fullName;
            Symbol = symbol;
            ConvertToBaseEquation = convertToBaseEquation;
            ConvertFromBaseEquation = convertFromBaseEquation;
            UnitSystem = unitSystem;
            IsBaseUnit = isBaseUnit;
            IsUserDefined = isUserDefined;
            AbsoluteScaleUnit = absoluteScaleUnit;
        }



        /// <summary>
        /// Non-Absolute Scale unit constructor (non-base unit)
        /// </summary>
        /// <param name="fullName">as in meters cubed not as m3</param>
        /// <param name="symbol">as in m3 not meters cubed</param>
        /// <param name="convertToBaseEquation">        
        /// Equation which converts from this unit into the base unit
        /// Note: Only valid variable inside string is "curUnit"(without quotes)
        /// </param>
        /// <param name="convertFromBaseEquation">
        /// Equation which converts from base unit to this unit.
        /// Note: Only valid variable inside string is "baseUnit"(without quotes)
        /// </param>
        /// <param name="unitSystem"></param>
        /// <param name="isUserDefined"></param>
        public Unit(string fullName, string symbol,
            string convertToBaseEquation, 
            string convertFromBaseEquation, 
            UnitSystem unitSystem,
            bool isUserDefined = false) :
            this(fullName, symbol, convertToBaseEquation,
                convertFromBaseEquation, unitSystem, false, isUserDefined, false)
        {

        }


        /// <summary>
        /// Base Unit constructor 
        /// </summary>
        /// <param name="fullName">as in meters cubed not as m3</param>
        /// <param name="symbol">as in m3 not meters cubed</param>
        /// <param name="unitSystem"></param>
        /// <param name="isUserDefined"></param>
        /// <param name="absoluteScale">true if unit is on an absolute scale (i.e. meters) as opposed to Celsius</param>
        public Unit(string fullName, string symbol,
            UnitSystem unitSystem,
            bool isUserDefined = false,
            bool absoluteScale = true) :
            this(fullName, symbol, string.Empty,
                string.Empty, unitSystem, true, isUserDefined, absoluteScale)
        {

        }

        /// <summary>
        /// Absolute scale constructor (non-base unit)
        /// </summary>
        /// <param name="fullName">as in meters cubed not as m3</param>
        /// <param name="symbol">as in m3 not meters cubed</param>
        /// <param name="convertToBaseFactor"></param>
        /// <param name="unitSystem"></param>
        /// <param name="isBaseUnit"></param>
        /// <param name="isUserDefined"></param>
        public Unit(string fullName, string symbol,
            double convertToBaseFactor,
            UnitSystem unitSystem,            
            bool isUserDefined = false) : 
            this(fullName, symbol, $"${CurUnitVar} * {convertToBaseFactor}", 
                $"${BaseUnitVar} / {convertToBaseFactor}", unitSystem, false, isUserDefined, true)
        {

        }


        [XmlIgnore]
        private Category<Unit> ParentObject { get; set; }


        public Category<Unit> Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }


        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set
            {
                _FullName = value;
                OnPropertyChanged();
            }
        }

        private string _Symbol;
        public string Symbol
        {
            get { return _Symbol; }
            set
            {
                _Symbol = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The string which is replaced in a ConvertToBaseEquation with the value in this object's units
        /// </summary>
        public static readonly string CurUnitVar = "curUnit";

        /// <summary>
        /// The string which is replaced in a ConvertFromBaseEquation with the value in the base unit object's units
        /// </summary>
        public static readonly string BaseUnitVar = "baseUnit";




        public double ConvertToBase(double curUnit)
        {
            if (IsBaseUnit)
                return curUnit;
            return Equation.Evaluate(ConvertToBaseEquation, new KeyValuePair<string, double>(CurUnitVar, curUnit));
        }

        public double ConvertFromBase(double baseUnit)
        {
            if (IsBaseUnit)
                return baseUnit;
            return Equation.Evaluate(ConvertFromBaseEquation, new KeyValuePair<string, double>(BaseUnitVar, baseUnit));
        }


        public string ConvertToBaseEquation { get; set; }


        public string ConvertFromBaseEquation { get; set; }

        /// <summary>
        /// True if this unit is based on an absolute scale
        /// </summary>
        public bool AbsoluteScaleUnit { get; set; }


        public UnitSystem UnitSystem { get; set; }
        public bool IsBaseUnit { get; set; }
        public bool IsUserDefined { get; set; }
        Category<Unit> IChildItem<Category<Unit>>.Parent { get => Parent; set => Parent = value; }

        string IChildItem<Category<Unit>>.Key => FullName;

        string ISettingOption.Name => FullName;

        public override string ToString()
        {
            return this.FullName;
        }


        public class UnitSIUnitNotOnAbsoluteScaleException : ArgumentException
        {
            public UnitSIUnitNotOnAbsoluteScaleException(string unitFullName) : base(unitFullName) { }
        }
    }
}
