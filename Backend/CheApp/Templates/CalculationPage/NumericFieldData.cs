using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Units;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Stores all of the data required to have a field which handles data inputs
    /// </summary>
    public class NumericFieldData
    {

        /// <summary>
        /// Stores all of the data required to have a field which handles data inputs
        /// </summary>
        /// <param name="bindedObject">Object which is binded to the field</param>
        public NumericFieldData(ref FieldBindData bindedObject)
        {
            if (bindedObject.ConvertionUnits.Length > 2)
            {
                throw new Exception("convertion units out of range");
            }
            this.BindedObject = bindedObject;
        }


        /// <summary>
        /// Object which will bind to the field
        /// </summary>
        public FieldBindData BindedObject { get; set; }


        internal int ID
        {
            get
            {
                return BindedObject.ID;
            }
        }

        /// <summary>
        /// Title of the field
        /// </summary>
        internal string Title
        {
            get
            {
                return BindedObject.Title;
            }
        }

        private Entry _InputEntry;
        /// <summary>
        /// Contains the user entered text
        /// </summary>
        public string EntryText
        {
            get
            {
                return BindedObject.LabelText;
            }
            set
            {
                BindedObject.LabelText = value;
            }
        }

        private Label _ResultsLb;
        private Label _ResultsTitleLb;
        private Label _InputTitleLb;


        /// <summary>
        /// Updates the label with the value of 
        /// </summary>
        /// <param name="finalResult">Result of internal calculation assumed to be in the same units as "resultUnits" specificed in the constructor</param>
        internal void SetFinalResult(double finalResult)
        {

            // WE ARE ASSUMING THAT A MAX OF 2 ELEMENTS WILL BE IN THE ARRAY
            BindedObject.LabelText = String.Format("{0:G4}", EngineeringMath.Units.HelperFunctions.ConvertTo(
                finalResult,
                BindedObject.ConvertionUnits,
                SelectedStrings));


        }

        /// <summary>
        /// gets the user input in the desired units specified in constructor
        /// </summary>
        internal double GetUserInput()
        {
            try
            {
                double temp = EngineeringMath.Units.HelperFunctions.ConvertFrom(
                Convert.ToDouble(EntryText),
                BindedObject.ConvertionUnits,
                SelectedStrings);
                BindedObject.BackgroundColor = Color.LightGreen;
                return temp;
            }
            catch (System.FormatException)
            {
                BindedObject.BackgroundColor = Color.PaleVioletRed;
                throw new FormatException();
            }

            // WE ARE ASSUMING THAT A MAX OF 2 ELEMENTS WILL BE IN THE ARRAY

        }



        /// <summary>
        /// Stores reference to all pickers
        /// </summary>
        internal Picker[] Pickers { private set; get; }

        /// <summary>
        /// creates a pickers for the input field
        /// </summary>
        /// <returns></returns>
        private Picker[] CreatePickers()
        {
            Picker[] allPickers = new Picker[BindedObject.ConvertionUnits.Length];

            for(int i = 0; i < BindedObject.ConvertionUnits.Length; i++)
            {
                // create the picker
                allPickers[i] = new Picker
                {


                };
                foreach (string str in this.ListOfUnitNames(BindedObject.ConvertionUnits[i].GetType()))
                {
                    allPickers[i].Items.Add(str);
                }

                // bind it up!
                allPickers[i].SetBinding(Picker.SelectedIndexProperty, new Binding($"SelectedIndex[{i}]"));
                allPickers[i].BindingContext = BindedObject;
            }

            return allPickers;
        }


        string[] _SelectedStrings;
        /// <summary>
        /// Strings selected by the user at the pickers
        /// </summary>
        protected string[] SelectedStrings
        {
            get
            {
                if(_SelectedStrings == null)
                {
                    _SelectedStrings = new string[Pickers.Length];
                }

                // update the array
                for (int i = 0; i < BindedObject.ConvertionUnits.Length; i++)
                {
                    _SelectedStrings[i] = Pickers[i].Items[BindedObject.SelectedIndex[i]];
                }

                return _SelectedStrings;
            }
            private set
            {
                _SelectedStrings = value;
            }
        }

        private List<string> ListOfUnitNames(Type unitType)
        {
            return new List<string>(StaticUnitProperties.AllUnits[unitType].Keys);
        }

        /// <summary>
        /// Section intended to be placed in a grid
        /// </summary>
        virtual internal Grid GetGridSection()
        {
            // create entry cell
            Label title = new Label
            {
                Text = BindedObject.Title,
                HorizontalTextAlignment = TextAlignment.Center
            };

            
            // create the pickers
            this.Pickers = CreatePickers();

            // create columns
            ColumnDefinitionCollection colDefs = new ColumnDefinitionCollection();


            colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });

            if (this.Pickers.Length == 1)
            {
                // for input/ouput cell
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                // for unit pickers
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
            else
            {
                // for input/ouput cell
                colDefs.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Star) });
                // for unit pickers
                colDefs.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                colDefs.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            }

            colDefs.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Absolute) });

            Grid grid = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(20, GridUnitType.Absolute) }
                },
                ColumnDefinitions = colDefs                
            };


            // row 1
            grid.Children.Add(title, 1, 1);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count - 2);



            Label unitLb = new Label { Text = "Units" };
            grid.Children.Add(unitLb, 2, 2);

            // row 3
            // Entry cell will be taken care of by the classes which inherit this class
            if (this.Pickers.Length == 1)
            {
                grid.Children.Add(this.Pickers[0], 2, 3);
                
            }
            else
            {
                grid.Children.Add(this.Pickers[0], 2, 3);
                grid.Children.Add(new Label
                {
                    Text = "Per",
                    HorizontalTextAlignment = TextAlignment.Center
                }, 3, 3);
                grid.Children.Add(this.Pickers[1], 4, 3);
                Grid.SetColumnSpan(unitLb, 3);
            }


            // create entry cell
            this._InputEntry = new Entry
            {
                Keyboard = Keyboard.Numeric,
                HeightRequest = 10
            };

            this._ResultsLb = new Label
            {

            };

            // bind it up!
            _InputEntry.SetBinding(Entry.TextProperty, new Binding("LabelText"));
            _InputEntry.BindingContext = BindedObject;
            _ResultsLb.SetBinding(Label.TextProperty, new Binding("LabelText"));
            _ResultsLb.BindingContext = BindedObject;

            grid.SetBinding(Grid.BackgroundColorProperty, new Binding("BackgroundColor"));
            grid.BindingContext = BindedObject;

            // row 2
            _InputTitleLb = new Label { Text = "Input" };
            _ResultsTitleLb = new Label { Text = "Result" };

            _ResultsLb.SetBinding(Entry.IsVisibleProperty, new Binding("isOutput"));
            _ResultsLb.BindingContext = BindedObject;

            _ResultsTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("isOutput"));
            _ResultsTitleLb.BindingContext = BindedObject;

            _InputEntry.SetBinding(Entry.IsVisibleProperty, new Binding("isInput"));
            _InputEntry.BindingContext = BindedObject;

            _InputTitleLb.SetBinding(Entry.IsVisibleProperty, new Binding("isInput"));
            _InputTitleLb.BindingContext = BindedObject;

            grid.Children.Add(_InputTitleLb, 1, 2);
            grid.Children.Add(_ResultsTitleLb, 1, 2);

            // row 3
            grid.Children.Add(this._InputEntry, 1, 3);
            grid.Children.Add(this._ResultsLb, 1, 3);

            return grid;
        }







    }
}
