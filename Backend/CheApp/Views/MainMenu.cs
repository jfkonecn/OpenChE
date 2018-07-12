using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Resources;
using CheApp.Models;

/*
 * For Help
 * https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.masterdetailpage/
     */

namespace CheApp.Views
{
	public class MainMenu : MasterDetailPage
    {

        public MainMenu()
        {
            this.Title = LibraryResources.AppTitle;
            

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Padding = new Thickness(20, 40, 20, 20);
                    break;
                case Device.Android:
                    Padding = new Thickness(20, 40, 20, 20);
                    break;
                case Device.UWP:
                    Padding = new Thickness(20);
                    break;
            }

            MasterPageItem[] ItemsSourceArr = new MasterPageItem[]
            {
                ThermoMenu.DetailItem,
                FluidsMenu.DetailItem
            };
            Array.Sort(ItemsSourceArr, (x, y) => x.Title.CompareTo(y.Title));

            ListView = new ListView()
            {
                SeparatorVisibility = SeparatorVisibility.Default,
                ItemsSource = ItemsSourceArr,
                ItemTemplate = new DataTemplate(() => {

                    Image itemImage = new Image()
                    {
                        WidthRequest = 40,
                        HeightRequest = 40,
                        VerticalOptions = LayoutOptions.Start
                    };
                    itemImage.SetBinding(Image.SourceProperty, "Icon");

                    Label itemLabel = new Label()
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Color.Black
                    };
                    itemLabel.SetBinding(Label.TextProperty, "Title");

                    return new ViewCell()
                    {
                        View = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(20, 10, 0, 10),
                            Spacing = 20,
                            Children =
                            {
                                itemImage,
                                itemLabel
                            }
                        }
                    };
                })
            };
            ListView.ItemSelected += OnItemSelected;
            
            this.Master = new ContentPage()
            {
                Title = LibraryResources.Menu,
                Content = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Children =
                    {

                        ListView
                    }
                }
            };





            this.Detail = new NavigationPage(new FluidsMenu());
            this.MasterBehavior = MasterBehavior.Popover;
        }

        public ListView ListView { private set; get; }



        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                ListView.SelectedItem = null;
                IsPresented = false;
            }
        }



    }
}