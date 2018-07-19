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
    public class CollapsibleViewCell : ContentView
    {


        public CollapsibleViewCell() : base()
        {
            ExpandedViewContainer = new ContentView()
            {
                IsVisible = true
            };
            ExpandedView = new StackLayout()
            {
                Orientation = StackOrientation.Vertical
            };

            ToggleVisibilityButton = new Button()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };
            //ExpandedViewContainer.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsCollapsed", source: this));
            //this.Tapped += (object sender, EventArgs e) => { Debug.WriteLine($"IsVisible:{localStackLayout.IsVisible.ToString()}"); };
            this.Content = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    ToggleVisibilityButton,
                    ExpandedViewContainer
                }
            };
        }

        public static readonly BindableProperty HeaderProperty =
BindableProperty.Create(nameof(Header), typeof(string), typeof(CollapsibleViewCell), "");

        public static readonly BindableProperty IsCollapsedProperty =
BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(CollapsibleViewCell), false);

        public static readonly BindableProperty ExpandedViewProperty =
BindableProperty.Create(nameof(ExpandedView), typeof(View), typeof(CollapsibleViewCell), new StackLayout());

        /// <summary>
        /// The text displayed when the cell is both expanded and contracted
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public ContentView ExpandedViewContainer { get; set; }
        /// <summary>
        /// The view when this view cell is expanded
        /// </summary>
        public View ExpandedView
        {
            get { return (View)GetValue(ExpandedViewProperty); }
            set
            {
                SetValue(ExpandedViewProperty, value);
                ExpandedViewContainer.Content = ExpandedView;
            }
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
