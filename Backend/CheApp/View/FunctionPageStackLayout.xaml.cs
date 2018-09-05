using CheApp.Controls;
using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FunctionPageStackLayout : StackLayout
	{
		private FunctionPageStackLayout ()
		{
			InitializeComponent ();
		}

        public static FunctionPageStackLayout Builder(string header, IEnumerable<ISetting> settings, SettingState state)
        {
            FunctionPageStackLayout page = new FunctionPageStackLayout();            
            page.Header.Text = header;
            foreach(ISetting setting in settings)
            {
                page.Payload.Children.Add(new TitleDetailButton(setting, state));
            }
            return page;
        }

        public static FunctionPageStackLayout Builder(string header, IEnumerable<IParameter> parameters, ParameterState state)
        {
            FunctionPageStackLayout page = new FunctionPageStackLayout();
            page.Header.Text = header;
            foreach (IParameter parameter in parameters)
            {
                page.Payload.Children.Add(new TitleDetailButton(parameter, state));
            }
            return page;
        }
	}
}