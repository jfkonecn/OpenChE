using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public abstract class Parameter : NotifyPropertyChangedExtension, ISortedListItem<string, IParameterContainerNode>
    {
        protected Parameter()
        {

        }
        public Parameter(string name)
        {
            Name = name;
        }
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

        /// <summary>
        /// Used to get value from this parameter for functions
        /// </summary>
        public abstract double BaseUnitValue { get; }


        [XmlIgnore]
        public IParameterContainerNode ParentObject { get; internal set; }



        IParameterContainerNode IChildItem<IParameterContainerNode>.Parent
        {
            get
            {
                return this.ParentObject;
            }
            set
            {
                this.ParentObject = value;
            }
        }

        public string Key
        {
            get
            {
                return Name;
            }
        }
    }
}
