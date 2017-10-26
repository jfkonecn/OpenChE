using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    class NumericOutputField : NumericFieldData
    {
        internal NumericOutputField(string title, Type[] unitType) : base(title, unitType){}


        private Label _Label;
        /// <summary>
        /// Contains the user entered text
        /// </summary>
        internal string Label
        {
            get
            {
                return _Label.Text;
            }
            set
            {
                _Label.Text = value;
            }
        }



        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal override Grid GetGridSection()
        {
            Grid grid = base.GetGridSection();




            this._Label = new Label
            {
                Text = "0.0"
            };

            // row 2
            grid.Children.Add(new Label { Text = "Result" }, 0, 1);

            // row 3
            grid.Children.Add(this._Label, 0, 2);



            return grid;
        }
    }
}
