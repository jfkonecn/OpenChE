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
    /// A button which binds itself to the subfunction 
    /// </summary>
    public class LinkToFunctionButton : Button
    {
        /// <summary>
        /// Binds a button to a parameter to handle using a function in the place of the parameter
        /// </summary>

        /// <param name="style">The style to be binded to this object</param>
        public LinkToFunctionButton(Page currentPage, Parameter para, out PageLinkStyle style)
        {
            // create style
            style = new PageLinkStyle(currentPage, delegate ()
            {
                return new BasicPage(para.SubFunction);
            });

            // bind style to the parameter object
            style.SetBinding(PageLinkStyle.IsEnabledProperty, new Binding("AllowSubFunctionClick", BindingMode.TwoWay));
            style.BindingContext = para;



            this.Clicked += style.OnClickFunction;

            style.Text = LibraryResources.SubFunction;

            this.SetBinding(Button.TextProperty, new Binding("Text"));
            this.SetBinding(Button.StyleProperty, new Binding("ButtonStyle"));
            this.SetBinding(Button.IsEnabledProperty, new Binding("IsEnabled"));
            this.BindingContext = style;
        }
    }
}
