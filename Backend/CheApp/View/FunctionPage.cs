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
            Button solveBtn = new Button
            {
                Text = LibraryResources.Solve,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Button))
            };
            solveBtn.SetBinding(Button.CommandProperty, new Binding(nameof(fun.Solve)));
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(fun.FullName)));

            StackLayout stack = new StackLayout()
            {

            };
            if (fun.AllSettings.Count > 0)
            {
                stack.Children.Add(FunctionPageStackLayout.Builder(LibraryResources.Settings, fun.AllSettings, SettingState.Active));
            }
            stack.Children.Add(FunctionPageStackLayout.Builder(LibraryResources.Inputs, fun.AllParameters, ParameterState.Input));
            stack.Children.Add(FunctionPageStackLayout.Builder(LibraryResources.Outputs, fun.AllParameters, ParameterState.Output));

            this.Content = new StackLayout()
            {
                Margin = 5,
                Children =
                {
                    new ScrollView()
                    {                        
                        Content = stack

                    },                
                    solveBtn
                }
            };

        }
    }
}