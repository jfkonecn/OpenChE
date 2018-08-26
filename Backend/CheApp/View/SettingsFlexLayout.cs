using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp.View
{
	public class SettingsFlexLayout : FlexLayout
	{
		public SettingsFlexLayout(IEnumerable<ISetting> settings)
		{
            foreach(ISetting setting in settings)
            {
                Children.Add(new BoxView());
            }
		}
	}
}