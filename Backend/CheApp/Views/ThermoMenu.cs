using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EngineeringMath.Resources;
using EngineeringMath.Calculations;
using Xamarin.Forms;
using CheApp.Views.Templates.MainMenu;
using CheApp.Models;
using CheApp;
/*
 * Grid Help
 * https://msdn.microsoft.com/en-us/library/system.windows.controls.grid(v=vs.110).aspx
 */

namespace CheApp.Views
{
	public class ThermoMenu : MainMenuDetailPage
	{
        public static readonly MasterPageItem DetailItem = new MasterPageItem(LibraryResources.Thermodynamics, "Assets/Images/thermodynamics.png", typeof(ThermoMenu));
        public ThermoMenu() : base(
                DetailItem,
                new ButtonData[] {
                        new ButtonData(LibraryResources.SteamTable, typeof(EngineeringMath.Calculations.Thermo.ThermoLookupTables.SteamTable)),
                        new ButtonData(LibraryResources.ThermodynamicCycle, typeof(EngineeringMath.Calculations.Thermo.Cycles.AllCycles))
                }
            )
		{

        }
	}
}
