using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    class NumericOutputField : NumericFieldData
    {
        /// <summary>
        /// Handles results of internal calculations
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="unitType">If there is 2 elements, then the format will be treated as element 1 per element 2. </param>
        /// <param name="resultUnits">The units which the results of the internal calculations will be in</param>
        internal NumericOutputField(int id, string title, Type[] unitType, CheMath.Units.AbstractUnit[] resultUnits) : base(id, title, unitType, resultUnits) {}


        private Label _Label;
        /// <summary>
        /// Contains the user entered text
        /// </summary>
        private string Label
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

        // TODO: not good for temperature... Try and find a way to use the convertTo function instead
        /// <summary>
        /// Updates the label with the value of 
        /// </summary>
        /// <param name="finalResult">Result of internal calculation assumed to be in the same units as "resultUnits" specificed in the constructor</param>
        internal void SetFinalResult(double finalResult)
        {
            // TODO: fix this bad naming
            string unitName = Pickers[0].Items[Pickers[0].SelectedIndex];
            string unitName2 = Pickers[1].Items[Pickers[1].SelectedIndex];
            if (Pickers.Length == 1)
            {

                Label = ((CheMath.Units.StaticUnitProperties.AllUnits[UnitType[0]][unitName].ConversionFactor /
                    this.ConvertionFactor) * finalResult).ToString();
            }
            else
            {
                Label = (
                    ((CheMath.Units.StaticUnitProperties.AllUnits[UnitType[0]][unitName].ConversionFactor 
                    / CheMath.Units.StaticUnitProperties.AllUnits[UnitType[1]][unitName2].ConversionFactor) 
                    /
                    this.ConvertionFactor) * finalResult).ToString();
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
