using CheApp.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CheApp.CustomUI
{
    /// <summary>
    /// View Cell which displays a regular view when expanded
    /// When collapsed the the cell becomes just a label with specified placeholder text
    /// The collapsed state is toggled by tapping the cell
    /// </summary>
    public class CollapsibleView : ContentView
    {


        public CollapsibleView() : base()
        {
            ExpandedViewContainer = new ContentView()
            {
            };
            ExpandedViewContainer.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsCollapsed", source: this, converter: new BoolToNotBool()));
            
            Label headerLabel = new Label()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand                
            };
            headerLabel.SetBinding(Label.TextProperty, new Binding("Header", source: this));
            headerLabel.SetBinding(Label.FontSizeProperty, new Binding("HeaderFontSize", source: this));

            Label caretLabel = new Label()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.End
            };
            caretLabel.SetBinding(Label.TextProperty, new Binding("CaretArrow", source: this));
            caretLabel.SetBinding(Label.FontSizeProperty, new Binding("HeaderFontSize", source: this));
            
            HeaderLayout = new ClickableContentView()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        headerLabel,
                        caretLabel
                    }
                }
            };
            
            HeaderLayout.Clicked += ToggleVisibilityButton_Clicked;
            this.Content = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    HeaderLayout,
                    ExpandedViewContainer
                }
            };
        }



        private void ToggleVisibilityButton_Clicked(object sender, EventArgs e)
        {
            IsCollapsed = !IsCollapsed;
            this.InvalidateLayout();
            Task.WhenAll(Collapse());

        }

        private async Task Collapse()
        {

            await Task.WhenAll(
                ExpandedViewContainer.FadeTo(IsCollapsed ? 0 : 1, 1000, Easing.SinOut));
        }

        public static readonly BindableProperty HeaderProperty =
BindableProperty.Create(nameof(Header), typeof(string), typeof(CollapsibleView), "");

        public static readonly BindableProperty HeaderFontSizeProperty =
BindableProperty.Create(nameof(HeaderFontSize), typeof(double), typeof(CollapsibleView), Device.GetNamedSize(NamedSize.Small, typeof(Label)));

        public static readonly BindableProperty IsCollapsedProperty =
BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(CollapsibleView), true);

        public static readonly BindableProperty ExpandedViewProperty =
BindableProperty.Create(nameof(ExpandedView), typeof(View), typeof(CollapsibleView), new StackLayout());

        /// <summary>
        /// The text displayed when the cell is both expanded and contracted
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private ContentView ExpandedViewContainer { get; set; }
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
                OnPropertyChanged(nameof(CaretArrow));
            }
        }

        private ClickableContentView HeaderLayout { get; set; }
        /// <summary>
        /// true with the cell is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set
            {
                SetValue(IsCollapsedProperty, value);
                OnPropertyChanged("CaretArrow");
            }
        }

        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }

        public string CaretArrow
        {
            get
            {
                if (IsCollapsed)
                {
                    return ("▼");
                }
                else
                {
                    return ("▲");
                }
            }
        }
    }
}
