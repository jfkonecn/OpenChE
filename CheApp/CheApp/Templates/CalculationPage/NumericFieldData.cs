using System;
using System.Collections.Generic;
using System.Text;
using CheApp.CheMath.Units;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Stores all of the data required to have a field which handles data inputs
    /// </summary>
    abstract internal class NumericFieldData
    {
        /// <summary>
        /// Stores all of the data required to have a field which handles data inputs
        /// </summary>
        /// <param name="title">Title of the field</param>
        /// <param name="unitType">The type of type being represented in the unit list</param>
        internal NumericFieldData(string title, Type unitType)
        {
            this.Title = title;
            this.UnitType = unitType;
        }

        /// <summary>
        /// The unit this field represents
        /// </summary>
        private Type UnitType { get; set; }


        protected List<string> ListOfUnitNames
        {
            get
            {
                if (UnitType == typeof(Mass))
                {
                    return new List<string>(Mass.StringToUnit.Keys);
                }
                else if (UnitType == typeof(Pressure))
                {
                    return new List<string>(Pressure.StringToUnit.Keys);
                }
                else if (UnitType == typeof(Temperature))
                {
                    return new List<string>(Temperature.StringToUnit.Keys);
                }
                else if (UnitType == typeof(Time))
                {
                    return new List<string>(Time.StringToUnit.Keys);
                }
                else if (UnitType == typeof(Volume))
                {
                    return new List<string>(Volume.StringToUnit.Keys);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        abstract internal Grid GetGridSection();




        /// <summary>
        /// Title of the field
        /// </summary>
        internal string Title { get; private set; }

        /// <summary>
        /// Converts between two different "unitType" units (the type is determined in the constructor)
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current unit of "value"</param>
        /// <param name="desiredUnit">Desired unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public double Convert(double value, string currentUnit, string desiredUnit)
        {

            if (UnitType == typeof(Mass))
            {
                return Mass.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if(UnitType == typeof(Pressure))
            {
                return Pressure.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Temperature))
            {
                return Temperature.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Time))
            {
                return Time.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Volume))
            {
                return Volume.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
