using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.CalculationPage;
using CheApp.CheMath.Units;

namespace CheApp.Templates.CalculationPage
{
    public abstract class BasicPage : ContentPage
    {
        protected FieldBindData[] inputFieldData;
        protected NumericInputField[] inputFields;
        public Dictionary<int, NumericInputField> inputFieldsDic;

        protected FieldBindData[] outputFieldData;
        protected NumericOutputField[] outputFields;
        public Dictionary<int, NumericOutputField> outputFieldsDic;



        // TODO: make it so that more than one function can be used on a page 
        // ie switch between a direct input for density and using the ideal gas law to calculate density
        // TODO: make it so that the user can solve for any parameter in the function.

        /// <summary>
        /// Sets up a basic page which handles a single function
        /// </summary>
        internal void PageSetup()
        {
            inputFields = inputFieldData.Select(item => new NumericInputField(ref item)).ToArray();
            inputFieldsDic = inputFields.ToDictionary(item => item.ID, item => item);
            outputFields = outputFieldData.Select(item => new NumericOutputField(ref item)).ToArray();
            outputFieldsDic = outputFields.ToDictionary(item => item.ID, item => item);


            Grid grid = BasicGrids.SimpleGrid(inputFields.Length + inputFields.Length + 1, 1);

            for (int i = 0; i < inputFields.Length; i++)
            {
                grid.Children.Add(inputFields[i].GetGridSection(), 1, i + 1);
            }


            for (int i = 0; i < outputFields.Length; i++)
            {
                grid.Children.Add(outputFields[i].GetGridSection(), 1, i + 1 + inputFields.Length);
            }

            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = "Calculate!",
                Margin = 20
            };

            calculateBtn.Clicked += CalculateButtonClicked;

            grid.Children.Add(calculateBtn, 1, 1 + inputFieldData.Length + outputFieldData.Length);
            Grid.SetColumnSpan(calculateBtn, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                BackgroundColor = Color.WhiteSmoke
            };
        }

        /// <summary>
        /// Make sure parameters are valid and execute function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void CalculateButtonClicked(object sender, EventArgs e);
    }
}
