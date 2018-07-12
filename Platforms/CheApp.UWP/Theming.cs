using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheApp.Theming;

namespace CheApp
{
    internal partial class Theming
    {
        public static void SetTheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Light:
                    //UWP.App.Current.RequestedTheme = Windows.UI.Xaml.ApplicationTheme.Light;
                    break;
                case Theme.Dark:
                    
                    break;
                default:
                    break;
            }
        }
    }
}
