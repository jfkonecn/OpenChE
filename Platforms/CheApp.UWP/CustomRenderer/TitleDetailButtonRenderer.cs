using CheApp.Controls;
using CheApp.UWP.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using XamCtrl = Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using WinCtrl = Windows.UI.Xaml.Controls;

[assembly: ExportRenderer(typeof(TitleDetailButton), typeof(TitleDetailButtonRenderer))]
namespace CheApp.UWP.CustomRenderer
{
    public class TitleDetailButtonRenderer : ButtonRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<XamCtrl.Button> e)
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
