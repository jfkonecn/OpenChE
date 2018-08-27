using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp.View
{
	public class ParameterPage : ContentPage
	{
        public static ParameterPage Builder(IParameter parameter)
        {
            return new ParameterPage(parameter);
        }

        private ParameterPage (IParameter para)
		{
            BindingContext = para;
            if (para is SIUnitParameter siPara)
            {

            }

            Entry numEntry = new Entry()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            numEntry.SetBinding(Entry.TextProperty, nameof(para.BindValue));
            numEntry.SetBinding(Entry.PlaceholderProperty, nameof(para.DisplayDetail));


            Content = new StackLayout
            {
                Children =
                {
                    numEntry
                }
            };
		}
	}
}