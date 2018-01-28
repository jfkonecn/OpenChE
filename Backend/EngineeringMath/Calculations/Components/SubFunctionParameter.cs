using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using System.Diagnostics;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations.Components
{
    /// <summary>
    /// Allows for a link to a function which can calculate this parameter
    /// </summary>
    public class SubFunctionParameter : SimpleParameter
    {
        private static readonly string TAG = "SubFunctionParameter:";
        /// <param name="title">Title of the field</param>
        /// <param name="ID">Id of the parameter</param>
        /// <param name="subFunctions">The functions which this parameter may be replaced by the string key is the title which is intended to be stored in a picker
        /// <para>EX: An area parameter can replaced by a function which calculates the area of a circle</para>
        /// </param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal SubFunctionParameter(int ID, string title, AbstractUnit[] desiredUnits,
            Dictionary<string, FunctionFactory.FactoryData> subFunctions,
            bool isInput = true,
            double lowerLimit = double.MinValue,
            double upperLimit = double.MaxValue) :
            this(ID, title, new NumericField(title, desiredUnits, isInput, lowerLimit, upperLimit), subFunctions, isInput)
        {





        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="title"></param>
        /// <param name="field">The field attached to this object</param>
        /// <param name="subFunctions">The functions which this parameter may be replaced by the string key is the title which is intended to be stored in a picker
        /// <para>EX: An area parameter can replaced by a function which calculates the area of a circle</para>
        /// </param>
        /// <param name="isInput"></param>
        internal SubFunctionParameter(int ID, string title, NumericField field, 
            Dictionary<string, FunctionFactory.FactoryData> subFunctions, bool isInput = true) : 
            base(ID, title, field, isInput)
        {
            // Build SubFunctionSelection
            // add a default selection
            Dictionary<string, FunctionFactory.FactoryData> temp = new Dictionary<string, FunctionFactory.FactoryData>
                {
                    { LibraryResources.DirectInput, null }
                };

            foreach (KeyValuePair<string, FunctionFactory.FactoryData> ele in subFunctions)
                temp.Add(ele.Key, ele.Value);

            this.SubFunctionSelection = new FunctionDataPickerSelection(temp);
            this.SubFunctionSelection.OnFunctionCreatedEvent += SyncSubFunctionWithParameter;
            this.SubFunctionSelection.PropertyChanged += SubFunctionSelection_PropertyChanged;
            this.SubFunctionSelection.SelectedObject = null;
        }

        /// <summary>
        /// Called when a property is changed in the the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubFunctionSelection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public override bool AllowUserInput
        {
            get
            {
                return isInput && SubFunctionSelection.SelectedObject == null;
            }
        }

        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public bool AllowSubFunctionClick
        {
            get
            {
                return SubFunctionSelection.SelectedObject != null && isInput;
            }
        }

        /// <summary>
        /// Contains all of the functions which will be allowed to substituted out all the fields within the function (intended to binded with a picker
        /// </summary>
        public FunctionDataPickerSelection SubFunctionSelection;


        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public SolveForFunction SubFunction
        {
            get
            {
                return SubFunctionSelection.SubFunction;
            }
        }

        /// <summary>
        /// Syncs the subfunction with its parameter
        /// </summary>
        private void SyncSubFunctionWithParameter()
        {
            // double check that that the parameter is the type of unit as the desired output of the subfunction
            SimpleParameter outputPara = SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID);

            for (int i = 0; i < outputPara.UnitSelection.Length; i++)
            {
                if (this.UnitSelection[i].GetType() != outputPara.UnitSelection[i].GetType())
                {
                    throw new Exception("Output parameter in subfunction does not match this parameter's units");
                }
            }


            for (int i = 0; i < SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection.Length; i++)
            {

                SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection[i].SelectedObject =
                    this.UnitSelection[i].SelectedObject;
            }

            SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).ValueStr = this.ValueStr;
            SubFunction.Title = this.Title;

            // dont allow user to be able to change the output function
            SubFunction.OutputSelection.SelectedObject = outputPara;
            SubFunction.OutputSelection.IsEnabled = false;

            SubFunction.OnSolve += delegate ()
            {
                for (int i = 0; i < SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection.Length; i++)
                {
                    this.UnitSelection[i].SelectedObject =
                    SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection[i].SelectedObject;
                }

                this.ValueStr = SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).ValueStr;
            };
        }
    }
}
