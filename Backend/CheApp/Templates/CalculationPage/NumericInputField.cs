using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    public class NumericInputField : NumericFieldData
    {
        /// <summary>
        /// Handles the numeric inputs from the user
        /// </summary>
        /// <param name="bindedObject">Object which is binded to the field</param>
        public NumericInputField(ref FieldBindData bindedObject) : base(ref bindedObject) {  }


        private Entry _Entry;
        /// <summary>
        /// Contains the user entered text
        /// </summary>
        public string EntryText
        {
            get
            {
                return BindedObject.LabelText;
            }
            set
            {
                BindedObject.LabelText = value;
            }
        }



        /// <summary>
        /// gets the user input in the desired units specified in constructor
        /// </summary>
        internal double GetUserInput()
        {
            try
            {
                double temp = EngineeringMath.Units.HelperFunctions.ConvertFrom(
                Convert.ToDouble(EntryText),
                BindedObject.ConvertionUnits,
                SelectedStrings);
                BindedObject.BackgroundColor = Color.LightGreen;
                return temp;
            }
            catch (System.FormatException)
            {
                BindedObject.BackgroundColor = Color.PaleVioletRed;
                throw new FormatException();
            }

            // WE ARE ASSUMING THAT A MAX OF 2 ELEMENTS WILL BE IN THE ARRAY
            
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

            // bind it up!
            _Entry.SetBinding(Entry.TextProperty, new Binding("LabelText"));
            _Entry.BindingContext = BindedObject;

            grid.SetBinding(Grid.BackgroundColorProperty, new Binding("BackgroundColor"));
            grid.BindingContext = BindedObject;

            // row 2
            grid.Children.Add(new Label { Text = "Input" }, 1, 2);

            // row 3
            grid.Children.Add(this._Entry, 1, 3);


            return grid;
        }
    }
}
