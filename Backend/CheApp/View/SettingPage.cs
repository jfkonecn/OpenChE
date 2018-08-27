using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp.View
{
	public class SettingPage : ContentPage
	{
        public static SettingPage Builder(ISetting setting)
        {
            int temp = setting.SelectedIndex;
            SettingPage page = new SettingPage
            {
                BindingContext = setting
            };
            // creating a picker sets the Selected Index to -1
            setting.SelectedIndex = temp;
            return page;
        }

        private SettingPage()
        {
            Picker picker = new Picker()
            {
                SelectedIndex = 0
            };
            picker.SetBinding(Picker.TitleProperty, new Binding(nameof(ISetting.Name)));
            picker.SetBinding(Picker.SelectedIndexProperty, new Binding(nameof(ISetting.SelectedIndex)));
            picker.SetBinding(Picker.ItemsSourceProperty, new Binding(nameof(ISetting.AllOptions)));
            SetBinding(ContentPage.TitleProperty, new Binding(nameof(ISetting.Name)));
            Content = new StackLayout() { Children = { picker } };
        }
    }
}