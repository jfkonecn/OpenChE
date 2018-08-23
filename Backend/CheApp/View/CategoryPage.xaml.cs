using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Component;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CategoryPage : ContentPage
	{
		public CategoryPage ()
		{
			InitializeComponent ();
		}

        private void CatListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if(e.SelectedItem is CategoryItemSearchResult<Function> funResult)
            {
                Debug.WriteLine("this is a function");
                ((ListView)sender).SelectedItem = null;
                Navigation.PushAsync(new FunctionPage(funResult));
            }
            else if(e.SelectedItem is CategoryItemSearchResult<Unit> unit)
            {
                ((ListView)sender).SelectedItem = null;
                Debug.WriteLine("this is a unit");
            }
            else
            {
                Debug.WriteLine("I don't know this type");
            }
            
        }
    }
}