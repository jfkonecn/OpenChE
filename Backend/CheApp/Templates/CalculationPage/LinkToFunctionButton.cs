using System;
using System.Collections.Generic;
using System.Text;
using CheApp.Templates.ObjectStyleBinders;
using Xamarin.Forms;
using EngineeringMath.Calculations;
using EngineeringMath.Resources;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// A button which binds itself to a parameter and then creates a link to a function 
    /// </summary>
    public class LinkToFunctionButton : Button
    {
        /// <summary>
        /// Binds a button to a parameter to handle using a function in the place of the parameter
        /// </summary>
        public LinkToFunctionButton(Page currentPage, Parameter para)
        {
            this.Clicked += async delegate(System.Object o, System.EventArgs e) 
            {
                await currentPage.Navigation.PushAsync(new BasicPage(para.SubFunction));
            };

            this.Text = LibraryResources.SubFunction;
            this.Style = (Style)Application.Current.Resources["buttonStyle"];
            this.SetBinding(Button.IsEnabledProperty, new Binding("AllowSubFunctionClick"));
            this.BindingContext = para;
        }
    }
}
