using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.MainMenu;
using Xamarin.Forms;
using EngineeringMath.Resources;

namespace CheApp.FluidsPages
{
	public class MainPage : ContentPage
	{
		public MainPage()
		{

            CheApp.Templates.MainMenu.MainMenuTab.TabbedPage(
            this,
            LibraryResources.Fluids,
            "water.png",
            new CheApp.Templates.MainMenu.ButtonData[] {
                    new CheApp.Templates.MainMenu.ButtonData(LibraryResources.OrificePlate, typeof(EngineeringMath.Calculations.Fluids.OrificePlate)),
                    new CheApp.Templates.MainMenu.ButtonData(LibraryResources.BernoullisEquation, typeof(EngineeringMath.Calculations.Fluids.BernoullisEquation)),
                    new CheApp.Templates.MainMenu.ButtonData(LibraryResources.PitotTube, typeof(EngineeringMath.Calculations.Fluids.PitotTube))
            });
        }
	}
}