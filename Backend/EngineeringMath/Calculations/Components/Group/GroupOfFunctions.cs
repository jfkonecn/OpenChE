using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringMath.Calculations.Components.Functions;

namespace EngineeringMath.Calculations.Components.Group
{
    /// <summary>
    /// Makes user pick from a group of functions to use
    /// </summary>
    public class GroupOfFunctions : AbstractComponent, IEnumerable
    {

        internal GroupOfFunctions(Dictionary<string, FunctionFactory.FactoryData> funData)
        {
            this.FunctionSelection = new FunctionDataPickerSelection(funData);
            this.FunctionSelection.PropertyChanged += FunctionSelection_PropertyChanged;
            this.FunctionSelection.SelectedIndex = 0;
        }

        /// <summary>
        /// Called when a property is changed in the the field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FunctionSelection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e.PropertyName);
        }

        string _Title;
        /// <summary>
        /// Title of the group of functions
        /// </summary>
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }

        }

        /// <summary>
        /// Stores the function the user selected (intended to be binded with a picker)
        /// </summary>
        public FunctionDataPickerSelection FunctionSelection;



        public override Type CastAs()
        {
            return typeof(GroupOfFunctions);
        }

        /// <summary>
        /// Iterate through each abstract components in this collection in order
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            yield return FunctionSelection;
        }
    }
}
