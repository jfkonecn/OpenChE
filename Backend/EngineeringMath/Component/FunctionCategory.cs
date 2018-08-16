using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionCategory : ChildItem<FunctionCategoryCollection>, ICategory
    {

        protected FunctionCategory() : base()
        {
            Children = new NotifyPropertySortedList<Function, FunctionCategory>(this);
        }

        public FunctionCategory(string name, bool isUserDefined = false) : this()
        {
            Name = name;
            IsUserDefined = isUserDefined;
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


        public bool IsUserDefined { get; set; }


        private NotifyPropertySortedList<Function, FunctionCategory> _Children;
        public NotifyPropertySortedList<Function, FunctionCategory> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
        private FunctionCategoryCollection ParentObject { get; set; }


        public override FunctionCategoryCollection Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }
    }
}
