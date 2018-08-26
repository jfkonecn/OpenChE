using EngineeringMath.Component;
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
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(fun.FullName)));
            this.Content = new ScrollView()
            {
                Content = new Frame()
                {
                    Margin = 20,
                    Content = new StackLayout()
                    {
                        Children =
                        {
                            new FunctionPageStackLayout(),
                            new FunctionPageStackLayout()
                        }
                    }
                }

            };

        }
    }
}