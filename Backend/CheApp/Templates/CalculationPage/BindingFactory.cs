using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Calculations;
using EngineeringMath.Units;
using EngineeringMath.Resources;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Contains static functions which bind EngineeringMath objects to Xamarin.Forms objects
    /// </summary>
    internal class BindingFactory
    {
        // this is an example of how to bind to an array
        // Not needed here, but was used at one point
        // allPickers[i].SetBinding(Picker.ItemsSourceProperty, new Binding($"PickerStrings[{i}]"));

        /// <summary>
        /// Class which contains binds for objects which contain generic objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class GenericBindings<T>
        {
            /// <summary>
            /// Binds a picker selection object to a picker
            /// </summary>
            /// <param name="obj"></param>
            /// <returns>A picker binded to the PickerSelection object</returns>
            internal static Picker PickerFactory(PickerSelection<T> obj)
            {

                Picker picker = new Picker();

                // bind it up!
                picker.SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
                picker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
                picker.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
                picker.BindingContext = obj;

                return picker;
            }



        }

        /// <summary>
        /// Binds a button to a parameter to handle using a function in the place of the parameter
        /// </summary>
        /// <param name="page">Current Page</param>
        /// <param name="para">Parameter to be binded to</param>
        /// <returns></returns>
        internal static Button CreateSubFunctionButton(ContentPage page, Parameter para)
        {
            Button subFunctionButton = new Button
            {
                Text = LibraryResources.SubFunction
            };

            subFunctionButton.Clicked += async delegate (System.Object o, System.EventArgs e)
            {

                if(para.SubFunctionSelection.SelectedObject != null)
                {
                    BasicPage calPage = new BasicPage(para.SubFunction);
                    await page.Navigation.PushAsync(calPage);
                }
 
            };
            return subFunctionButton;
        }


        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal static ParameterStyle CreateInputField(ContentPage page, Grid mainGrid, Parameter para, int rowIdx)
        {
            ParameterStyle paraStyle = new ParameterStyle(para);

            // create entry cell
            Label title = new Label
            {
                Text = para.Title
            };
            title.SetBinding(Label.StyleProperty, new Binding("TitleStyle"));
            title.BindingContext = paraStyle;


            // create the pickers
            Picker[] unitPickers = CreateUnitPickers(para);


            Grid grid = CreateParameterGrid(unitPickers.Length);




            Label unitLb = new Label { Text = LibraryResources.Units };
            
            
            // Create input/output cells

            Entry inputEntry = new Entry
            {
                
            };
            inputEntry.SetBinding(Entry.TextProperty, new Binding("EntryText"));
            inputEntry.SetBinding(Entry.IsEnabledProperty, new Binding("EntryIsEnabled"));
            inputEntry.SetBinding(Entry.StyleProperty, new Binding("EntryStyle"));
            inputEntry.BindingContext = paraStyle;
            
            


            Label valueTitleLb = new Label { Text = LibraryResources.Value };
            valueTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("AllowUserInput"));
            valueTitleLb.BindingContext = para;


            Picker subFunctionPicker = CreateSubFunctionPicker(para, paraStyle);

            Button subFunctionBtn = CreateSubFunctionButton(page, para);

            // row 1
            grid.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count - 2);

            // row 2
            grid.Children.Add(subFunctionPicker, 1, 2);
            Grid.SetColumnSpan(subFunctionPicker, grid.ColumnDefinitions.Count - 2);

            // row 3
            grid.Children.Add(valueTitleLb, 1, 3);
            grid.Children.Add(unitLb, 2, 3);


            // row 4
            // Entry cell will be taken care of by the classes which inherit this class
            if (unitPickers.Length == 1)
            {
                grid.Children.Add(unitPickers[0], 2, 4);

            }
            else
            {
                grid.Children.Add(unitPickers[0], 2, 4);
                grid.Children.Add(new Label
                {
                    Text = LibraryResources.Per,
                    HorizontalTextAlignment = TextAlignment.Center
                }, 3, 4);
                grid.Children.Add(unitPickers[1], 4, 4);
                Grid.SetColumnSpan(unitLb, 3);
            }
            grid.Children.Add(inputEntry, 1, 4);


            // row 5
            grid.Children.Add(subFunctionBtn, 1, 5);
            Grid.SetColumnSpan(subFunctionBtn, grid.ColumnDefinitions.Count - 2);

            mainGrid.Children.Add(grid, 1, rowIdx + 2);

            return paraStyle;
        }

        /// <summary>
        /// Creates a grid intended to be binded to a parameter object
        /// </summary>
        /// <param name="unitPickersLength"></param>
        /// <returns></returns>
        private static Grid CreateParameterGrid(int unitPickersLength)
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

            Grid grid = new Grid
            {
                Style = (Style)Application.Current.Resources["parameterStyle"],
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                },
                ColumnDefinitions = colDefs
            };

            return grid;
        }

        /// <summary>
        /// Creates the a picker to allow the user to select substitute functions for the given parameter
        /// </summary>
        /// <param name="paraStyle"></param>
        /// <returns></returns>
        private static Picker CreateSubFunctionPicker(Parameter para, ParameterStyle paraStyle)
        {
            Picker picker = BindingFactory.GenericBindings<FunctionFactory.FactoryData>.PickerFactory(para.SubFunctionSelection);
            para.SubFunctionSelection.SelectedIndex = 0;
            
            picker.SetBinding(Picker.StyleProperty, new Binding("PickerStyle"));
            //picker.BindingContext = paraStyle;
            return picker;
        }

        /// <summary>
        /// creates a pickers for selecting units for the field
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private static Picker[] CreateUnitPickers(Parameter para)
        {
            Picker[] allPickers = new Picker[para.DesiredUnits.Length];
            
            for (int i = 0; i < para.DesiredUnits.Length; i++)
            {
                allPickers[i] = BindingFactory.GenericBindings<AbstractUnit>.PickerFactory(para.UnitSelection[i]);
                para.UnitSelection[i].SelectedIndex = 0;
            }

            return allPickers;
        }


    }
}
