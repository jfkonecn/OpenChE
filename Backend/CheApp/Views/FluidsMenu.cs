using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Views.Templates.MainMenu;
using CheApp.Models;
using Xamarin.Forms;
using EngineeringMath.Resources;

namespace CheApp.Views
{
	public class FluidsMenu : MainMenuDetailPage
	{
        public static readonly MasterPageItem DetailItem = new MasterPageItem(LibraryResources.Fluids, "Assets/Images/fluids.png", typeof(FluidsMenu));
        public FluidsMenu() : base(
                DetailItem,
                    new ButtonData[] {
                    new ButtonData(LibraryResources.OrificePlate, typeof(EngineeringMath.Calculations.Fluids.OrificePlate)),
                    new ButtonData(LibraryResources.BernoullisEquation, typeof(EngineeringMath.Calculations.Fluids.BernoullisEquation)),
                    new ButtonData(LibraryResources.PitotTube, typeof(EngineeringMath.Calculations.Fluids.PitotTube)),
                    new ButtonData(LibraryResources.UnitConverter, typeof(EngineeringMath.Calculations.UnitConverter.UnitConverterFunctionSubber))
                }
            )
        {

        }
	}
}