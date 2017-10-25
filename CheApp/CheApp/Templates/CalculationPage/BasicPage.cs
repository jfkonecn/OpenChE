using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    internal class BasicPage
    {
        internal static void BasicInputPage(ContentPage contentPage, NumericInputField[] inputFieldData, NumericOutputField outputFieldData)
        {

            const int ROW_MARIGN = 20;
            const int COL_MARIGN = 20;
            const int ROW_HEIGHT = 50;



            StackLayout stackLayout = new StackLayout
            {

            };

            foreach (NumericFieldData field in inputFieldData)
            {
                stackLayout.Children.Add(field.GetGridSection());
            }

            stackLayout.Children.Add(outputFieldData.GetGridSection());



            contentPage.Content = new ScrollView
            {
                Content = stackLayout

            };
        }
    }
}
