using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EngineeringMath;
using CheApp;


namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : MasterDetailPage
	{
		public MainMenu ()
		{
            InitializeComponent();
            //BindingContext = MathManager.AllFunctions;
        }
	}
}