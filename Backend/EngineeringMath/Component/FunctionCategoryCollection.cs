using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class FunctionCategoryCollection : NotifyPropertyChangedExtension, ICategoryCollection
    {
        protected FunctionCategoryCollection()
        {
            Children = new NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection>(this);
        }

        private NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection> _Children;
        public NotifyPropertySortedList<FunctionCategory, FunctionCategoryCollection> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }
    }
}
