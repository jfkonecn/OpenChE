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
                page.Payload.Children.Add(new CustomButton(setting, state));
            }
            return page;
        }

        public static FunctionPageStackLayout Builder(string header, IEnumerable<IParameter> parameters, ParameterState state)
        {
            FunctionPageStackLayout page = new FunctionPageStackLayout();
            page.Header.Text = header;
            foreach (IParameter parameter in parameters)
            {
                page.Payload.Children.Add(new CustomButton(parameter, state));
            }
            page.InvalidateLayout();
            return page;
        }


        private class CustomButton : Button
        {

            public CustomButton(ISetting setting, SettingState state) : this((int)state)
            {
                BindingContext = setting;
                SetBinding(CustomButton.TitleProperty, new Binding(nameof(setting.Name)));
                SetBinding(CustomButton.DetailProperty, new Binding(nameof(setting.SelectOptionStr)));
                SetBinding(CustomButton.CurrentStateProperty, new Binding(nameof(setting.CurrentState)));
                Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(SettingPage.Builder(setting)); };
            }

            public CustomButton(IParameter para, ParameterState state) : this((int)state)
            {
                BindingContext = para;
                SetBinding(CustomButton.TitleProperty, new Binding(nameof(para.DisplayName)));
                SetBinding(CustomButton.DetailProperty, new Binding(nameof(para.DisplayDetail)));
                SetBinding(CustomButton.CurrentStateProperty, new Binding(nameof(para.CurrentState)));
                Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(ParameterPage.Builder(para)); };
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="visibleState">the enum which this Frame should be visible for</param>
            private CustomButton(int visibleState)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                    case Device.Android:

                        Margin = 1;
                        break;
                    case Device.UWP:

                        Margin = 5;
                        break;
                }
                VerticalOptions = LayoutOptions.CenterAndExpand;
                HorizontalOptions = LayoutOptions.CenterAndExpand;
                VisibleState = visibleState;
            }

 

            public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), 
                typeof(string), typeof(CustomButton), string.Empty, 
                propertyChanged: TitlePropertyChanged);

            private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                CustomButton btn = (CustomButton)bindable;
                btn.Text = $"{(string)newValue}\n{btn.Detail}";
            }

            public static readonly BindableProperty DetailProperty = BindableProperty.Create(nameof(Detail), 
                typeof(string), typeof(CustomButton), string.Empty,
                propertyChanged: DetailPropertyChanged);
            private static void DetailPropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                CustomButton btn = (CustomButton)bindable;
                btn.Text = $"{btn.Title}\n{(string)newValue}";
            }

            public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(nameof(Detail),
    typeof(int), typeof(CustomButton), -1,
    propertyChanged: CurrentStatePropertyChanged);
            private static void CurrentStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
            {
                CustomButton frame = (CustomButton)bindable;
                frame.IsVisible = (int)newValue == frame.VisibleState;
            }

            public static readonly BindableProperty VisibleStateProperty = BindableProperty.Create(nameof(Detail),
typeof(int), typeof(CustomButton), -1);


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