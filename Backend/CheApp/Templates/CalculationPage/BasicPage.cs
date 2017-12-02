﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;
using System.Diagnostics;
using EngineeringMath.Calculations;

namespace CheApp.Templates.CalculationPage
{
    public abstract class BasicPage : ContentPage
    {
        protected FieldStyle[] fieldStyle;
        protected Function myFun;



        // TODO: make it so that more than one function can be used on a page 
        // ie switch between a direct input for density and using the ideal gas law to calculate density

        /// <summary>
        /// Sets up a basic page which handles a single function
        /// <para>Solve for data defaults to having last element in the solve for picker being selected</para>
        /// </summary>
        /// <param name="pageFun">Function which the page will represent</param>
        public void PageSetup(Function pageFun)
        {
            myFun = pageFun;
            Grid grid = BasicGrids.SimpleGrid(myFun.FieldDic.Count + 2, 1);

            fieldStyle = new FieldStyle[myFun.FieldDic.Count];
            for (int i = 0; i < myFun.FieldDic.Count; i++)
            {
                fieldStyle[i] = new FieldStyle();
            }

            // Setup title for the page
            Label pageTitle = new Label()
            {
                FontSize = Device.GetNamedSize (NamedSize.Large, typeof(Label))
            };

            pageTitle.SetBinding(Label.TextProperty, new Binding("Title"));
            pageTitle.BindingContext = myFun;



            // create title block
            Grid titleGrid = BasicGrids.SimpleGrid(2, 1);
            titleGrid.Children.Add(pageTitle, 1, 1);
            titleGrid.Children.Add(CreateSolverForPicker(myFun), 1, 2);

            grid.Children.Add(new Grid
            {
                Children =
                {
                    titleGrid
                }
            }, 1, 1);
            for (int i = 0; i < myFun.FieldDic.Count; i++)
            {
                CreateInputFields(ref grid, myFun.FieldDic[i], fieldStyle[i], i);
            }
                

            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = "Calculate!",
                Margin = 20
            };

            calculateBtn.Clicked += CalculateButtonClicked;

            grid.Children.Add(calculateBtn, 1, 2 + myFun.FieldDic.Count);
            Grid.SetColumnSpan(calculateBtn, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                BackgroundColor = Color.WhiteSmoke
            };
        }

        /// <summary>
        /// Make sure parameters are valid and execute function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CalculateButtonClicked(object sender, EventArgs e)
        {

            try
            {
                myFun.Solve();
            }
            catch (OverflowException)
            {
                this.DisplayAlert("ERROR!", $"The result is greater than {double.MaxValue}!", "OK!");
            }
            catch (System.FormatException)
            {
                this.DisplayAlert("ERROR!", "All inputs must be a type of number!", "OK!");
            }
            catch (Exception err)
            {
                this.DisplayAlert("ERROR!",
                    string.Format("Unexpected exception of type {0} caught: {1}", err.GetType(), err.Message),
                    "OK");
            }
        }



        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        private void CreateInputFields(ref Grid mainGrid, Parameter para, FieldStyle style, int rowIdx)
        {
            // create entry cell
            Label title = new Label
            {
                Text = para.Title,
                HorizontalTextAlignment = TextAlignment.Center
            };


            // create the pickers
            Picker[] unitPickers = CreateUnitPickers(para);

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


            colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });

            if (unitPickers.Length == 1)
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




            
            Label unitLb = new Label { Text = "Units" };
            // create entry cell
            Entry inputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HeightRequest = 10
            };

            Label resultsLb = new Label
            {

            };
            // bind it up!
            inputEntry.SetBinding(Entry.TextProperty, new Binding("ValueStr"));
            inputEntry.BindingContext = para;
            resultsLb.SetBinding(Label.TextProperty, new Binding("ValueStr"));
            resultsLb.BindingContext = para;

            grid.SetBinding(Grid.BackgroundColorProperty, new Binding("BackgroundColor"));
            grid.BindingContext = style;

            Label inputTitleLb = new Label { Text = "Input" };
            Label resultsTitleLb = new Label { Text = "Result" };

            resultsLb.SetBinding(Entry.IsVisibleProperty, new Binding("DontAllowUserInput"));
            resultsLb.BindingContext = para;

            resultsTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("DontAllowUserInput"));
            resultsTitleLb.BindingContext = para;

            inputEntry.SetBinding(Entry.IsVisibleProperty, new Binding("AllowUserInput"));
            inputEntry.BindingContext = para;

            inputTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("AllowUserInput"));
            inputTitleLb.BindingContext = para;
            Picker subFunctionPicker = CreateSubFunctionPicker(para);



            // row 1
            grid.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count - 2);

            // row 2
            grid.Children.Add(subFunctionPicker, 1, 2);
            Grid.SetColumnSpan(subFunctionPicker, grid.ColumnDefinitions.Count - 2);

            // row 3
            grid.Children.Add(inputTitleLb, 1, 3);
            grid.Children.Add(resultsTitleLb, 1, 3);
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
                    Text = "Per",
                    HorizontalTextAlignment = TextAlignment.Center
                }, 3, 3);
                grid.Children.Add(unitPickers[1], 4, 4);
                Grid.SetColumnSpan(unitLb, 3);
            }
            grid.Children.Add(inputEntry, 1, 4);
            grid.Children.Add(resultsLb, 1, 4);



            mainGrid.Children.Add(grid, 1, rowIdx + 2);

        }


        /// <summary>
        /// creates a pickers for selecting the output field
        /// </summary>
        /// <returns></returns>
        private Picker CreateSolverForPicker(Function fun)
        {
            Picker picker = BindingFactory.GenericBindings<Parameter>.PickerFactory(ref fun.OutputSelection);
            picker.Title = "Solve For:";
            fun.OutputSelection.SelectedIndex = fun.OutputSelection.PickerList.Count - 1;
            return picker;
        }

        /// <summary>
        /// Creates the a picker to allow the user to select substitute functions for the given parameter
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Picker CreateSubFunctionPicker(Parameter para)
        {
            Picker picker = BindingFactory.GenericBindings<FunctionFactory.FactoryData>.PickerFactory(ref para.SubFunctionSelection);
            para.SubFunctionSelection.SelectedIndex = 0;
            return picker;
        }

        /// <summary>
        /// creates a pickers for selecting units for the field
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        private Picker[] CreateUnitPickers(Parameter para)
        {
            Picker[] allPickers = new Picker[para.DesiredUnits.Length];

            for (int i = 0; i < para.DesiredUnits.Length; i++)
            {
                allPickers[i] = BindingFactory.GenericBindings<AbstractUnit>.PickerFactory(ref para.UnitSelection[i]);
                para.UnitSelection[i].SelectedIndex = 0;
            }

            return allPickers;
        }




        private Button CreateSubFunctionButton()
        {
            return null;
        }
    }
}
