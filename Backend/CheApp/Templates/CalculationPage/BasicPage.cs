using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Units;
using System.Diagnostics;
using EngineeringMath.Calculations;
using CheApp.Templates.ObjectStyleBinders;
using EngineeringMath.Resources;

namespace CheApp.Templates.CalculationPage
{
    public class BasicPage : ContentPage
    {
        protected Dictionary<int, ParameterStyle> parameterStyleDic = new Dictionary<int, ParameterStyle>();
        protected Function myFun;

        /// <summary>
        /// Sets up a basic page which handles a single function
        /// <para>Solve for data defaults to having last element in the solve for picker being selected</para>
        /// </summary>
        /// <param name="funType">The type of function which the page will represent</param>
        public BasicPage(Type funType) : this(FunctionFactory.BuildFunction(funType))
        {

        }


        /// <summary>
        /// Sets up a basic page which handles a single function
        /// <para>Solve for data defaults to having last element in the solve for picker being selected</para>
        /// </summary>
        /// <param name="fun">The function which the page will represent</param>
        public BasicPage(Function fun)
        {
            myFun = fun;
            Grid grid = CreatePageGrid();

            this.SetBinding(Page.TitleProperty, new Binding("Title"));
            this.BindingContext = myFun;


            // add solve for frame       
            grid.Children.Add(CreateSolveForFrame(), 1, 1);


            // create parameter grids
            int i = 0;
            foreach (Parameter para in myFun.FieldDic.Values)
            {
                ParameterStyle tempStyle;
                grid.Children.Add(
                    new ParameterFrame(this, para, out tempStyle)
                    , 1, i + 2);
                parameterStyleDic.Add(para.ID, tempStyle);
                i++;
            }


            // create frame for the calcuation button
            Frame calculateFrame = CreateCalculateFrame();
            grid.Children.Add(calculateFrame, 1, 2 + myFun.FieldDic.Count);
            Grid.SetColumnSpan(calculateFrame, grid.ColumnDefinitions.Count - 2);

            // create frame for the done button
            Frame doneButtonFrame = CreateDoneFrame();
            grid.Children.Add(doneButtonFrame, 1, 3 + myFun.FieldDic.Count);
            Grid.SetColumnSpan(doneButtonFrame, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                Style = (Style)Application.Current.Resources["backgroundStyle"]                
            };
        }

        private void SolveForPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // reset style
            foreach (ParameterStyle style in parameterStyleDic.Values)
            {
                style.Style = (Style)Application.Current.Resources["neutralParameterStyle"];
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
                foreach(Parameter para in myFun.FieldDic.Values)
                {
                    if(para.ErrorMessage == null)
                    {
                        // valid input
                        parameterStyleDic[para.ID].Style = (Style)Application.Current.Resources["goodParameterStyle"];
                    }
                    else
                    {
                        // bad input
                        parameterStyleDic[para.ID].Style = (Style)Application.Current.Resources["badParameterStyle"];
                    }
                }
            }
            catch (Exception err)
            {
                this.DisplayAlert(LibraryResources.ErrorMessageTitle,
                    string.Format(LibraryResources.UnexpectedException, err.GetType(), err.Message),
                    LibraryResources.Okay);
            }
        }


        /// <summary>
        /// Creates a grid for this page
        /// <para>myFun cannot be null!</para>
        /// </summary>
        /// <param name="rows">Total rows in the grid</param>
        /// <param name="cols">Total cols grid</param>
        private Grid CreatePageGrid()
        {
            int rowMargin = (int)Application.Current.Resources["standardRowMargin"];
            int columnMargin = (int)Application.Current.Resources["standardColumnMargin"];

            // create rows
            RowDefinitionCollection rowDefs = new RowDefinitionCollection();
            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });

            // Solve for picker row
            rowDefs.Add(new RowDefinition { Height = new GridLength(2, GridUnitType.Star) });

            // Parameter rows
            for (int i = 0; i < myFun.FieldDic.Count; i++)
            {
                rowDefs.Add(new RowDefinition { Height = new GridLength(5, GridUnitType.Star) });
            }

            // Calculate button row
            rowDefs.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Done Button row
            rowDefs.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            rowDefs.Add(new RowDefinition { Height = new GridLength(rowMargin, GridUnitType.Absolute) });

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();
            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });
            colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            colDefs.Add(new ColumnDefinition { Width = new GridLength(columnMargin, GridUnitType.Absolute) });



            return new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions = rowDefs,
                ColumnDefinitions = colDefs
            };
        }

        /// <summary>
        /// Create a grid to represent the user choosing the parameter to be solved for
        /// <para>myFun cannot be null!</para>
        /// </summary>
        /// <returns></returns>
        private Frame CreateSolveForFrame()
        {
            Grid solveForGrid = BasicGrids.SimpleGrid(2, 1, 0, 0);

            Label label = new Label()
            {
                Style = (Style)Application.Current.Resources["minorHeaderStyle"]
            };
            label.SetBinding(Label.TextProperty, new Binding("Title"));
            label.BindingContext = myFun.OutputSelection;

            Picker solveForPicker = new CalculationPicker<Parameter>(myFun.OutputSelection, LibraryResources.SolveFor);
            solveForPicker.SelectedIndexChanged += SolveForPicker_SelectedIndexChanged;
            myFun.OutputSelection.SelectedIndex = myFun.OutputSelection.PickerList.Count - 1;

            solveForGrid.Children.Add(label, 1, 1);
            solveForGrid.Children.Add(solveForPicker, 1, 2);

            return new Frame
            {
                Content = solveForGrid,
                Style = (Style)Application.Current.Resources["neutralParameterStyle"]
            };
        }

        /// <summary>
        /// Creates the calcualte button for this page
        /// </summary>
        /// <returns></returns>
        private Frame CreateCalculateFrame()
        {
            Button calculateBtn = new Button
            {
                Text = LibraryResources.Calculate,
                Style = (Style)Application.Current.Resources["buttonStyle"]
            };

            calculateBtn.Clicked += CalculateButtonClicked;

            return new Frame
            {
                Content = calculateBtn,
                Style = (Style)Application.Current.Resources["neutralParameterStyle"]
            };
        }

        /// <summary>
        /// Creates the done button for this page
        /// </summary>
        /// <returns></returns>
        private Frame CreateDoneFrame()
        {
            Button doneBtn = new Button
            {
                Text = LibraryResources.Done,
                Style = (Style)Application.Current.Resources["buttonStyle"]
            };

            doneBtn.Clicked += async delegate (System.Object o, System.EventArgs e)
            { await this.Navigation.PopAsync(); };

            return new Frame
            {
                Content = doneBtn,
                Style = (Style)Application.Current.Resources["neutralParameterStyle"]
            };
        }

    }
}
