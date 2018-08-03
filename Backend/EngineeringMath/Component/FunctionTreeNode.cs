using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public abstract class FunctionTreeNode : NotifyPropertyChangedExtension
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private Parameter[] _Parameters;

        public Parameter[] Parameters
        {
            get { return _Parameters; }
            set
            {
                _Parameters = value;
                OnPropertyChanged();
            }
        }

    }
}
