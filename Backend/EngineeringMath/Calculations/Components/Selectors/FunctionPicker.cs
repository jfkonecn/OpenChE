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
        }


        private static readonly string TAG = "FunctionDataPickerSelection";


        /// <summary>
        /// 
        /// </summary>
        private void SubFunctionSelection_OnSelectedIndexChanged()
        {
            // free the memory being used
            _SubFunction = null;
        }

        public override Type CastAs()
        {
            return typeof(FunctionPicker);
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
                if (this.SelectedObject == null)
                {
                    _SubFunction = null;
                }
                else
                {
                    // Do not rebuild if right function is already in place
                    if (_SubFunction == null || !this.SelectedObject.Equals(_SubFunction.GetType()))
                    {
                        Debug.WriteLine($"{TAG} Building a new SubFunction");
                        _SubFunction = FunctionFactory.BuildFunction(this.SelectedObject);
                        OnFunctionCreated();
                    }
                    else
                    {
                        Debug.WriteLine($"{TAG} Kept old SubFunction");
                    }
                }

                return _SubFunction;
            }
        }


        /// <summary>
        /// On function created handler
        /// </summary>
        public delegate void OnFunctionCreatedEventHandler();

        /// <summary>
        /// Called when a new subfunction is object is created
        /// </summary>
        public event OnSuccessEventHandler OnFunctionCreatedEvent;

        /// <summary>
        /// Call on success event handler
        /// </summary>
        internal virtual void OnFunctionCreated()
        {
            OnFunctionCreatedEvent?.Invoke();
        }
    }
}
