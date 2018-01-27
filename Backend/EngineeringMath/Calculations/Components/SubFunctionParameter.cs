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

            this.SubFunctionSelection = new PickerSelection<FunctionFactory.FactoryData>(temp);
            this.SubFunctionSelection.OnSelectedIndexChanged += SubFunctionSelection_OnSelectedIndexChanged;
            this.SubFunctionSelection.SelectedObject = null;
        }


        /// <summary>
        /// 
        /// </summary>
        private void SubFunctionSelection_OnSelectedIndexChanged()
        {
            // free the memory being used
            _SubFunction = null;
            OnPropertyChanged("AllowUserInput");
            OnPropertyChanged("AllowSubFunctionClick");
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
        public PickerSelection<FunctionFactory.FactoryData> SubFunctionSelection;


        private SolveForFunction _SubFunction;
        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public SolveForFunction SubFunction
        {
            get
            {
                if (SubFunctionSelection.SelectedObject == null)
                {
                    _SubFunction = null;
                }
                else
                {
                    // Do not rebuild if right function is already in place
                    if (_SubFunction == null || !SubFunctionSelection.SelectedObject.FunType.Equals(_SubFunction.GetType()))
                    {
                        Debug.WriteLine($"{TAG} Building a new SubFunction");
                        _SubFunction = (SolveForFunction)FunctionFactory.BuildFunction(SubFunctionSelection.SelectedObject.FunType);

                        // double check that that the parameter is the type of unit as the desired output of the subfunction
                        SimpleParameter outputPara = _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID);

                        for (int i = 0; i < outputPara.UnitSelection.Length; i++)
                        {
                            if (this.UnitSelection[i].GetType() != outputPara.UnitSelection[i].GetType())
                            {
                                throw new Exception("Output parameter in subfunction does not match this parameter's units");
                            }
                        }


                        for (int i = 0; i < _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection.Length; i++)
                        {

                            _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection[i].SelectedObject =
                                this.UnitSelection[i].SelectedObject;
                        }

                        _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).ValueStr = this.ValueStr;
                        _SubFunction.Title = this.Title;

                        // dont allow user to be able to change the output function
                        _SubFunction.OutputSelection.SelectedObject = outputPara;
                        _SubFunction.OutputSelection.IsEnabled = false;

                        _SubFunction.OnSolve += delegate ()
                        {
                            for (int i = 0; i < _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection.Length; i++)
                            {
                                this.UnitSelection[i].SelectedObject =
                                _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).UnitSelection[i].SelectedObject;
                            }

                            this.ValueStr = _SubFunction.GetParameter(SubFunctionSelection.SelectedObject.OuputID).ValueStr;
                        };


                    }
                    else
                    {
                        Debug.WriteLine($"{TAG} Kept old SubFunction");
                    }
                }

                return _SubFunction;
            }
        }
    }
}
