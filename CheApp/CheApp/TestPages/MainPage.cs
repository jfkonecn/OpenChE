using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.TestPages
{
    class MainPage : ContentPage
    {
        public MainPage()
        {
            CheApp.Templates.MainMenu.MainMenuTab.TabbedPage(
                this,
                "Debug Tests",
                "",
                new CheApp.Templates.MainMenu.ButtonData[] {
                new CheApp.Templates.MainMenu.ButtonData("Che Math", new TestPages.CheMath())
                });
        }
    }
}
