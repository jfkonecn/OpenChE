using System;
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

            int i = 0;
            foreach (Parameter para in myFun.FieldDic.Values)
            {
                parameterStyleDic.Add(para.ID, BindingFactory.CreateInputField(this, grid, para, i));
                i++;
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
        /// creates a pickers for selecting the output field
        /// </summary>
        /// <returns></returns>
        private Picker CreateSolverForPicker(Function fun)
        {
            Picker picker = BindingFactory.GenericBindings<Parameter>.PickerFactory(fun.OutputSelection);
            picker.Title = "Solve For:";
            fun.OutputSelection.SelectedIndex = fun.OutputSelection.PickerList.Count - 1;
            return picker;
        }

       
    }
}
