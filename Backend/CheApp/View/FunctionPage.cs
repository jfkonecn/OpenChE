using EngineeringMath.Component;
using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp.View
{
    public class FunctionPage : ContentPage
    {
        public FunctionPage(Function fun)
        {
            BindingContext = fun;
            Button solveBtn = new Button();
            solveBtn.Text = LibraryResources.Solve;
            solveBtn.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button));
            solveBtn.SetBinding(Button.CommandProperty, new Binding(nameof(fun.Solve)));
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(fun.FullName)));
            this.Content = new StackLayout()
            {
                Margin = 5,
                Children =
                {
                    new ScrollView()
                    {                        
                        Content =
                            new StackLayout
                            {
                                Children =
                                {
                                    FunctionPageStackLayout.Builder(LibraryResources.Settings, fun.AllSettings),
                                    FunctionPageStackLayout.Builder(LibraryResources.Inputs, fun.AllParameters, ParameterState.Input),
                                    FunctionPageStackLayout.Builder(LibraryResources.Outputs, fun.AllParameters, ParameterState.Output)
                                }
                            }
                    },                
                    solveBtn
                }
            };

        }
    }
}