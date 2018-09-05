using CheApp.Controls;
using CheApp.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(TitleDetailButtonRenderer))]
namespace CheApp.UWP.CustomRenderer
{
    public class TitleDetailButtonRenderer : ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            
            if (Control?.Content is string text)
            {
                var textBlock = new TextBlock
                {
                    Text = text,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = Windows.UI.Xaml.TextAlignment.Center,
                    TextWrapping = TextWrapping.WrapWholeWords
                };
                Control.Content = textBlock;
            }
        }
    }
}
