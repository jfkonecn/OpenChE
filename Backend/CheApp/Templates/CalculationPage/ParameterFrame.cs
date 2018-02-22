using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Calculations;
using EngineeringMath.Resources;
using EngineeringMath.Units;
using EngineeringMath.Calculations.Components.Parameter;

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
        public ParameterFrame(SimpleParameter para)
        {
            Grid grid = CreateParameterGrid(para);
            FinishUp(para, grid);
        }

        private void FinishUp(SimpleParameter para, Grid grid)
        {
            // Bind this frame to a style
            View paraStyle = new Frame();
            paraStyle.Style = (Style)Application.Current.Resources["neutralParameterStyle"];
            this.SetBinding(Frame.StyleProperty, new Binding("Style"));
            this.BindingContext = paraStyle;


            para.OnResetEvent += delegate ()
            {
                paraStyle.Style = (Style)Application.Current.Resources["neutralParameterStyle"];
            };

            para.OnErrorEvent += delegate (Exception e)
            {
                paraStyle.Style = (Style)Application.Current.Resources["badParameterStyle"];
            };

            para.OnSuccessEvent += delegate ()
            {
                paraStyle.Style = (Style)Application.Current.Resources["goodParameterStyle"];
            };

            this.Content = grid;
        }




        /// <summary>
        /// Creates grid binded to parameter
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateParameterGrid(SimpleParameter para)
        {
            int rowMargin = (int)Application.Current.Resources["standardRowMargin"];
            int columnMargin = (int)Application.Current.Resources["standardColumnMargin"];

            Grid grid = new Grid();

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });

            if (para.UnitSelection.Length == 1)
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

            RowDefinitionCollection rowDefs = new RowDefinitionCollection();

            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });

            // Title
            rowDefs.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.Children.Add(CreateTitleLabel(para), 1, 1);

            int row = 2;
            if (para.GetType().Equals(typeof(SubFunctionParameter)))
            {
                // SubFunction Grid
                rowDefs.Add(new RowDefinition { Height = new GridLength(3, GridUnitType.Auto) });
                grid.Children.Add(CreateSubFunctionGrid((SubFunctionParameter)para), 1, row);
                row++;
            }

            // Input Value Grid Row
            rowDefs.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Auto) });
            grid.Children.Add(CreateEntryGrid(para), 1, row);
            row++;

            // Unit Grid Row
            rowDefs.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Auto) });
            grid.Children.Add(CreateUnitGrid(para), 1, row);
            row++;

            // Error Label
            rowDefs.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.Children.Add(CreateErrorLabel(para), 1, row);
            row++;

            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });


            grid.RowDefinitions = rowDefs;
            grid.ColumnDefinitions = colDefs;

            return grid;
        }




        /// <summary>
        /// Creates the title for this parameter frame
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Label CreateTitleLabel(SimpleParameter para)
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
        private Grid CreateSubFunctionGrid(SubFunctionParameter para)
        {
            Grid subFunctionGrid = BasicGrids.SimpleGrid(2, 1, 0, 0);

            Picker subFunctionPicker = new Picker();
            subFunctionPicker.SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
            subFunctionPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            subFunctionPicker.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
            subFunctionPicker.BindingContext = para.SubFunctionSelection;

            Button subFunctionBtn = CreateSubFunctionButton(para);
            subFunctionGrid.Children.Add(subFunctionPicker, 1, 1);
            subFunctionGrid.Children.Add(subFunctionBtn, 1, 2);
            return subFunctionGrid;
        }

        /// <summary>
        /// Creates a grid which contains an entry and a label for the user to input the value of this parameter
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateEntryGrid(SimpleParameter para)
        {
            Grid inputGrid = BasicGrids.SimpleGrid(1, 1, 0, 0);

            // Create input/output cells
            Entry inputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("ValueStr"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("AllowUserInput"));
            inputEntry.SetBinding(Entry.PlaceholderProperty, new Binding("Placeholder"));
            inputEntry.BindingContext = para;
            inputGrid.Children.Add(inputEntry, 1, 1);

            return inputGrid;
        }


        /// <summary>
        /// Creates a grid which contains the labels and pickers for this parameter's units
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Grid CreateUnitGrid(SimpleParameter para)
        {
            Picker[] unitPickers = new Picker[para.UnitSelection.Length];

            for (int i = 0; i < para.UnitSelection.Length; i++)
            {
                unitPickers[i] = new Picker();
                unitPickers[i].SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
                unitPickers[i].SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
                unitPickers[i].SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
                unitPickers[i].BindingContext = para.UnitSelection[i];
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


            Grid unitGrid = BasicGrids.SimpleGrid(1, totalColumns, 0, 0);


            // add pickers to grid
            unitGrid.Children.Add(unitPickers[0], 1, 1);

            if (unitPickers.Length == 2)
            {               
                unitGrid.Children.Add(new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                }, 2, 1);
                unitGrid.Children.Add(unitPickers[1], 3, 1);
            }

            return unitGrid;
        }

        /// <summary>
        /// Creates the error text label
        /// </summary>
        /// <returns></returns>
        private Label CreateErrorLabel(SimpleParameter para)
        {
            Label errorLb = new Label
            {

            };
            errorLb.SetBinding(Label.TextProperty, new Binding("ErrorMessage"));
            errorLb.BindingContext = para;

            return errorLb;
        }

        /// <summary>
        /// Binds a button to a parameter to handle using a function in the place of the parameter
        /// </summary>
        public Button CreateSubFunctionButton(SubFunctionParameter para)
        {
            Button btn = new Button();
            btn.Clicked += async delegate (System.Object o, System.EventArgs e)
            {
                await this.Navigation.PushAsync(new BasicPage(para.SubFunction));
            };

            btn.Text = LibraryResources.SubFunction;
            btn.Style = (Style)Application.Current.Resources["buttonStyle"];
            btn.SetBinding(Button.IsEnabledProperty, new Binding("AllowSubFunctionClick"));
            btn.BindingContext = para;

            return btn;
        }
    }
}
