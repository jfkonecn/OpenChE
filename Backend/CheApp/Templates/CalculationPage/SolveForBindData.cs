using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace CheApp.Templates.CalculationPage
{
    public class SolveForBindData : INotifyPropertyChanged
    {

        public SolveForBindData()
        {
            SelectedIndex = 0;
        }

        /// <summary>
        /// The selected index of picker
        /// </summary>
        public int SelectedIndex { get; set; }


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
