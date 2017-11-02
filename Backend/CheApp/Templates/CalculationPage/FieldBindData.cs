using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CheApp.CheMath.Units;

namespace CheApp.Templates.CalculationPage
{
    /// <summary>
    /// Object which binds with a Numeric Field Data Object
    /// </summary>
    public class FieldBindData : INotifyPropertyChanged
    {


        /// <param name="id">Desired ID number, not used internally</param>
        /// <param name="title">Title of the field</param>
        /// <param name="convertionUnits">Used to create a conversion factor</param>
        public FieldBindData(int id, string title, AbstractUnit[] convertionUnits)
        {
            if (convertionUnits.Length > 2)
            {
                throw new Exception("convertion units out of range");
            }

            this.Title = title;
            this.ConvertionUnits = convertionUnits;
            this.ID = id;
            SelectedIndex = convertionUnits.Select(item => 0).ToArray();
        }

        internal int ID { get; private set; }

        /// <summary>
        /// Title of the field
        /// </summary>
        internal string Title { get; private set; }


        /// <summary>
        /// The unit this field represents
        /// </summary>
        internal AbstractUnit[] ConvertionUnits { get; private set; }

        /// <summary>
        /// The selected index of each picker
        /// </summary>
        public int[] SelectedIndex { get; set; }


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
}
