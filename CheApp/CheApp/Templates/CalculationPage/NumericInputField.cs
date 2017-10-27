using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    internal class NumericInputField : NumericFieldData
    {
        /// <summary>
        /// Handles the numeric inputs from the user
        /// </summary>
        /// <param name="title"></param>
        /// <param name="unitType">If there is 2 elements, then the format will be treated as element 1 per element 2. </param>
        /// <param name="desiredUnits">The units which are desired for the purpose of performing internal calculations</param>
        internal NumericInputField(int id, string title, Type[] unitType, CheMath.Units.AbstractUnit[] desiredUnits) : base(id, title, unitType, desiredUnits) {  }


        private Entry _Entry;
        /// <summary>
        /// Contains the user entered text
        /// </summary>
        internal string EntryText
        {
            get
            {
                return _Entry.Text;
            }
            set
            {
                _Entry.Text = value;
            }
        }

        // TODO: not good for temperature... Try and find a way to use the convertTo function instead
        /// <summary>
        /// gets the user input in the desired units specified in constructor
        /// </summary>
        internal double GetUserInput()
        {
            // TODO: fix this bad naming
            string unitName = Pickers[0].Items[Pickers[0].SelectedIndex];
            string unitName2 = Pickers[1].Items[Pickers[1].SelectedIndex];
            if (Pickers.Length == 1)
            {

                return ((this.ConvertionFactor 
                    / CheMath.Units.StaticUnitProperties.AllUnits[UnitType[0]][unitName].ConversionFactor) 
                    * Convert.ToDouble(EntryText));
            }
            else
            {
                return ((this.ConvertionFactor / 
                    (CheMath.Units.StaticUnitProperties.AllUnits[UnitType[0]][unitName].ConversionFactor
                    / CheMath.Units.StaticUnitProperties.AllUnits[UnitType[1]][unitName2].ConversionFactor)
                    ) 
                    * Convert.ToDouble(EntryText));
            }
        }

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal override Grid GetGridSection()
        {
            Grid grid = base.GetGridSection();

            // create entry cell
            this._Entry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HeightRequest = 10
            };

            // row 2
            grid.Children.Add(new Label { Text = "Input" }, 0, 1);

            // row 3
            grid.Children.Add(this._Entry, 0, 2);





            return grid;
        }
    }
}
