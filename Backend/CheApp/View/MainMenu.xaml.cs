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
            if (e.SelectedItem is MasterPageItem item)
            {
                Page temp = (Page)Activator.CreateInstance(item.TargetType);
                temp.BindingContext = item.BindingContext;
                Detail = new NavigationPage(temp);
                masterPage.masterPageList.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}