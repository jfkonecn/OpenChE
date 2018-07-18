using CheApp.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace CheApp.CustomUI
{
    /// <summary>
    /// View Cell which displays a regular view when expanded
    /// When collapsed the the cell becomes just a label with specified placeholder text
    /// The collapsed state is toggled by tapping the cell
    /// </summary>
    public class CollapsibleViewCell : ViewCell, INotifyPropertyChanged
    {


        public CollapsibleViewCell() : base()
        {
            ExpandedView = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };
            StackLayout localStackLayout = new StackLayout()
            {
                Children = { ExpandedView },
                IsVisible = false,
                BindingContext = this
            };
            ToggleVisibilityButton = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            localStackLayout.SetBinding(StackLayout.IsVisibleProperty, "IsCollapsed");
            this.Tapped += (object sender, EventArgs e) => { Debug.WriteLine($"IsVisible:{localStackLayout.IsVisible.ToString()}"); };
            this.View = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    ToggleVisibilityButton,
                    localStackLayout
                }
            };
        }

        public static readonly BindableProperty HeaderProperty =
BindableProperty.Create(nameof(Header), typeof(string), typeof(CollapsibleViewCell), "");

        public static readonly BindableProperty IsCollapsedProperty =
BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(CollapsibleViewCell), true);

        /// <summary>
        /// The text displayed when the cell is both expanded and contracted
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// The view when this view cell is expanded
        /// </summary>
        public StackLayout ExpandedView
        {
            get;
            private set;
        }

        public Button ToggleVisibilityButton { get; private set; }
        /// <summary>
        /// true with the cell is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }
    }
}
