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
            Style backgroundStyle = new Style(typeof(Style))
            {
                Setters =
                {
                    new Setter { Property = Grid.BackgroundColorProperty,   Value = Color.LightGray }
                }
            };
            Resources.Add("backgroundStyle", backgroundStyle);


            // used before the user takes any actions
            Style neutralParameterStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter { Property = Frame.OutlineColorProperty, Value = Color.Silver },
                    new Setter { Property = Frame.BackgroundColorProperty, Value = Color.WhiteSmoke }
                }
            };
            Resources.Add("neutralParameterStyle", neutralParameterStyle);

            // used when the user's parameter inputs are valid
            Style goodParameterStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter { Property = Frame.OutlineColorProperty, Value = Color.Green },
                    new Setter { Property = Frame.BackgroundColorProperty, Value = Color.LightGreen }
                }
            };
            Resources.Add("goodParameterStyle", goodParameterStyle);

            // used when the user's parameter inputs are invalid
            Style badParameterStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter { Property = Frame.OutlineColorProperty, Value = Color.Red },
                    new Setter { Property = Frame.BackgroundColorProperty, Value = Color.PaleVioletRed }
                }
            };
            Resources.Add("badParameterStyle", badParameterStyle);


            Style minorHeaderStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Label))}
                }
            };
            Resources.Add("minorHeaderStyle", minorHeaderStyle);

            // used for grid margins
            int standardRowMargin = 5;
            Resources.Add("standardRowMargin", standardRowMargin);
            int standardColumnMargin = 5;
            Resources.Add("standardColumnMargin", standardColumnMargin);


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