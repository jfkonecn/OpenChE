using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class SelectableList<TValue, P> : NotifyPropertySortedList<TValue, P>, ISetting
        where TValue : IChildItem<P>
        where P : class
    {
        public SelectableList(P parent) : base(parent)
        {

        }

        public SelectableList(P parent, SortedList<string, TValue> list) : base(parent, list)
        {

        }

        public SelectableList(NotifyPropertySortedList<TValue, P> list) : base(list)
        {

        }

        private int _SelectedIndex = 0;
        public int SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                OnPropertyChanged(nameof(ItemAtSelectedIndex));
                OnPropertyChanged();
            }
        }


        public TValue ItemAtSelectedIndex
        {
            get
            {
                return this[this.SelectedIndex];
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].Equals(value))
                    {
                        SelectedIndex = i;
                        return;
                    }                        
                }
                throw new ArgumentException(value.ToString());
            }
        }
    }
}
