using CheApp.Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

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
            Canvas = new SKCanvasView()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0)                
            };
            Canvas.SetBinding(SKCanvasView.HeightProperty, new Binding("Height", source: headerLabel));
            Canvas.PaintSurface += Canvas_PaintSurface;

            HeaderLayout = new ClickableContentView()
            {
                /*FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.Black*/
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        headerLabel,
                        Canvas
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

        private SKCanvasView Canvas;
        private readonly float ArrowHorizontalScale = (float)0.75;
        private readonly float ArrowVerticalScale = (float)0.5;

        private void Canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            // we get the current surface from the event args
            SKSurface surface = e.Surface;
            // then we get the canvas that we can draw on
            SKCanvas canvas = surface.Canvas;
            SKPaint circleFill = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = Color.Gray.ToSKColor(),
                StrokeWidth = 2
            };
            canvas.Clear();
            SKPoint midpoint = new SKPoint((float)Canvas.Width / 2, (float)Canvas.Height / 2);
            if (IsCollapsed)
            {
                SKPoint arrowHead = new SKPoint(midpoint.X, midpoint.Y + midpoint.Y * ArrowVerticalScale);
                float yPoint = midpoint.Y - midpoint.Y * ArrowVerticalScale;
                canvas.DrawLine(new SKPoint(midpoint.X - midpoint.X * ArrowHorizontalScale, yPoint), arrowHead, circleFill);
                canvas.DrawLine(arrowHead, new SKPoint(midpoint.X + midpoint.X * ArrowHorizontalScale, yPoint), circleFill);
            }
            else
            {
                SKPoint arrowHead = new SKPoint(midpoint.X, midpoint.Y - midpoint.Y * ArrowVerticalScale);
                float yPoint = midpoint.Y + midpoint.Y * ArrowVerticalScale;
                canvas.DrawLine(new SKPoint(midpoint.X - midpoint.X * ArrowHorizontalScale, yPoint), arrowHead, circleFill);
                canvas.DrawLine(arrowHead, new SKPoint(midpoint.X + midpoint.X * ArrowHorizontalScale, yPoint), circleFill);
            }
            

        }

        private void ToggleVisibilityButton_Clicked(object sender, EventArgs e)
        {
            IsCollapsed = !IsCollapsed;
            this.InvalidateLayout();
            Canvas.InvalidateSurface();
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
            }
        }

        private ClickableContentView HeaderLayout { get; set; }
        /// <summary>
        /// true with the cell is collapsed
        /// </summary>
        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, value); }
        }

        public double HeaderFontSize
        {
            get { return (double)GetValue(HeaderFontSizeProperty); }
            set { SetValue(HeaderFontSizeProperty, value); }
        }
    }
}
