using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    internal class NumericInputField : NumericFieldData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="unitType">If there is 2 elements, then the format will be treated as element 1 per element 2. </param>
        internal NumericInputField(string title, Type[] unitType) : base(title, unitType){  }


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
