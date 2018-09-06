using CheApp.View;
using EngineeringMath.Component;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Controls
{
    public class TitleDetailButton : Button
    {
        public TitleDetailButton() : base()
        {
            
        }
        public TitleDetailButton(ISetting setting, SettingState state) : this((int)state)
        {
            BindingContext = setting;
            SetBinding(TitleDetailButton.TitleProperty, new Binding(nameof(setting.Name)));
            SetBinding(TitleDetailButton.DetailProperty, new Binding(nameof(setting.SelectOptionStr)));
            SetBinding(TitleDetailButton.CurrentStateProperty, new Binding(nameof(setting.CurrentState)));
            Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(SettingPage.Builder(setting)); };
        }

        public TitleDetailButton(IParameter para, ParameterState state) : this((int)state)
        {
            BindingContext = para;
            SetBinding(TitleDetailButton.TitleProperty, new Binding(nameof(para.DisplayName)));
            SetBinding(TitleDetailButton.DetailProperty, new Binding(nameof(para.DisplayDetail)));
            SetBinding(TitleDetailButton.CurrentStateProperty, new Binding(nameof(para.CurrentState)));
            Clicked += (object sender, EventArgs e) => { Navigation.PushAsync(ParameterPage.Builder(para)); };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibleState">the enum which this Frame should be visible for</param>
        private TitleDetailButton(int visibleState) : base()
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
            CornerRadius = 75;
            HeightRequest = CornerRadius * 2;
            WidthRequest = CornerRadius * 2;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            HorizontalOptions = LayoutOptions.CenterAndExpand;
            VisibleState = visibleState;
        }



        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string), typeof(TitleDetailButton), string.Empty,
            propertyChanged: TitlePropertyChanged);

        private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleDetailButton btn = (TitleDetailButton)bindable;
            btn.Text = $"{(string)newValue}\n{btn.Detail}";
        }

        public static readonly BindableProperty DetailProperty = BindableProperty.Create(nameof(Detail),
            typeof(string), typeof(TitleDetailButton), string.Empty,
            propertyChanged: DetailPropertyChanged);
        private static void DetailPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleDetailButton btn = (TitleDetailButton)bindable;
            btn.Text = $"{btn.Title}\n{(string)newValue}";
        }

        public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(nameof(Detail),
typeof(int), typeof(TitleDetailButton), -1,
propertyChanged: CurrentStatePropertyChanged);
        private static void CurrentStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleDetailButton frame = (TitleDetailButton)bindable;
            frame.IsVisible = (int)newValue == frame.VisibleState;
        }

        public static readonly BindableProperty VisibleStateProperty = BindableProperty.Create(nameof(Detail),
typeof(int), typeof(TitleDetailButton), -1);


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
