using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheApp.Templates.CalculationPage;
using EngineeringMath.Units;
using EngineeringMath.Calculations;

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
            // stores all inputs from the user
            Dictionary<Field, double> allUserInputs = new Dictionary<Field, double>();
            Field outputField = Field.volFlow;


            try
            {
                foreach (NumericFieldData obj in fieldsDic.Values)
                {
                    if (obj.BindedObject.isOutput)
                    {
                        outputField = (Field)obj.ID;
                        allUserInputs.Add((Field)obj.ID, 0.0);
                    }
                    else
                    {
                        // extract all user data inside of try block just in case there is an invalid input
                        allUserInputs.Add((Field)obj.ID, fieldsDic[obj.ID].GetUserInput());
                    }
                }
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




            if (outputField.Equals(Field.volFlow))
            {
                fieldsDic[(int)Field.volFlow].SetFinalResult(EngineeringMath.Calculations.Fluids.OrificePlate(
                   allUserInputs[Field.disCo],
                   allUserInputs[Field.density],
                   allUserInputs[Field.pDia],
                   allUserInputs[Field.oDia],
                   allUserInputs[Field.deltaP])
                );
            }
            else
            {
                Solver.MyFunction fun = solver_function(outputField, allUserInputs);
                fieldsDic[(int)outputField].SetFinalResult(Solver.NewtonsMethod(allUserInputs[outputField], fun));
            }
            solveOutput;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void solveOutput(Field outputField, Dictionary<Field, double> allUserInputs)
        {

            double finalOutput = 0;
            Solver.MyFunction fun;
            switch (outputField)
            {
                case Field.disCo:
                    fun = delegate (double x)
                    {
                        return EngineeringMath.Calculations.Fluids.OrificePlate(
                            x,
                            allUserInputs[Field.density],
                            allUserInputs[Field.pDia],
                            allUserInputs[Field.oDia],
                            allUserInputs[Field.deltaP]
                        );
                    };
                    finalOutput = Solver.NewtonsMethod(allUserInputs[outputField], fun, minValueDbl: 0);
                    break;
                case Field.density:
                    fun = delegate (double x)
                    {
                        return EngineeringMath.Calculations.Fluids.OrificePlate(
                            allUserInputs[Field.disCo],
                            x,
                            allUserInputs[Field.pDia],
                            allUserInputs[Field.oDia],
                            allUserInputs[Field.deltaP]
                        );
                    };
                    finalOutput = Solver.NewtonsMethod(allUserInputs[outputField], fun);
                    break;
                case Field.pDia:
                    fun = delegate (double x)
                    {

                        return EngineeringMath.Calculations.Fluids.OrificePlate(
                            allUserInputs[Field.disCo],
                            allUserInputs[Field.density],
                            x,
                            allUserInputs[Field.oDia],
                            allUserInputs[Field.deltaP]
                        );
                    };
                    finalOutput = Solver.NewtonsMethod(allUserInputs[outputField], fun);
                    break;
                case Field.oDia:
                    fun = delegate (double x)
                    {
                        return EngineeringMath.Calculations.Fluids.OrificePlate(
                            allUserInputs[Field.disCo],
                            allUserInputs[Field.density],
                            allUserInputs[Field.pDia],
                            x,
                            allUserInputs[Field.deltaP]
                        );
                    };
                    finalOutput = Solver.NewtonsMethod(allUserInputs[outputField], fun);
                    break;
                case Field.deltaP:
                    fun = delegate (double x)
                    {
                        return EngineeringMath.Calculations.Fluids.OrificePlate(
                            allUserInputs[Field.disCo],
                            allUserInputs[Field.density],
                            allUserInputs[Field.pDia],
                            allUserInputs[Field.oDia],
                            x
                        );
                    };
                    finalOutput = Solver.NewtonsMethod(allUserInputs[outputField], fun);
                    break;
                case Field.volFlow:
                    finalOutput = EngineeringMath.Calculations.Fluids.OrificePlate(
                        allUserInputs[Field.disCo],
                        allUserInputs[Field.density],
                        allUserInputs[Field.pDia],
                        allUserInputs[Field.oDia],
                        allUserInputs[Field.deltaP]
                    );
                    break;
                default:
                    throw new NotImplementedException("That field is not included in this function");
            }

            fieldsDic[(int)outputField].SetFinalResult(finalOutput);
        }
    }
}