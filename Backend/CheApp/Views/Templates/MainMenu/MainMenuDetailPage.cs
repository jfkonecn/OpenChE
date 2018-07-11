using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheApp.Models;

/// <summary>
/// Contains static functions related to the main menu grid 
/// </summary>
namespace CheApp.Views.Templates.MainMenu
{
    public abstract class MainMenuDetailPage : ContentPage
    {
        /// <summary>
        /// Builds a detail page for the main menu
        /// </summary>
        /// <param name="item">Master item selected</param>
        /// <param name="btnData">functions which will can be selected</param>
        public MainMenuDetailPage(MasterPageItem item, ButtonData[] btnData)
        {
            this.Title = item.Title;
            this.Icon = item.Icon;
            this.Style = (Style)Application.Current.Resources["neutralParameterStyle"];


            ListView view = new ListView()
            {
                RowHeight = 55,
                SeparatorVisibility = SeparatorVisibility.Default,
                ItemsSource = btnData,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label itemLabel = new Label()
                    {
                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                        VerticalOptions = LayoutOptions.Center
                    };
                    itemLabel.SetBinding(Label.TextProperty, "Text");

                    return new ViewCell()
                    {
                        View = new StackLayout()
                        {
                            Orientation = StackOrientation.Horizontal,
                            Padding = new Thickness(20, 10, 0, 10),
                            Spacing = 20,
                            Children =
                            {
                                itemLabel
                            }
                        }
                    };
                })
            };

            view.ItemSelected += delegate(object sender, SelectedItemChangedEventArgs e)
            {
                var curItem = e.SelectedItem as ButtonData;
                if (curItem != null)
                {
                    this.Navigation.PushAsync(new Templates.CalculationPage.BasicPage(curItem.NavFunctionType));
                    view.SelectedItem = null;
                }
            };

            this.Content = view;

        }




        /// <summary>
        /// Creates a button which navigates to a page specified
        /// </summary>
        /// <param name="btnData"></param>
        /// <returns></returns>
        private Button NavButton(ButtonData btnData)
        {
            Button btn = new Button();
            btn.Text = btnData.Text;
            btn.Style = (Style)Application.Current.Resources["buttonStyle"];
            btn.Clicked += async delegate (System.Object o, System.EventArgs e)
            { await this.Navigation.PushAsync(new Templates.CalculationPage.BasicPage(btnData.NavFunctionType)); };

            return btn;
        }
    }
}
