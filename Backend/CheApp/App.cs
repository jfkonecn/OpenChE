using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CheApp
{
    /*
     * https://developer.xamarin.com/guides/xamarin-forms/user-interface/styles/application/
     */
    public partial class App : Application
    {
        public App()
        {
            Resources = new ResourceDictionary();

            Style buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.TextColorProperty,   Value = Color.White },
                    new Setter { Property = Button.BackgroundColorProperty,   Value = Color.Gray }
                }
            };
            Resources.Add("buttonStyle", buttonStyle);

            Style pageStyle = new Style(typeof(ContentPage))
            {

            };

            MainPage = new NavigationPage(new CheApp.MainMenu());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}