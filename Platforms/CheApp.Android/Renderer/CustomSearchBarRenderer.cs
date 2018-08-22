using Android.Content;
using Android.OS;
using App.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace App.Droid
{
    /// <summary>
    /// Workaround for searchBar not appearing on Android >= 7
    /// William-H-M
    /// https://stackoverflow.com/questions/49274908/xamarin-search-bar-not-showing
    /// </summary>
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                Element.HeightRequest = 42;
            }
        }
    }
}