using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Units;
using EngineeringMath.Resources;
using System.Diagnostics;
using EngineeringMath.Calculations.Components.Functions;
using EngineeringMath.Calculations.Components.Selectors;
using EngineeringMath.Calculations.Components.Commands;

namespace EngineeringMath.Calculations.Components.Parameter
{
    /// <summary>
    /// Allows for a link to a function which can calculate this parameter
    /// </summary>
    public class SubFunctionParameter : SimpleParameter
    {

        /// <param name="title">Title of the field</param>
        /// <param name="ID">Id of the parameter</param>
        /// <param name="subFunctions">The functions which this parameter may be replaced by the string key is the title which is intended to be stored in a picker
        /// <param>EX: An area parameter can replaced by a function which calculates the area of a circle</param>
        /// </param>
        /// <param name="lowerLimit">The lowest number the parameter is allowed to be</param>
        /// <param name="upperLimit">The highest number the paramter is allowed to be</param>
        /// <param name="desiredUnits">Used to create a conversion factor</param>
        /// <param name="isInput">If this parameter(else it's an output)</param>
        internal SubFunctionParameter(int ID, string title, AbstractUnit[] desiredUnits,
            Dictionary<string, ComponentFactory.SolveForFactoryData> subFunctions,
            bool isInput = true,
            double lowerLimit = double.MinValue,
            double upperLimit = double.MaxValue) :
            base(ID, title, desiredUnits, isInput, lowerLimit, upperLimit)
        {
            // Build SubFunctionSelection
            // add a default selection
            Dictionary<string, Type> temp = new Dictionary<string, Type>
                {
                    { LibraryResources.DirectInput, null }
                };

            foreach (KeyValuePair<string, ComponentFactory.SolveForFactoryData> ele in subFunctions)
                temp.Add(ele.Key, ele.Value.FunType);

            FunTypeToOutputID = subFunctions.ToDictionary(x => x.Value.FunType, x => x.Value.OuputID);

            this.SubFunctionSelection = new FunctionPicker(temp);
            this.SubFunctionSelection.OnFunctionCreatedEvent += SyncSubFunctionWithParameter;
            this.SubFunctionSelection.OnSelectedIndexChanged += SubFunctionSelection_OnSelectedIndexChanged;
            this.SubFunctionSelection.SelectedObject = null;

            SubFunctionButton = new ButtonComponent(
            (object parameter) => { return; },
            (object parameter) => { return AllowSubFunctionClick; })
            {
                Title = LibraryResources.SubFunction
            };
        }

        private void SubFunctionSelection_OnSelectedIndexChanged(object sender, EventArgs args)
        {
            OnPropertyChanged("AllowUserInput");
            OnPropertyChanged("AllowSubFunctionClick");
        }

        /// <summary>
        /// Relates function types to output ID's
        /// </summary>
        private readonly Dictionary<Type, int> FunTypeToOutputID;

        private ButtonComponent _SubFunctionButton = null;
        public ButtonComponent SubFunctionButton
        {
            get
            {
                return _SubFunctionButton;
            }
            set
            {
                _SubFunctionButton = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public override bool AllowUserInput
        {
            get
            {
                return IsInput && SubFunctionSelection.SelectedObject == null;
            }
        }

        public override bool IsInput {
            get => base.IsInput;
            set
            {
                base.IsInput = value;
                OnPropertyChanged("AllowSubFunctionClick");
                if(SubFunctionSelection != null)
                    SubFunctionSelection.IsEnabled = IsInput;
            }
        }

        /// <summary>
        /// True when user input is allowed
        /// </summary>
        public bool AllowSubFunctionClick
        {
            get
            {
                return SubFunctionSelection.SelectedObject != null && IsInput;
            }
        }


        private FunctionPicker _SubFunctionSelection = null;
        /// <summary>
        /// Contains all of the functions which will be allowed to substituted out all the fields within the function (intended to binded with a picker
        /// </summary>
        public FunctionPicker SubFunctionSelection
        {
            get
            {
                return _SubFunctionSelection;
            }
            set
            {
                _SubFunctionSelection = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public SimpleFunction SubFunction
        {
            get
            {
                return SubFunctionSelection.SubFunction;
            }
        }

        /// <summary>
        /// Syncs the subfunction with its parameter
        /// </summary>
        private void SyncSubFunctionWithParameter(object sender, EventArgs args)
        {

            int outputID = FunTypeToOutputID[SubFunctionSelection.SelectedObject];
            SimpleParameter outputPara = SubFunction.GetParameter(outputID);
            // make sure that the output parameter can be an output
            if (SubFunction as SimpleFunction != null && outputPara.IsInput)
            {
                throw new Exception("The subfunction must use the desired parameter as an output!");
            }
            // double check that that the parameter is the type of unit as the desired output of the subfunction
            for (int i = 0; i < outputPara.UnitSelection.Count; i++)
            {
                if (this.UnitSelection[i].GetType() != outputPara.UnitSelection[i].GetType())
                {
                    throw new Exception("Output parameter in subfunction does not match this parameter's units");
                }
            }


            for (int i = 0; i < SubFunction.GetParameter(outputID).UnitSelection.Count; i++)
            {

                SubFunction.GetParameter(outputID).UnitSelection[i].SelectedObject =
                    this.UnitSelection[i].SelectedObject;
            }

            SubFunction.GetParameter(outputID).ValueStr = this.ValueStr;
            SubFunction.Title = this.Title;


            if (SubFunction as SolveForFunction != null)
            {
                // dont allow user to be able to change the output function
                ((SolveForFunction)SubFunction).OutputSelection.SelectedObject = outputPara;
                ((SolveForFunction)SubFunction).OutputSelection.IsEnabled = false;
            }


            SubFunction.OnSolve += delegate ()
            {
                for (int i = 0; i < SubFunction.GetParameter(outputID).UnitSelection.Count; i++)
                {
                    this.UnitSelection[i].SelectedObject =
                    SubFunction.GetParameter(outputID).UnitSelection[i].SelectedObject;
                }

                this.ValueStr = SubFunction.GetParameter(outputID).ValueStr;
            };
        }
    }
}
