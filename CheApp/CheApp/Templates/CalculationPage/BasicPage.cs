using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    internal class BasicPage
    {


        /// <summary>
        /// Sets up a basic page which handles a single function
        /// </summary>
        /// <param name="contentPage">The page which will be performing the function</param>
        /// <param name="inputFieldData">Objects which help build input fields</param>
        /// <param name="outputFieldData">Objects which help build output fields</param>
        /// <param name="calFun">Responsible for performing the calculations</param>
        internal static void BasicInputPage(
            ContentPage contentPage, 
            NumericInputField[] inputFieldData, 
            NumericOutputField[] outputFieldData,
            EventHandler calFun)
        {
            
            Grid grid = BasicGrids.SimpleGrid(inputFieldData.Length + outputFieldData.Length + 1, 1);

            for (int i = 0; i < inputFieldData.Length; i++)
            {
                grid.Children.Add(inputFieldData[i].GetGridSection(), 1, i + 1);
            }


            for (int i = 0; i < outputFieldData.Length; i++)
            {
                grid.Children.Add(outputFieldData[i].GetGridSection(), 1, i + 1 + inputFieldData.Length);
            }

            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = "Calculate!",
                Margin = 20
            };

            calculateBtn.Clicked += calFun;

            grid.Children.Add(calculateBtn, 1, 1 + inputFieldData.Length + outputFieldData.Length);
            Grid.SetColumnSpan(calculateBtn, grid.ColumnDefinitions.Count - 2);

            // finish up
            contentPage.Content = new ScrollView
            {
                Content = grid
            };
        }
    }
}
