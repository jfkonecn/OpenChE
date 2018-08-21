using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class SelectableList<TValue, P> : NotifyPropertySortedList<TValue, P>, ISetting
        where TValue : ChildItem<P>
        where P : class
    {
        public SelectableList(string name, P parent) : base(parent)
        {
            Name = name;
        }

        public SelectableList(string name, P parent, SortedList<string, TValue> list) : base(parent, list)
        {
            Name = name;
        }

        public SelectableList(string name, NotifyPropertySortedList<TValue, P> list) : base(list)
        {
            Name = name;
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
                OnIndexChanged();
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

        public SettingState CurrentState { get; internal set; }

        public string Name { get; protected set; }

        protected void OnIndexChanged()
        {
            IndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler IndexChanged;
    }
}
