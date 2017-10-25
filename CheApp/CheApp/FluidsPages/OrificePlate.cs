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

        private static readonly NumericInputField[] inputFields = {
                new NumericInputField("Mass", typeof(Mass)),
                new NumericInputField("Pressure", typeof(Pressure))};

        private static readonly NumericOutputField outputField = 
                new NumericOutputField("Mass", typeof(Mass));

        public OrificePlate()
        {
            BasicPage.BasicInputPage(this, inputFields, outputField);
        }
    }
}