using System;
using System.Collections.Generic;

using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    public class NumericOutputField : NumericFieldData
    {
        /// <summary>
        /// Handles results of internal calculations
        /// </summary>
        /// <param name="bindedObject">Object which is binded to the field</param>
        public NumericOutputField(ref FieldBindData bindedObject) : base(ref bindedObject) {}


        private Label _Label;



        /// <summary>
        /// Updates the label with the value of 
        /// </summary>
        /// <param name="finalResult">Result of internal calculation assumed to be in the same units as "resultUnits" specificed in the constructor</param>
        internal void SetFinalResult(double finalResult)
        {
            // WE ARE ASSUMING THAT A MAX OF 2 ELEMENTS WILL BE IN THE ARRAY
            BindedObject.LabelText = CheMath.Units.HelperFunctions.ConvertTo(
                finalResult,
                BindedObject.ConvertionUnits,
                SelectedStrings).ToString();


        }



        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal override Grid GetGridSection()
        {
            Grid grid = base.GetGridSection();

            this._Label = new Label
            {
                
            };

            // bind it up!
            _Label.SetBinding(Label.TextProperty, new Binding("LabelText"));
            _Label.BindingContext = BindedObject;

            
            // row 2
            grid.Children.Add(new Label { Text = "Result" }, 0, 1);

            // row 3
            grid.Children.Add(this._Label, 0, 2);

            return grid;
        }
    }
}
