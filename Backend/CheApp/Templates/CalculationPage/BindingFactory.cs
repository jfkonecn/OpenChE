using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using EngineeringMath.Calculations;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Contains static functions which bind EngineeringMath objects to Xamarin.Forms objects
    /// </summary>
    internal class BindingFactory
    {
        // this is an example of how to bind to an array
        // Not needed here, but was used at one point
        // allPickers[i].SetBinding(Picker.ItemsSourceProperty, new Binding($"PickerStrings[{i}]"));

        /// <summary>
        /// Class which contains binds for objects which contain generic objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        internal class GenericBindings<T>
        {
            /// <summary>
            /// Binds a picker selection object to a picker
            /// </summary>
            /// <param name="obj"></param>
            /// <returns>A picker binded to the PickerSelection object</returns>
            internal static Picker PickerFactory(ref PickerSelection<T> obj)
            {

                Picker picker = new Picker();

                // bind it up!
                picker.SetBinding(Picker.ItemsSourceProperty, new Binding("PickerList"));
                picker.SetBinding(Picker.SelectedIndexProperty, new Binding("SelectedIndex"));
                picker.SetBinding(Picker.IsEnabledProperty, new Binding("IsEnabled"));
                picker.BindingContext = obj;

                return picker;
            }



        }

        /// <summary>
        /// Binds a button to a parameter to handle using a function in the place of the parameter
        /// </summary>
        /// <param name="page">Current Page</param>
        /// <param name="para">Parameter to be binded to</param>
        /// <returns></returns>
        internal static Button CreateSubFunctionButton(ContentPage page, Parameter para)
        {
            Button subFunctionButton = new Button
            {
                Text = "Subfunction"
            };

            subFunctionButton.Clicked += async delegate (System.Object o, System.EventArgs e)
            {

                if(para.SubFunctionSelection.SelectedObject != null)
                {
                    BasicPage calPage = new BasicPage(FunctionFactory.BuildFunction(para.SubFunctionSelection.SelectedObject.FunType));
                    await page.Navigation.PushAsync(calPage);
                }
 
            };
            return subFunctionButton;
        }


    }
}
