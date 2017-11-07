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
    public class OrificePlate : BasicPage
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






        public OrificePlate()
        {

            inputFieldData = new FieldBindData[]
            {
                new FieldBindData((int)Inputs.disCo, "Discharge Coefficient", new AbstractUnit[] { Unitless.unitless }),
                new FieldBindData((int)Inputs.density, "Density", new AbstractUnit[] { Density.kgm3 }),
                new FieldBindData((int)Inputs.pDia, "Inlet Pipe Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Inputs.oDia, "Orifice Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Inputs.deltaP, "Drop in Pressure (pIn - pOut) Across Orifice Plate", new AbstractUnit[] { Pressure.Pa })
            };



            outputFieldData = new FieldBindData[]
            {
                new FieldBindData((int)Outputs.volFlow, "Volumetric Flow Rate", new AbstractUnit[] { Volume.m3, Time.sec })
            };

            this.PageSetup();
#if DEBUG            
            inputFields[0].EntryText = "1";
            inputFields[1].EntryText = "1000";
            inputFields[2].EntryText = "10";
            inputFields[3].EntryText = "8";
            inputFields[4].EntryText = "10";            
#endif
            outputFields[(int)Outputs.volFlow].SetFinalResult(0.0);
        }


        protected override void CalculateButtonClicked(object sender, EventArgs e)
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
            catch (System.FormatException)
            {
                this.DisplayAlert("ERROR!", "All inputs must be a type of number!", "OK!");
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