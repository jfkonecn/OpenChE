using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class Unit : NotifyPropertyChangedExtension
    {

        protected Unit() : base()
        {

        }
        /// <summary>
        /// 
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
        public Unit(string fullName, string symbol,
            string convertToBaseEquation, 
            string convertFromBaseEquation, 
            UnitSystem.UnitSystemType unitSystem, 
            bool isBaseUnit = false,
            bool isUserDefined = false)
        {
            FullName = fullName;
            Symbol = symbol;
            ConvertToBaseEquation = convertToBaseEquation;
            ConvertFromBaseEquation = convertFromBaseEquation;
            UnitSystem = unitSystem;
            IsBaseUnit = isBaseUnit;
            IsUserDefined = isUserDefined;
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

        public double ConvertToBase(double curUnit)
        {
            if (IsBaseUnit)
                return curUnit;
            _Converter.EquationExpression = ConvertToBaseEquation.Replace(nameof(curUnit), curUnit.ToString());
            return _Converter.Evaluate();
        }

        public double ConvertFromBase(double baseUnit)
        {
            if (IsBaseUnit)
                return baseUnit;
            _Converter.EquationExpression = ConvertFromBaseEquation.Replace(nameof(baseUnit), baseUnit.ToString());
            return _Converter.Evaluate();
        }


        public string ConvertToBaseEquation { get; set; }


        public string ConvertFromBaseEquation { get; set; }


        private readonly Equation _Converter = new Equation("42");

        public UnitSystem.UnitSystemType UnitSystem { get; set; }
        public bool IsBaseUnit { get; set; }
        public bool IsUserDefined { get; set; }
    }
}
