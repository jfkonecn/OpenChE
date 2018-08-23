using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FunctionPage : ContentPage
	{
		public FunctionPage (string catName, string functionFullName)
		{
			InitializeComponent ();
            BindingContext = MathManager.AllFunctions.GetItemByFullName(catName, functionFullName);
		}

        public FunctionPage(CategoryItemSearchResult<Function> result) : this(result.CatName, result.FullName)
        {

        }
	}
}