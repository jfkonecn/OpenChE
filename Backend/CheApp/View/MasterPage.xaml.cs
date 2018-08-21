using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CheApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EngineeringMath;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{
		public MasterPage ()
		{
			InitializeComponent ();
		}
	}
}