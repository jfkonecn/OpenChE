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
        // TODO: make it so that the user can choose both the volume and the time unit when inputing a volumetric flow rate
        private static readonly NumericInputField[] inputFields = {
                new NumericInputField("Discharge coefficient", typeof(Unitless)),
                new NumericInputField("Density", typeof(Density)),
                new NumericInputField("Inlet Pipe Diameter", typeof(Length)),
                new NumericInputField("Orifice Diameter", typeof(Length)),
                new NumericInputField("Drop in Pressure (pIn - pOut) Across Orifice Plate", typeof(Pressure))
        };

        // TODO: Add multiple outputs so that you can have different types of flow rates (i.e. mass and volume)
        private static readonly NumericOutputField outputField = 
                new NumericOutputField("Mass", typeof(Mass));

        public OrificePlate()
        {
            BasicPage.BasicInputPage(this, inputFields, outputField);
        }
    }
}