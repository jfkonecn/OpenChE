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

            // Style for grids with no parent grids
            Style gridStyleLevel1 = new Style(typeof(Grid))
            {
                Setters =
                {
                    new Setter { Property = Button.TextColorProperty,   Value = Color.Black },
                    new Setter { Property = Grid.BackgroundColorProperty,   Value = Color.White }
                }
            };
            Resources.Add("gridStyleLevel1", gridStyleLevel1);

            // Style for grids with only one parent grid
            Style gridStyleLevel2 = new Style(typeof(Grid))
            {
                Setters =
                {
                    new Setter { Property = Button.TextColorProperty,   Value = Color.White },
                    new Setter { Property = Grid.BackgroundColorProperty,   Value = Color.WhiteSmoke }
                }
            };
            Resources.Add("gridStyleLevel2", gridStyleLevel2);



            Style parameterStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter { Property = Frame.OutlineColorProperty, Value = Color.Silver },
                    new Setter { Property = Frame.BackgroundColorProperty, Value = Color.WhiteSmoke }
                }
            };
            Resources.Add("parameterStyle", parameterStyle);


            Style minorHeaderStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Label))}
                }
            };
            Resources.Add("minorHeaderStyle", minorHeaderStyle);

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