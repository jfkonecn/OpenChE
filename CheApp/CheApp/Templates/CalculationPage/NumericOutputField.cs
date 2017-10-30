using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    public class NumericOutputField : NumericFieldData
    {
        /// <summary>
        /// Handles results of internal calculations
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="unitType">If there is 2 elements, then the format will be treated as element 1 per element 2. </param>
        /// <param name="resultUnits">The units which the results of the internal calculations will be in</param>
        public NumericOutputField(int id, string title, CheMath.Units.AbstractUnit[] resultUnits) : base(id, title, resultUnits) {}


        private Label _Label;



        public class InputBindObject : INotifyPropertyChanged
        {


            string _LabelText;
            /// <summary>
            /// Contains the user entered text
            /// </summary>
            public string LabelText
            {
                get
                {
                    return _LabelText;
                }
                set
                {
                    _LabelText = value;
                    OnPropertyChanged("LabelText");
                }
            }


            protected virtual void OnPropertyChanged(string property)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

            #region INotifyPropertyChanged Members

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion

        }





        /// <summary>
        /// Object which will bind to the page
        /// </summary>
        public InputBindObject BindingObject { get; set; }

        private string _LabelText = "0.0";


        // TODO: not good for temperature... Try and find a way to use the convertTo function instead
        /// <summary>
        /// Updates the label with the value of 
        /// </summary>
        /// <param name="finalResult">Result of internal calculation assumed to be in the same units as "resultUnits" specificed in the constructor</param>
        internal void SetFinalResult(double finalResult)
        {
            // WE ARE ASSUMING THAT A MAX OF 2 ELEMENTS WILL BE IN THE ARRAY
            BindingObject.LabelText = CheMath.Units.HelperFunctions.ConvertFrom(
                finalResult,
                ConvertionUnits,
                SelectedStrings).ToString();

            BindingObject.LabelText = "NYes";


        }


        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        internal override Grid GetGridSection()
        {
            Grid grid = base.GetGridSection();




            this._Label = new Label
            {
                
            };

            BindingObject = new InputBindObject { LabelText = "Hello" };

            _Label.SetBinding(Label.TextProperty, new Binding("LabelText"));
            _Label.BindingContext = BindingObject;

            
            // row 2
            grid.Children.Add(new Label { Text = "Result" }, 0, 1);

            // row 3
            grid.Children.Add(this._Label, 0, 2);

            return grid;
        }
    }
}
