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
                    new Setter { Property = Button.TextColorProperty,   Value = Color.White },
                    new Setter { Property = Button.BackgroundColorProperty,   Value = Color.Gray }
                }
            };
            Resources.Add("gridStyleLevel1", gridStyleLevel1);



            Style buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter { Property = Button.TextColorProperty,   Value = Color.White },
                    new Setter { Property = Button.BackgroundColorProperty,   Value = Color.Gray }
                }
            };
            Resources.Add("buttonStyle", buttonStyle);

            Style numericEntryStyle = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter { Property = Entry.KeyboardProperty,   Value = Keyboard.Numeric },
                    new Setter { Property = Entry.HeightRequestProperty, Value = 10 },
                    new Setter { Property = Entry.BackgroundColorProperty,   Value = Color.LightGray }
                }
            };
            Resources.Add("numericEntryStyle", numericEntryStyle);

            Style pickerStyle = new Style(typeof(Picker))
            {
                Setters =
                {
                    new Setter { Property = Picker.BackgroundColorProperty,   Value =  Color.LightGray }
                }
            };
            Resources.Add("pickerStyle", numericEntryStyle);

            Style entryLabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Small, typeof(Label))},
                    new Setter { Property = Label.BackgroundColorProperty,   Value = Color.Gray }
                }
            };
            Resources.Add("entryLabelStyle", entryLabelStyle);


            Style parameterStyle = new Style(typeof(Grid))
            {
                Setters =
                {
                    new Setter { Property = Grid.BackgroundColorProperty, Value = Color.Yellow}
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