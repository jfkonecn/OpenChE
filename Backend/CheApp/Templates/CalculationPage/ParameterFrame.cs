using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Calculations;
using CheApp.Templates.ObjectStyleBinders;
using EngineeringMath.Resources;
using EngineeringMath.Units;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// A grid representation of a parameter this grid is binded to
    /// </summary>
    public class ParameterFrame : Frame
    {

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        /// <param name="para">The parameter this frame will bind to</param>
        public ParameterFrame(Page page, Parameter para, out ParameterStyle paraStyle)
        {
            Grid grid = CreateParameterGrid(para.UnitSelection.Length);

            // Bind this frame to a style
            paraStyle = new ParameterStyle(para);
            this.SetBinding(Frame.StyleProperty, new Binding("Style"));
            this.BindingContext = paraStyle;

            // row 1
            grid.Children.Add(CreateTitleLabel(para), 1, 1);

            // row 2
            grid.Children.Add(CreateSubFunctionGrid(page, para), 1, 2);

            // row 3
            grid.Children.Add(CreateEntryGrid(para, paraStyle), 1, 3);

            // row 4
            grid.Children.Add(CreateUnitGrid(para), 1, 4);

            // row 5
            grid.Children.Add(CreateErrorLabel(para), 1, 5);

            this.Content = grid;
        }



        /// <summary>
        /// Creates intended to be binded to a parameter object
        /// </summary>
        /// <param name="unitPickersLength"></param>
        /// <returns></returns>
        private Grid CreateParameterGrid(int unitPickersLength)
        {
            int rowMargin = (int)Application.Current.Resources["standardRowMargin"];
            int columnMargin = (int)Application.Current.Resources["standardColumnMargin"];

            Grid grid = new Grid();
            
            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });

            if (unitPickersLength == 1)
            {
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                // for input/ouput cell
                colDefs.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
            }

            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });
            
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            grid.RowDefinitions = new RowDefinitionCollection
                {
                new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) },
                    // Title
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    // SubFunction Grid
                    new RowDefinition { Height = new GridLength(3, GridUnitType.Star) },
                    // Input Value Grid Row
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    // Unit Grid Row
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Star) },
                    // Error Label
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) }
                };

            grid.ColumnDefinitions = colDefs;

            return grid;
        }

        /// <summary>
        /// Creates the title for this parameter frame
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Label CreateTitleLabel(Parameter para)
        {
            Label title = new Label
            {
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };

            title.SetBinding(Label.TextProperty, new Binding("Title"));
            title.BindingContext = para;

            return title;
        }

        /// <summary>
        /// Create a grid which contains a picker and button to allow the user to substitute the current function with 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateSubFunctionGrid(Page page, Parameter para)
        {
            Grid subFunctionGrid = BasicGrids.SimpleGrid(2, 1, 0, 0);
            Picker subFunctionPicker = new CalculationPicker<FunctionFactory.FactoryData>(para.SubFunctionSelection);
            Button subFunctionBtn = new LinkToFunctionButton(page, para);
            subFunctionGrid.Children.Add(subFunctionPicker, 1, 1);
            subFunctionGrid.Children.Add(subFunctionBtn, 1, 2);
            return subFunctionGrid;
        }

        /// <summary>
        /// Creates a grid which contains an entry and a label for the user to input the value of this parameter
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateEntryGrid(Parameter para, ParameterStyle paraStyle)
        {
            Grid inputGrid = BasicGrids.SimpleGrid(2, 1, 0, 0);


            Label valueTitleLb = new Label
            {
                Text = LibraryResources.Value,
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };
            inputGrid.Children.Add(valueTitleLb, 1, 1);

            // Create input/output cells
            Entry inputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                Placeholder = string.Format(LibraryResources.ParameterValidRange, para.LowerLimit, para.UpperLimit)
            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("EntryText"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("EntryIsEnabled"));
            inputEntry.SetBinding(Entry.StyleProperty, new Binding("EntryStyle"));
            inputEntry.BindingContext = paraStyle;
            inputGrid.Children.Add(inputEntry, 1, 2);

            return inputGrid;
        }


        /// <summary>
        /// Creates a grid which contains the labels and pickers for this parameter's units
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateUnitGrid(Parameter para)
        {
            Picker[] unitPickers = new Picker[para.UnitSelection.Length];

            for (int i = 0; i < para.UnitSelection.Length; i++)
            {
                unitPickers[i] = new CalculationPicker<AbstractUnit>(para.UnitSelection[i]);
            }

            // total columns in this grid
            int totalColumns = 1;

            // should only be two cases for unitPickers.Length 1 and 2...
            if(unitPickers.Length == 2)
            {
                totalColumns = 3;
            }
            else if (unitPickers.Length > 2)
            {
                throw new Exception("Unexpected Unit Picker Length!");
            }


            Grid unitGrid = BasicGrids.SimpleGrid(2, totalColumns, 0, 0);

            // add the unit label to grid
            Label unitLb = new Label
            {
                Text = LibraryResources.Units,
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };
            unitGrid.Children.Add(unitLb, 1, 1);
            Grid.SetColumnSpan(unitLb, totalColumns);


            // add pickers to grid
            unitGrid.Children.Add(unitPickers[0], 1, 2);

            if (unitPickers.Length == 2)
            {               
                unitGrid.Children.Add(new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                }, 2, 2);
                unitGrid.Children.Add(unitPickers[1], 3, 2);
            }

            return unitGrid;
        }

        /// <summary>
        /// Creates the error text label
        /// </summary>
        /// <returns></returns>
        private Label CreateErrorLabel(Parameter para)
        {
            Label errorLb = new Label
            {

            };
            errorLb.SetBinding(Label.TextProperty, new Binding("ErrorMessage"));
            errorLb.BindingContext = para;

            return errorLb;
        }
    }
}
