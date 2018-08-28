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
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(para.DisplayName)));
            StackLayout stack = new StackLayout()
            {
                Margin = 5
            };
            Content = stack;
            Entry numEntry = new Entry()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            numEntry.Text = string.Format("{0:G4}", para.BindValue);
            numEntry.SetBinding(Entry.PlaceholderProperty, nameof(para.Placeholder));
            stack.Children.Add(numEntry);
            
            Disappearing += (object sender, EventArgs e) =>
            {
                if (string.Empty.Equals(numEntry.Text) || !double.TryParse(numEntry.Text, out double num))
                    return;

                if(num >= para.MinBindValue && num <= para.MaxBindValue)
                    para.BindValue = num;
            };

            if (para is SIUnitParameter siPara)
            {
                Picker unitPicker = new Picker()
                {

                };
                int temp = siPara.ParameterUnits.SelectedIndex;
                unitPicker.SetBinding(Picker.SelectedIndexProperty, 
                    new Binding(nameof(siPara.ParameterUnits.SelectedIndex), source: siPara.ParameterUnits));
                unitPicker.SetBinding(Picker.ItemsSourceProperty, 
                    new Binding(nameof(siPara.ParameterUnits.AllOptions), source: siPara.ParameterUnits));
                stack.Children.Add(unitPicker);
                siPara.ParameterUnits.SelectedIndex = temp;
            }
        }
    }
}