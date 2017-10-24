using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.CalculationPage;
using CheApp.CheMath.Units;

using Xamarin.Forms;

namespace CheApp.FluidsPages
{
    /*
     * Help:
     https://developer.xamarin.com/api/type/Xamarin.Forms.Picker/
         
         */


    public class OrificePlate : ContentPage
    {

        private static readonly new NumericFieldData[] inputFields = {
                new NumericFieldData("Mass", typeof(Mass)),
                new NumericFieldData("Pressure", typeof(Pressure))};

    public OrificePlate()
        {
            BasicPage.BasicInputPage(this, inputFields);
        }
    }
}