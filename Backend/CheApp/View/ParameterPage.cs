using CheApp.Controls;
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
        public static Page Builder(IParameter parameter)
        {
            if(parameter is INumericParameter numPara)
            {
                return new ParameterPage(numPara);
            }
            else if (parameter is IPickerParameter pickPara)
            {
                return SettingPage.Builder(pickPara);
            }
            return new ParameterPage((IParameter)null);
        }

        private ParameterPage(IParameter para)
        {
            BindingContext = para;
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(para.DisplayName)));
        }

        private ParameterPage (INumericParameter para) : this((IParameter)para)
		{
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

            if (para.ParameterUnits != null)
            {
                Picker unitPicker = new Picker()
                {

                };
                int temp = para.ParameterUnits.SelectedIndex;
                unitPicker.SetBinding(Picker.SelectedIndexProperty, 
                    new Binding(nameof(para.ParameterUnits.SelectedIndex), source: para.ParameterUnits));
                unitPicker.SetBinding(Picker.ItemsSourceProperty, 
                    new Binding(nameof(para.ParameterUnits.AllOptions), source: para.ParameterUnits));
                stack.Children.Add(unitPicker);
                para.ParameterUnits.SelectedIndex = temp;
            }
        }


    }
}