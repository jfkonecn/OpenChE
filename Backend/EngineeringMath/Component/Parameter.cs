using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class Parameter : NotifyPropertyChangedExtension
    {
        public Parameter(string name)
        {
            Name = name;
        }
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
    }
}
