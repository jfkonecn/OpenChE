using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EngineeringMath;
using CheApp;
using CheApp.ViewModel;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : MasterDetailPage
	{
		public MainMenu ()
		{
            InitializeComponent();
            masterPage.masterPageList.ItemSelected += MasterPageList_ItemSelected;
        }

        private void MasterPageList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType))
                {
                    BindingContext = item.BindingContext
                };
                masterPage.masterPageList.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}