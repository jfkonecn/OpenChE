using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CheApp.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FunctionPageStackLayout : StackLayout
	{
		private FunctionPageStackLayout ()
		{
			InitializeComponent ();
		}

        public static FunctionPageStackLayout Builder(string header, IEnumerable<ISetting> settings, SettingState state = SettingState.Active)
        {
            FunctionPageStackLayout page = new FunctionPageStackLayout();
            page.Header.Text = header;
            foreach(ISetting setting in settings)
            {
                page.Payload.Children.Add(new CustomFrame(setting, state));
            }
            return page;
        }

        public static FunctionPageStackLayout Builder(string header, IEnumerable<IParameter> parameters, ParameterState state)
        {
            FunctionPageStackLayout page = new FunctionPageStackLayout();
            page.Header.Text = header;
            foreach (IParameter parameter in parameters)
            {
                page.Payload.Children.Add(new CustomFrame(parameter, state));
            }
            return page;
        }


        private class CustomFrame : Frame
        {

            public CustomFrame(ISetting setting, SettingState state) : this((int)state)
            {
                BindingContext = setting;
                SetBinding(CustomFrame.TitleProperty, new Binding(nameof(setting.Name)));
                SetBinding(CustomFrame.DetailProperty, new Binding(nameof(setting.SelectOptionStr)));
                SetBinding(CustomFrame.CurrentStateProperty, new Binding(nameof(setting.CurrentState)));
                Tapped +=(object sender, EventArgs e) => { Navigation.PushAsync(SettingPage.Builder(setting)); };
            }

            public CustomFrame(IParameter para, ParameterState state) : this((int)state)
            {
                BindingContext = para;
                SetBinding(CustomFrame.TitleProperty, new Binding(nameof(para.DisplayName)));
                SetBinding(CustomFrame.DetailProperty, new Binding(nameof(para.DisplayDetail)));
                SetBinding(CustomFrame.CurrentStateProperty, new Binding(nameof(para.CurrentState)));
                Tapped += (object sender, EventArgs e) => { Navigation.PushAsync(ParameterPage.Builder(para)); };
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="visibleState">the enum which this Frame should be visible for</param>
            private CustomFrame(int visibleState)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                    case Device.Android:
                        WidthRequest = 100;
                        HeightRequest = 80;
                        Margin = 1;
                        break;
                    case Device.UWP:
                        WidthRequest = 160;
                        HeightRequest = 160;
                        Margin = 5;
                        break;
                }


                VerticalOptions = LayoutOptions.CenterAndExpand;
                HorizontalOptions = LayoutOptions.CenterAndExpand;
                BorderColor = Color.Black;
                GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Xamarin.Forms.Command(() => { OnTapped(); }) });
                VisibleState = visibleState;
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children =
                    {
                        TitleLabel,
                        DetailLabel
                    }
                };
            }

            private readonly Label TitleLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            private readonly Label DetailLabel = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
            };

            public event EventHandler Tapped;
            private void OnTapped()
            {
                Tapped?.Invoke(this, EventArgs.Empty);
            }

            public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), 
                typeof(string), typeof(CustomFrame), string.Empty, 
                propertyChanged: TitlePropertyChanged);

            private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {                
                CustomFrame frame = (CustomFrame)bindable;
                frame.TitleLabel.Text = (string)newValue;
            }

            public static readonly BindableProperty DetailProperty = BindableProperty.Create(nameof(Detail), 
                typeof(string), typeof(CustomFrame), string.Empty,
                propertyChanged: DetailPropertyChanged);
            private static void DetailPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                CustomFrame frame = (CustomFrame)bindable;
                frame.DetailLabel.Text = (string)newValue;
            }

            public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(nameof(Detail),
    typeof(int), typeof(CustomFrame), -1,
    propertyChanged: CurrentStatePropertyChanged);
            private static void CurrentStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                CustomFrame frame = (CustomFrame)bindable;
                frame.IsVisible = (int)newValue == frame.VisibleState;
            }

            public static readonly BindableProperty VisibleStateProperty = BindableProperty.Create(nameof(Detail),
typeof(int), typeof(CustomFrame), -1);


            public string Title
            {
                get { return (string)GetValue(TitleProperty); }
                set { SetValue(TitleProperty, value); }
            }

            public string Detail
            {
                get { return (string)GetValue(DetailProperty); }
                set { SetValue(DetailProperty, value); }
            }

            public int CurrentState
            {
                get { return (int)GetValue(CurrentStateProperty); }
                set { SetValue(CurrentStateProperty, value); }
            }

            public int VisibleState
            {
                get { return (int)GetValue(VisibleStateProperty); }
                set { SetValue(VisibleStateProperty, value); }
            }
        }
	}
}