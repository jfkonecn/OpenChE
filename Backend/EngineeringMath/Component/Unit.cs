using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class Unit : NotifyPropertyChangedExtension
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            private set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }


        public double ConvertToBase()
        {
            return ConvertToBaseEquation.Evaluate();
        }

        public double ConvertFromBase()
        {
            return ConvertFromBaseEquation.Evaluate();
        }

        private Equation ConvertToBaseEquation { get; set; }
        private Equation ConvertFromBaseEquation { get; set; }
    }
}
