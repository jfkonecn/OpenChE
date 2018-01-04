using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using CheApp.Templates.MainMenu;
using EngineeringMath.Resources;

/*
 * For Help
 * https://developer.xamarin.com/api/type/Xamarin.Forms.TabbedPage/
     */

namespace CheApp
{
	public class MainMenu : TabbedPage
    {

        public MainMenu()
		{
            this.Title = LibraryResources.AppTitle;
            this.Style = (Style)Application.Current.Resources["backgroundStyle"];

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Padding = new Thickness(20, 40, 20, 20);
                    break;
                case Device.Android:
                    Padding = new Thickness(20, 40, 20, 20);
                    break;
                case Device.WinPhone:
                    Padding = new Thickness(20);
                    break;
            }

            this.Children.Add(new FluidsPages.MainPage());
            this.Children.Add(new ThermoPages.MainPage());            
        }









    }
}