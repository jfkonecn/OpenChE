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
    public class ParameterGrid : Grid
    {

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        /// <param name="para">The parameter this grid will bind to</param>
        public ParameterGrid(Page page, Parameter para, out ParameterStyle paraStyle)
        {
            paraStyle = new ParameterStyle(para);

            // create entry cell
            Label title = new Label
            {
                Text = para.Title
            };
            title.SetBinding(Label.StyleProperty, new Binding("TitleStyle"));
            title.BindingContext = paraStyle;


            // create the pickers
            Picker[] unitPickers = CreateUnitPickers(paraStyle);


            CreateParameterGrid(unitPickers.Length);


            this.SetBinding(Grid.StyleProperty, new Binding("GridStyle"));
            this.BindingContext = paraStyle;


            Label unitLb = new Label { Text = LibraryResources.Units };
            unitLb.SetBinding(Entry.StyleProperty, new Binding("StandardLabelStyle"));
            unitLb.BindingContext = paraStyle;

            // Create input/output cells

            Entry inputEntry = new Entry
            {

            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("EntryText"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("EntryIsEnabled"));
            inputEntry.SetBinding(Entry.StyleProperty, new Binding("EntryStyle"));
            inputEntry.BindingContext = paraStyle;




            Label valueTitleLb = new Label { Text = LibraryResources.Value };
            valueTitleLb.SetBinding(Entry.StyleProperty, new Binding("StandardLabelStyle"));
            valueTitleLb.BindingContext = paraStyle;


            Picker subFunctionPicker = new CalculationPicker<FunctionFactory.FactoryData>(paraStyle.SubFunctionPickersStyles);




            // dont care about storing style
            Button subFunctionBtn = new LinkToFunctionButton(page, para, out _);

            // row 1
            this.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, this.ColumnDefinitions.Count - 2);

            // row 2
            this.Children.Add(subFunctionPicker, 1, 2);
            Grid.SetColumnSpan(subFunctionPicker, this.ColumnDefinitions.Count - 2);

            // row 3
            this.Children.Add(valueTitleLb, 1, 3);
            this.Children.Add(unitLb, 2, 3);


            // row 4
            // Entry cell will be taken care of by the classes which inherit this class
            if (unitPickers.Length == 1)
            {
                this.Children.Add(unitPickers[0], 2, 4);

            }
            else
            {
                this.Children.Add(unitPickers[0], 2, 4);
                this.Children.Add(new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                }, 3, 4);
                this.Children.Add(unitPickers[1], 4, 4);
                Grid.SetColumnSpan(unitLb, 3);
            }
            this.Children.Add(inputEntry, 1, 4);


            // row 5
            this.Children.Add(subFunctionBtn, 1, 5);
            Grid.SetColumnSpan(subFunctionBtn, this.ColumnDefinitions.Count - 2);
        }



        /// <summary>
        /// Creates intended to be binded to a parameter object
        /// </summary>
        /// <param name="unitPickersLength"></param>
        /// <returns></returns>
        private void CreateParameterGrid(int unitPickersLength)
        {
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

            this.Style = Style = (Style)Application.Current.Resources["parameterStyle"];
            this.HorizontalOptions = LayoutOptions.FillAndExpand;
            this.RowDefinitions = new RowDefinitionCollection
                {
                new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                };

            this.ColumnDefinitions = colDefs;
        }

        /// <summary>
        /// creates a pickers for selecting units for the field
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private static Picker[] CreateUnitPickers(ParameterStyle paraStyle)
        {
            Picker[] allPickers = new Picker[paraStyle.UnitPickersStyles.Length];

            for (int i = 0; i < paraStyle.UnitPickersStyles.Length; i++)
            {
                allPickers[i] = new CalculationPicker<AbstractUnit>(paraStyle.UnitPickersStyles[i]);
            }

            return allPickers;
        }
    }
}
