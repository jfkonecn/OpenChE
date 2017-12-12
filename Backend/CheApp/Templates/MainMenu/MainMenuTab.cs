using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/// <summary>
/// Contains static functions related to the main menu grid 
/// </summary>
namespace CheApp.Templates.MainMenu
{
    internal class MainMenuTab
    {

        /// <summary>
        /// Template for a single tabbed page in the main menu
        /// </summary>
        /// <param name="title">title of the tabbed page</param>
        /// <param name="iconUrl">the name of the icon location i.e. "xyz.png"
        /// <para>Note: </para>
        /// <para>On andorid the image will be searched for in the Resources/Drawable folder</para>
        /// <para>On iOS the image will be searched for in the Rosources folder</para>
        /// <para>On windows the image will not show up at all (not sure if it it's possible)</para>
        /// </param>
        /// <param name="contentPage">The tabbed page (this parameter will be altered to fit the template)</param>
        internal static void TabbedPage(ContentPage contentPage, string title, string iconUrl, ButtonData[] btnData)
        {
            contentPage.Title = title;
            contentPage.Icon = iconUrl;
            Grid grid = BasicGrids.SimpleGrid(btnData.Length, 1, 20, 40);           
            for (int i = 0; i < btnData.Length; i++)
            {
                Button btn = TabbedPageButton(contentPage, btnData[i]);
                grid.Children.Add(btn, 1, i + 1);
            }

            // Build the page.
            contentPage.Content = new ScrollView
            {
                Content = grid
            };


        }



        /// <summary>
        /// Creates a button with a click event to navigate to a page which handles the function calculations
        /// </summary>
        /// <param name="tabbedPage">The page where the button is being placed</param>
        /// <param name="btnData">Button data used to customize the button</param>
        /// <returns>The button with a click event to the desired page</returns>
        private static Button TabbedPageButton(ContentPage tabbedPage, ButtonData btnData)
        {
            Button btn = new Button();
            btn.Text = btnData.Text;

            btn.Clicked += async delegate (System.Object o, System.EventArgs e)
            { await tabbedPage.Navigation.PushAsync(btnData.NavPage); };

            return btn;
        }
    }
}
