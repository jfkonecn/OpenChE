using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations.Components.Selectors
{
    /// <summary>
    /// Creates a PickerSelection object to handle FunctionData
    /// </summary>
    public class FunctionPicker : SimplePicker<Type>
    {
        internal FunctionPicker(Dictionary<string, Type> funData) : base(funData)
        {
            this.OnSelectedIndexChanged += SubFunctionSelection_OnSelectedIndexChanged;
            BuildSubFunction();
        }


        private static readonly string TAG = "FunctionDataPickerSelection";


        /// <summary>
        /// 
        /// </summary>
        private void SubFunctionSelection_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            BuildSubFunction();
        }

        /// <summary>
        /// Builds the SubFunctions
        /// </summary>
        private void BuildSubFunction()
        {
            if (this.SelectedObject == null)
            {
                SubFunction = null;
                OnPropertyChanged("SubFunction");
            }
            else
            {
                // Do not rebuild if right function is already in place
                if (SubFunction == null || !this.SelectedObject.Equals(SubFunction.GetType()))
                {
                    Debug.WriteLine($"{TAG} Building a new SubFunction");
                    SubFunction = FunctionConstructor();
                    OnFunctionCreated();
                }
                else
                {
                    Debug.WriteLine($"{TAG} Kept old SubFunction");
                }
            }
        }

        protected virtual SimpleFunction FunctionConstructor()
        {
            return ComponentFactory.BuildComponent(this.SelectedObject) as SimpleFunction;
        }

        private SimpleFunction _SubFunction;
        /// <summary>
        /// This is the function which is currently replacing this parameter
        /// It is null when no function is replacing it.
        /// </summary>
        public SimpleFunction SubFunction
        {
            get
            {
                return _SubFunction;
            }
            private set
            {
                _SubFunction = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// On function created handler
        /// </summary>
        public delegate void OnFunctionCreatedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Called when a new subfunction is object is created
        /// </summary>
        public event OnFunctionCreatedEventHandler OnFunctionCreatedEvent;

        /// <summary>
        /// Call on success event handler
        /// </summary>
        internal virtual void OnFunctionCreated(object sender, EventArgs e)
        {
            OnFunctionCreatedEvent?.Invoke(sender, e);
            OnPropertyChanged("SubFunction");
        }

        internal virtual void OnFunctionCreated(EventArgs e = null)
        {
            OnFunctionCreated(this, e);
        }
    }
}
