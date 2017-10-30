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
        enum Inputs
        {
            disCo,
            density,
            pDia,
            oDia,
            deltaP
        };

        enum Outputs
        {
            volFlow
        };

        private static readonly NumericInputField[] inputFields = 
        {
                new NumericInputField((int)Inputs.disCo, "Discharge Coefficient", new AbstractUnit[] { Unitless.unitless }),
                new NumericInputField((int)Inputs.density, "Density", new AbstractUnit[] { Density.kgm3 }),
                new NumericInputField((int)Inputs.pDia, "Inlet Pipe Diameter", new AbstractUnit[] { Length.m }),
                new NumericInputField((int)Inputs.oDia, "Orifice Diameter", new AbstractUnit[] { Length.m }),
                new NumericInputField((int)Inputs.deltaP, "Drop in Pressure (pIn - pOut) Across Orifice Plate", new AbstractUnit[] { Pressure.Pa })
        };

        private static readonly Dictionary<int, NumericInputField> inputFieldsDic = inputFields.ToDictionary(item => item.ID, item => item);

        private static readonly NumericOutputField[] outputFields = 
        {
                new NumericOutputField((int)Outputs.volFlow, "Volumetric Flow Rate", new AbstractUnit[] { Volume.m3,  Time.sec })
        };

        private static readonly Dictionary<int, NumericOutputField> outputFieldsDic = outputFields.ToDictionary(item => item.ID, item => item);

        public OrificePlate()
        {
            BasicPage.BasicInputPage(this, inputFields, outputFields, CalculateButtonClicked);
#if DEBUG
            inputFields[0].EntryText = "1";
            inputFields[1].EntryText = "1000";
            inputFields[2].EntryText = "10";
            inputFields[3].EntryText = "8";
            inputFields[4].EntryText = "10";
#endif
            outputFields[(int)Outputs.volFlow].SetFinalResult(0.0);
        }

        // TODO: get calculate button up and running
        private void CalculateButtonClicked(object sender, EventArgs e)
        {

            


            try
            {
                double orfFlow = CheMath.Calculations.Fluids.OrificePlate(
    inputFieldsDic[(int)Inputs.disCo].GetUserInput(),
    inputFieldsDic[(int)Inputs.density].GetUserInput(),
    inputFieldsDic[(int)Inputs.pDia].GetUserInput(),
    inputFieldsDic[(int)Inputs.oDia].GetUserInput(),
    inputFieldsDic[(int)Inputs.deltaP].GetUserInput()
    );

                outputFields[(int)Outputs.volFlow].SetFinalResult(orfFlow);

            }
            catch
            {
                this.DisplayAlert("Invalid Input", "All input must be numbers!", "OK");
            }
        }
    }
}