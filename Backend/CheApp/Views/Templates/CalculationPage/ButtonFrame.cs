using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Resources;

namespace CheApp.Views.Templates.CalculationPage
{
    public class ButtonFrame : Frame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">The button text</param>
        /// <param name="clickFunction"></param>
        public ButtonFrame(string text, EventHandler clickFunction)
        {
            Button btn = new Button
            {
                Text = text,
                Style = (Style)Application.Current.Resources["buttonStyle"]
            };

            btn.Clicked += clickFunction;

            this.Content = btn;
            this.Style = (Style)Application.Current.Resources["neutralParameterStyle"];
        }
    }
}
