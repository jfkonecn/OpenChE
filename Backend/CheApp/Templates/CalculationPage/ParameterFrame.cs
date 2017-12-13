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
            paraStyle = new ParameterStyle(para);

            // create entry cell
            Label title = new Label
            {
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };

            title.SetBinding(Label.TextProperty, new Binding("Title"));
            title.BindingContext = para;

            // create the pickers
            Picker[] unitPickers = CreateUnitPickers(para);


            Grid grid = CreateParameterGrid(unitPickers.Length);


            this.SetBinding(Frame.StyleProperty, new Binding("Style"));
            this.BindingContext = paraStyle;


            Label unitLb = new Label
            {
                Text = LibraryResources.Units,
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };

            // Create input/output cells

            Entry inputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric
            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("EntryText"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("EntryIsEnabled"));
            inputEntry.SetBinding(Entry.StyleProperty, new Binding("EntryStyle"));
            inputEntry.BindingContext = paraStyle;

            Label valueTitleLb = new Label
            {
                Text = LibraryResources.Value,
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };


            Picker subFunctionPicker = new CalculationPicker<FunctionFactory.FactoryData>(para.SubFunctionSelection);

            Label instructionLb = new Label
            {
                Text = string.Format(LibraryResources.ParameterValidRange, para.LowerLimit, para.UpperLimit)
            };


            Label errorLb = new Label
            {

            };
            errorLb.SetBinding(Label.TextProperty, new Binding("ErrorMessage"));
            errorLb.BindingContext = para;


            Button subFunctionBtn = new LinkToFunctionButton(page, para);

            // row 1
            grid.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count - 2);

            // row 2
            grid.Children.Add(subFunctionPicker, 1, 2);
            Grid.SetColumnSpan(subFunctionPicker, grid.ColumnDefinitions.Count - 2);

            // row 3
            grid.Children.Add(subFunctionBtn, 1, 3);
            Grid.SetColumnSpan(subFunctionBtn, grid.ColumnDefinitions.Count - 2);

            // row 4
            grid.Children.Add(valueTitleLb, 1, 4);
            grid.Children.Add(unitLb, 2, 4);


            // row 5
            // Entry cell will be taken care of by the classes which inherit this class
            if (unitPickers.Length == 1)
            {
                grid.Children.Add(unitPickers[0], 2, 5);

            }
            else
            {
                grid.Children.Add(unitPickers[0], 2, 5);
                grid.Children.Add(new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                }, 3, 5);
                grid.Children.Add(unitPickers[1], 4, 5);
                Grid.SetColumnSpan(unitLb, 3);
            }
            grid.Children.Add(inputEntry, 1, 5);

            // row 6
            grid.Children.Add(instructionLb, 1, 6);
            Grid.SetColumnSpan(instructionLb, grid.ColumnDefinitions.Count - 2);


            // row 7
            grid.Children.Add(errorLb, 1, 7);
            Grid.SetColumnSpan(errorLb, grid.ColumnDefinitions.Count - 2);

            this.Content = grid;
        }



        /// <summary>
        /// Creates intended to be binded to a parameter object
        /// </summary>
        /// <param name="unitPickersLength"></param>
        /// <returns></returns>
        private Grid CreateParameterGrid(int unitPickersLength)
        {
            Grid grid = new Grid();
            
            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


            colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });

            if (unitPickersLength == 1)
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

            colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });
            
            grid.HorizontalOptions = LayoutOptions.FillAndExpand;
            grid.RowDefinitions = new RowDefinitionCollection
                {
                new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                };

            grid.ColumnDefinitions = colDefs;

            return grid;
        }

        /// <summary>
        /// creates a pickers for selecting units for the field
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private static Picker[] CreateUnitPickers(Parameter para)
        {
            Picker[] allPickers = new Picker[para.UnitSelection.Length];

            for (int i = 0; i < para.UnitSelection.Length; i++)
            {
                allPickers[i] = new CalculationPicker<AbstractUnit>(para.UnitSelection[i]);
            }

            return allPickers;
        }
    }
}
