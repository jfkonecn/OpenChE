﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;
using System.Diagnostics;
using EngineeringMath.GenericObject;

namespace CheApp.Templates.CalculationPage
{
    public abstract class BasicPage : ContentPage
    {
        protected FieldStyle[] fieldStyle;
        protected SolveForBindData solveForBindData;
        private Picker solveForPicker;
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
            Grid grid = BasicGrids.SimpleGrid(myFun.fieldDic.Count + 2, 1);

            fieldStyle = new FieldStyle[myFun.fieldDic.Count];
            for (int i = 0; i < myFun.fieldDic.Count; i++)
            {
                fieldStyle[i] = new FieldStyle();
            }

            // setup solve for picker
            solveForBindData = new SolveForBindData(myFun.fieldDic.Count - 1);
            solveForPicker = new Picker();
            foreach (Parameter obj in myFun.fieldDic.Values)
            {
                solveForPicker.Items.Add(obj.Title);
            }

            solveForPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
            solveForPicker.BindingContext = solveForBindData;

            solveForPicker.SelectedIndexChanged += SolveForPicker_SelectedIndexChanged;

            solveForPicker.Title = "Solve For:";

            // Make fields correct format
            updateInputOutputs();

            grid.Children.Add(new Grid
            {
                Children =
                {
                    solveForPicker
                }
            }, 1, 1);

            CreateInputFields(ref grid);

            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = "Calculate!",
                Margin = 20
            };

            calculateBtn.Clicked += CalculateButtonClicked;

            grid.Children.Add(calculateBtn, 1, 2 + myFun.fieldDic.Count);
            Grid.SetColumnSpan(calculateBtn, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                BackgroundColor = Color.WhiteSmoke
            };
        }

        private void SolveForPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateInputOutputs();
        }

        /// <summary>
        /// Turn fields into inputs or outputs depending on the state of 
        /// </summary>
        private void updateInputOutputs()
        {
            string selectedString = solveForPicker.Items[solveForBindData.SelectedIndex];
            Debug.WriteLine($"\"{selectedString}\" was selected");
            foreach(Parameter obj in myFun.fieldDic.Values)
            {
                if (obj.Title.Equals(selectedString))
                {
                    obj.isOutput = true;
                }
                else
                {
                    obj.isInput = true;
                }
            }

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
        private void CreateInputFields(ref Grid mainGrid)
        {

            for (int i = 0; i < myFun.fieldDic.Count; i++)
            {
                // create entry cell
                Label title = new Label
                {
                    Text = myFun.fieldDic[i].Title,
                    HorizontalTextAlignment = TextAlignment.Center
                };


                // create the pickers
                Picker[] pickers = CreatePickers(myFun.fieldDic[i]);

                // create columns
                ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


                colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });

                if (pickers.Length == 1)
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
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                },
                    ColumnDefinitions = colDefs
                };


                // row 1
                grid.Children.Add(title, 1, 1);
                Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count - 2);



                Label unitLb = new Label { Text = "Units" };
                grid.Children.Add(unitLb, 2, 2);

                // row 3
                // Entry cell will be taken care of by the classes which inherit this class
                if (pickers.Length == 1)
                {
                    grid.Children.Add(pickers[0], 2, 3);

                }
                else
                {
                    grid.Children.Add(pickers[0], 2, 3);
                    grid.Children.Add(new Label
                    {
                        Text = "Per",
                        HorizontalTextAlignment = TextAlignment.Center
                    }, 3, 3);
                    grid.Children.Add(pickers[1], 4, 3);
                    Grid.SetColumnSpan(unitLb, 3);
                }


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
                inputEntry.BindingContext = myFun.fieldDic[i];
                resultsLb.SetBinding(Label.TextProperty, new Binding("ValueStr"));
                resultsLb.BindingContext = myFun.fieldDic[i];

                grid.SetBinding(Grid.BackgroundColorProperty, new Binding("BackgroundColor"));
                grid.BindingContext = fieldStyle[i];

                // row 2
                Label inputTitleLb = new Label { Text = "Input" };
                Label resultsTitleLb = new Label { Text = "Result" };

                resultsLb.SetBinding(Entry.IsVisibleProperty, new Binding("isOutput"));
                resultsLb.BindingContext = myFun.fieldDic[i];

                resultsTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("isOutput"));
                resultsTitleLb.BindingContext = myFun.fieldDic[i];

                inputEntry.SetBinding(Entry.IsVisibleProperty, new Binding("isInput"));
                inputEntry.BindingContext = myFun.fieldDic[i];

                inputTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("isInput"));
                inputTitleLb.BindingContext = myFun.fieldDic[i];

                grid.Children.Add(inputTitleLb, 1, 2);
                grid.Children.Add(resultsTitleLb, 1, 2);

                // row 3
                grid.Children.Add(inputEntry, 1, 3);
                grid.Children.Add(resultsLb, 1, 3);

                mainGrid.Children.Add(grid, 1, i + 2);
            }
        }

        /// <summary>
        /// creates a pickers for the input field
        /// </summary>
        /// <returns></returns>
        private Picker[] CreatePickers(Parameter para)
        {
            Picker[] allPickers = new Picker[para.DesiredUnits.Length];

            for (int i = 0; i < para.DesiredUnits.Length; i++)
            {
                // create the picker
                allPickers[i] = new Picker
                {

                    
                };

                allPickers[i].ItemsSource = para._PickerStrings[i];

                // bind it up!
                allPickers[i].SetBinding(Picker.SelectedIndexProperty, new Binding($"SelectedIndex[{i}]"));
                allPickers[i].BindingContext = para;
            }

            return allPickers;
        }
    }
}
