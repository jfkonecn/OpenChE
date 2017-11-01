using System;
using System.Collections.Generic;
using System.Text;
using CheApp.CheMath.Units;
using Xamarin.Forms;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Stores all of the data required to have a field which handles data inputs
    /// </summary>
    abstract public class NumericFieldData
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



            Grid grid = new Grid
            {
                Margin = 20,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = colDefs
            };




            // row 1
            grid.Children.Add(title, 0, 0);
            Grid.SetColumnSpan(title, grid.ColumnDefinitions.Count);



            Label unitLb = new Label { Text = "Units" };
            grid.Children.Add(unitLb, 1, 1);

            // row 3
            // Entry cell will be taken care of by the classes which inherit this class
            if (this.Pickers.Length == 1)
            {
                grid.Children.Add(this.Pickers[0], 1, 2);
                
            }
            else
            {
                grid.Children.Add(this.Pickers[0], 1, 2);
                grid.Children.Add(new Label
                {
                    Text = "Per",
                    HorizontalTextAlignment = TextAlignment.Center
                }, 2, 2);
                grid.Children.Add(this.Pickers[1], 3, 2);
                Grid.SetColumnSpan(unitLb, 3);
            }





            return grid;
        }







    }
}
