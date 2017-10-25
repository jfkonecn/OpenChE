using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    class NumericOutputField : NumericFieldData
    {
        internal NumericOutputField(string title, Type unitType) : base(title, unitType){}


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
            private set
            {
                _Label.Text = value;
            }
        }



        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal override Grid GetGridSection()
        {
            const int ROW_MARIGN = 20;
            const int COL_MARIGN = 20;
            const int ROW_HEIGHT = 50;


            Grid grid = new Grid
            {
                
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(ROW_MARIGN, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(ROW_MARIGN, GridUnitType.Absolute) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(COL_MARIGN, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(COL_MARIGN, GridUnitType.Absolute)  }
                }
            };

            
            

            // create the picker
            Picker picker = new Picker
            {


            };
            foreach (string str in this.ListOfUnitNames)
            {
                picker.Items.Add(str);
            }


            // create entry cell
            Label title = new Label
            {
                Text = this.Title,
                HorizontalTextAlignment = TextAlignment.Center
            };


            // create entry cell
            this._Label = new Label
            {
                Text = "0.0",
                VerticalTextAlignment = TextAlignment.Center
            };

            Button button = new Button
            {
                Text = "Calculate!"
            };

            // row 1
            grid.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, 2);

            // row 2
            grid.Children.Add(new Label { Text = "Result" }, 1, 2);
            grid.Children.Add(new Label { Text = "Units" }, 2, 2);

            // row 3
            grid.Children.Add(this._Label, 1, 3);
            grid.Children.Add(picker, 2, 3);

            // row 4
            grid.Children.Add(button, 1, 4);
            Grid.SetColumnSpan(button, 2);

            return grid;
        }
    }
}
