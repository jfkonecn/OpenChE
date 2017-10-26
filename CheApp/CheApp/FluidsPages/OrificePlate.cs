using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.CalculationPage;
using CheApp.CheMath.Units;

using Xamarin.Forms;

namespace CheApp.FluidsPages
{

    /// <summary>
    /// Performs orifice plate calculations
    /// </summary>
    public class OrificePlate : ContentPage
    {

        private static readonly NumericInputField[] inputFields = 
        {
                new NumericInputField("Discharge coefficient", new Type[] {typeof(Unitless)}),
                new NumericInputField("Density", new Type[] {typeof(Density) }),
                new NumericInputField("Inlet Pipe Diameter", new Type[] {typeof(Length) }),
                new NumericInputField("Orifice Diameter", new Type[] {typeof(Length) }),
                new NumericInputField("Drop in Pressure (pIn - pOut) Across Orifice Plate", new Type[] {typeof(Pressure) })
        };

        private static readonly NumericOutputField[] outputFields = 
        {
                new NumericOutputField("Volumetric Flow Rate", new Type[] { typeof(Volume), typeof(Time) })
        };

        public OrificePlate()
        {
            BasicPage.BasicInputPage(this, inputFields, outputFields, CalculateButtonClicked);
#if DEBUG
            inputFields[0].EntryText = "10";
            inputFields[1].EntryText = "1000";
            inputFields[2].EntryText = "10";
            inputFields[3].EntryText = "8";
            inputFields[4].EntryText = "10";
#endif
        }

        // TODO: get calculate button up and running
        private void CalculateButtonClicked(object sender, EventArgs e)
        {
            this.DisplayAlert("", "", "Ok");
        }
    }
}