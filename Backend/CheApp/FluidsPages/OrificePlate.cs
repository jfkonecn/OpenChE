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

        // TODO: put all of this into basic page class and inherit
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


        private static readonly FieldBindData[] inputFieldData =
        {
                new FieldBindData((int)Inputs.disCo, "Discharge Coefficient", new AbstractUnit[] { Unitless.unitless }),
                new FieldBindData((int)Inputs.density, "Density", new AbstractUnit[] { Density.kgm3 }),
                new FieldBindData((int)Inputs.pDia, "Inlet Pipe Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Inputs.oDia, "Orifice Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Inputs.deltaP, "Drop in Pressure (pIn - pOut) Across Orifice Plate", new AbstractUnit[] { Pressure.Pa })
        };

        private static readonly NumericInputField[] inputFields = inputFieldData.Select(item => new NumericInputField(ref item)).ToArray();

        private static readonly Dictionary<int, NumericInputField> inputFieldsDic = inputFields.ToDictionary(item => item.ID, item => item);


        private static readonly FieldBindData[] outputFieldData =
        {
                 new FieldBindData((int)Outputs.volFlow, "Volumetric Flow Rate", new AbstractUnit[] { Volume.m3,  Time.sec })
        };


        private static readonly NumericOutputField[] outputFields = outputFieldData.Select(item => new NumericOutputField(ref item)).ToArray();

        private static readonly Dictionary<int, NumericOutputField> outputFieldsDic = outputFields.ToDictionary(item => item.ID, item => item);


        public OrificePlate()
        {
            this.Content = BasicPage.BasicInputPage(inputFields, outputFields, CalculateButtonClicked);
#if DEBUG            
            inputFields[0].EntryText = "1";
            inputFields[1].EntryText = "1000";
            inputFields[2].EntryText = "10";
            inputFields[3].EntryText = "8";
            inputFields[4].EntryText = "10";            
#endif
            outputFields[(int)Outputs.volFlow].SetFinalResult(0.0);
        }


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
            catch(OverflowException)
            {
                this.DisplayAlert("ERROR!", $"The result is greater than {double.MaxValue}!", "OK!");
            }
            catch(Exception err)
            {
                this.DisplayAlert("ERROR!", 
                    string.Format("Unexpected exception of type {0} caught: {1}", err.GetType(), err.Message), 
                    "OK");
            }
            
        }
    }
}