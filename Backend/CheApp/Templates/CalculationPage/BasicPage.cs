using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;
using System.Diagnostics;

namespace CheApp.Templates.CalculationPage
{
    public abstract class BasicPage : ContentPage
    {
        protected FieldBindData[] bindFieldData;
        protected SolveForBindData solveForBindData;
        private Picker solveForPicker;
        protected NumericFieldData[] fields;
        public Dictionary<int, NumericFieldData> fieldsDic;
       


        // TODO: make it so that more than one function can be used on a page 
        // ie switch between a direct input for density and using the ideal gas law to calculate density
        // TODO: make it so that the user can solve for any parameter in the function.

        /// <summary>
        /// Sets up a basic page which handles a single function
        /// </summary>
        internal void PageSetup()
        {
            fields = bindFieldData.Select(item => new NumericFieldData(ref item)).ToArray();
            fieldsDic = fields.ToDictionary(item => item.ID, item => item);


            Grid grid = BasicGrids.SimpleGrid(fields.Length + 2, 1);

            // setup solve for picker
            solveForBindData = new SolveForBindData();
            solveForPicker = new Picker();
            foreach (NumericFieldData obj in this.fieldsDic.Values)
            {
                solveForPicker.Items.Add(obj.Title);
            }

            solveForPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            solveForPicker.BindingContext = solveForBindData;

            solveForPicker.SelectedIndexChanged += SolveForPicker_SelectedIndexChanged;

            solveForPicker.Title = "Solve For:";

            grid.Children.Add(new Grid
            {
                Children =
                {
                    solveForPicker
                }
            }, 1, 1);

            for (int i = 0; i < fields.Length; i++)
            {
                grid.Children.Add(fields[i].GetGridSection(), 1, i + 2);
            }

            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = "Calculate!",
                Margin = 20
            };

            calculateBtn.Clicked += CalculateButtonClicked;

            grid.Children.Add(calculateBtn, 1, 2 + fields.Length);
            Grid.SetColumnSpan(calculateBtn, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                BackgroundColor = Color.WhiteSmoke
            };
        }

        private void SolveForPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedString = solveForPicker.Items[solveForBindData.SelectedIndex];
            Debug.WriteLine($"\"{selectedString}\" was selected");
        }

        /// <summary>
        /// Make sure parameters are valid and execute function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected abstract void CalculateButtonClicked(object sender, EventArgs e);
    }
}
