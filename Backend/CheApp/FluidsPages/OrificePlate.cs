using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;

using Xamarin.Forms;

namespace CheApp.FluidsPages
{

    /// <summary>
    /// Performs orifice plate calculations
    /// </summary>
    public class OrificePlate : BasicPage
    {

        // TODO: put all of this into basic page class and inherit
        enum Field
        {
            disCo,
            density,
            pDia,
            oDia,
            deltaP,
            volFlow
        };






        public OrificePlate()
        {

            bindFieldData = new FieldBindData[]
            {
                new FieldBindData((int)Field.disCo, "Discharge Coefficient", new AbstractUnit[] { Unitless.unitless }),
                new FieldBindData((int)Field.density, "Density", new AbstractUnit[] { Density.kgm3 }),
                new FieldBindData((int)Field.pDia, "Inlet Pipe Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Field.oDia, "Orifice Diameter", new AbstractUnit[] { Length.m }),
                new FieldBindData((int)Field.deltaP, "Drop in Pressure (pIn - pOut) Across Orifice Plate", new AbstractUnit[] { Pressure.Pa }),
                new FieldBindData((int)Field.volFlow, "Volumetric Flow Rate", new AbstractUnit[] { Volume.m3, Time.sec })
            };


            this.PageSetup();
#if DEBUG            
            fields[0].EntryText = "1";
            fields[1].EntryText = "1000";
            fields[2].EntryText = "10";
            fields[3].EntryText = "8";
            fields[4].EntryText = "10";
#endif
            fields[5].SetFinalResult(0.0);
        }


        protected override void CalculateButtonClicked(object sender, EventArgs e)
        {




            try
            {
                double orfFlow = EngineeringMath.Calculations.Fluids.OrificePlate(
                    fieldsDic[(int)Field.disCo].GetUserInput(),
                    fieldsDic[(int)Field.density].GetUserInput(),
                    fieldsDic[(int)Field.pDia].GetUserInput(),
                    fieldsDic[(int)Field.oDia].GetUserInput(),
                    fieldsDic[(int)Field.deltaP].GetUserInput()
                );

                fieldsDic[(int)Field.volFlow].SetFinalResult(orfFlow);

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