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
            Grid grid = BasicGrids.SimpleGrid(myFun.FieldDic.Count + 2, 1);

            // Setup title for the page
            Label pageTitle = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalTextAlignment = TextAlignment.Center
            };

            pageTitle.SetBinding(Label.TextProperty, new Binding("Title"));
            pageTitle.BindingContext = myFun;



            // create title block
            Grid titleGrid = BasicGrids.SimpleGrid(2, 1);
            titleGrid.Children.Add(pageTitle, 1, 1);
            
            // create solve for picker
            Picker solveForPicker = new CalculationPicker<Parameter>(fun.OutputSelection, LibraryResources.SolveFor);
            solveForPicker.SelectedIndexChanged += SolveForPicker_SelectedIndexChanged;
            myFun.OutputSelection.SelectedIndex = fun.OutputSelection.PickerList.Count - 1;
            titleGrid.Children.Add(solveForPicker, 1, 2);

            grid.Children.Add(new Grid
            {
                Children =
                {
                    titleGrid
                }
            }, 1, 1);

            int i = 0;
            foreach (Parameter para in myFun.FieldDic.Values)
            {
                //parameterStyleDic.Add(para.ID, BindingFactory.CreateInputField(this, grid, para, i));
                ParameterStyle tempStyle;
                grid.Children.Add(
                    new ParameterFrame(this, para, out tempStyle)
                    , 1, i + 2);
                parameterStyleDic.Add(para.ID, tempStyle);
                i++;
            }

            
            // setup calculate button
            Button calculateBtn = new Button
            {
                Text = LibraryResources.Calculate,
                Margin = 20
            };
            
            calculateBtn.Clicked += CalculateButtonClicked;

            Grid endPageGrid = BasicGrids.SimpleGrid(2, 1);
            endPageGrid.Children.Add(calculateBtn, 1, 1);

            grid.Children.Add(endPageGrid, 1, 2 + myFun.FieldDic.Count);
            Grid.SetColumnSpan(endPageGrid, grid.ColumnDefinitions.Count - 2);

            // finish up
            this.Content = new ScrollView
            {
                Content = grid,
                Style = (Style)Application.Current.Resources["gridStyleLevel1"]                
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



        

       
    }
}
