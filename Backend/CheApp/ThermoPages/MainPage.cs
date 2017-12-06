using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringMath.Resources;
using Xamarin.Forms;
/*
 * Grid Help
 * https://msdn.microsoft.com/en-us/library/system.windows.controls.grid(v=vs.110).aspx
 */

namespace CheApp.ThermoPages
{
	public class MainPage : ContentPage
	{
		public MainPage()
		{
            CheApp.Templates.MainMenu.MainMenuTab.TabbedPage(
            this,
            LibraryResources.Thermo,
            "thermodynamics.png",
            new CheApp.Templates.MainMenu.ButtonData[] {
                    new CheApp.Templates.MainMenu.ButtonData("Hello", new FluidsPages.OrificePlate()),
                    new CheApp.Templates.MainMenu.ButtonData("Hello2", new FluidsPages.OrificePlate())
            });



        }
	}
}
