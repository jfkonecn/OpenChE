using System;
using System.Collections.Generic;
using System.Text;
using CheApp.CheMath.Units;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Stores all of the data required to have a field which handles data inputs
    /// </summary>
    abstract internal class NumericFieldData
    {
        /// <summary>
        /// Stores all of the data required to have a field which handles data inputs
        /// </summary>
        /// <param name="title">Title of the field</param>
        /// <param name="unitType">The types of units being represented in the unit list</param>
        internal NumericFieldData(string title, Type[] unitType)
        {
            if (unitType.Length > 2)
            {
                throw new Exception("unit type is out of range");
            }
            this.Title = title;
            this.UnitType = unitType;
        }

        /// <summary>
        /// The unit this field represents
        /// </summary>
        private Type[] UnitType { get; set; }

        /// <summary>
        /// Stores reference to all pickers
        /// </summary>
        internal Picker[] Pickers { private set; get; }

        /// <summary>
        /// creates a pickers for the input field
        /// </summary>
        /// <returns></returns>
        private Picker[] CreatePickers()
        {
            Picker[] allPickers = new Picker[UnitType.Length];

            for(int i = 0; i < UnitType.Length; i++)
            {
                // create the picker
                allPickers[i] = new Picker
                {


                };
                foreach (string str in this.ListOfUnitNames(UnitType[i]))
                {
                    allPickers[i].Items.Add(str);
                }
                allPickers[i].SelectedIndex = 0;
            }

            return allPickers;
        }

        private List<string> ListOfUnitNames(Type unitType)
        {
            if (unitType == typeof(Mass))
            {
                return new List<string>(Mass.StringToUnit.Keys);
            }
            else if (unitType == typeof(Pressure))
            {
                return new List<string>(Pressure.StringToUnit.Keys);
            }
            else if (unitType == typeof(Temperature))
            {
                return new List<string>(Temperature.StringToUnit.Keys);
            }
            else if (unitType == typeof(Time))
            {
                return new List<string>(Time.StringToUnit.Keys);
            }
            else if (unitType == typeof(Volume))
            {
                return new List<string>(Volume.StringToUnit.Keys);
            }
            else if (unitType == typeof(Unitless))
            {
                return new List<string>(Unitless.StringToUnit.Keys);
            }
            else if (unitType == typeof(Density))
            {
                return new List<string>(Density.StringToUnit.Keys);
            }
            else if (unitType == typeof(Length))
            {
                return new List<string>(Length.StringToUnit.Keys);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        virtual internal Grid GetGridSection()
        {
            // create entry cell
            Label title = new Label
            {
                Text = this.Title,
                HorizontalTextAlignment = TextAlignment.Center
            };

            
            // create the pickers
            this.Pickers = CreatePickers();

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();

            

            
            if (this.Pickers.Length == 1)
            {
                // for input/ouput cell
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // for unit pickers
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            else
            {

                // for input/ouput cell
                colDefs.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                // for unit pickers
                colDefs.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                colDefs.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            }



            Grid grid = new Grid
            {
                Margin = 20,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = colDefs
            };




            // row 1
            grid.Children.Add(title, 0, 0);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count);



            Label unitLb = new Label { Text = "Units" };
            grid.Children.Add(unitLb, 1, 1);

            // row 3
            // Entry cell will be taken care of by the classes which inherit this class
            if (this.Pickers.Length == 1)
            {
                grid.Children.Add(this.Pickers[0], 1, 2);
                
            }
            else
            {
                grid.Children.Add(this.Pickers[0], 1, 2);
                grid.Children.Add(new Label
                {
                    Text = "Per",
                    HorizontalTextAlignment = TextAlignment.Center
                }, 2, 2);
                grid.Children.Add(this.Pickers[1], 3, 2);
                Grid.SetColumnSpan(unitLb, 3);
            }





            return grid;
        }




        /// <summary>
        /// Title of the field
        /// </summary>
        internal string Title { get; private set; }

        /// <summary>
        /// Converts between two different "unitType" units (the type is determined in the constructor)
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="currentUnit">Current unit of "value"</param>
        /// <param name="desiredUnit">Desired unit of "value"</param>
        /// <returns>The value in the "desired units"</returns>
        public double Convert(double value, string currentUnit, string desiredUnit)
        {
            /*
            if (UnitType == typeof(Mass))
            {
                return Mass.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if(UnitType == typeof(Pressure))
            {
                return Pressure.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Temperature))
            {
                return Temperature.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Time))
            {
                return Time.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else if (UnitType == typeof(Volume))
            {
                return Volume.StringToUnit[currentUnit].ConvertTo(value, desiredUnit);
            }
            else
            {
                throw new NotImplementedException();
            }
            */
            throw new NotImplementedException();
        }
    }
}
