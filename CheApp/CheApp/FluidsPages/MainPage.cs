using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.MainMenu;
using Xamarin.Forms;

namespace CheApp.FluidsPages
{
	public class MainPage : ContentPage
	{
		public MainPage()
		{

            CheApp.Templates.MainMenu.MainMenuTab.TabbedPage(
            this,
            "Fluids",
            "water.png",
            new CheApp.Templates.MainMenu.ButtonData[] {
                    new CheApp.Templates.MainMenu.ButtonData("Orifice Plate", new FluidsPages.OrificePlate()),
                    new CheApp.Templates.MainMenu.ButtonData("Bernoulli's\n Equation", new FluidsPages.OrificePlate()),
                    new CheApp.Templates.MainMenu.ButtonData("Pitot Tube", new FluidsPages.OrificePlate())
            });
        }
	}
}