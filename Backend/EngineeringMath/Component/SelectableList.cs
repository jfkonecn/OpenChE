using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath.Component
{
    public class SelectableList<TValue, P> : NotifyPropertySortedList<TValue, P>, ISetting
        where TValue : class, IChildItem<P>, ISettingOption
        where P : class
    {
        public SelectableList(string name, P parent) : base(parent)
        {
            Name = name;
            ItemRemoved += ListChanged;
            ItemsCleared += ListChanged;
            ItemAdded += ListChanged;
        }

        private void ListChanged(object sender, ItemEventArgs<TValue> e)
        {
            ListChanged();
        }
        private void ListChanged(object sender, ItemEventArgs<IList<TValue>> list)
        {
            ListChanged();
        }
        private void ListChanged()
        {
            OnPropertyChanged(nameof(AllOptions));
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
                OnPropertyChanged(nameof(SelectOptionStr));
                OnPropertyChanged();                
            }
        }


        public TValue ItemAtSelectedIndex
        {
            get
            {
                if(SelectedIndex < 0 || SelectedIndex >= this.Count)
                {
                    return null;
                }
                return this[this.SelectedIndex];
            }
            set
            {
                if (SelectedIndex < 0 || SelectedIndex >= this.Count)
                {
                    return;
                }
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
        private SettingState _CurrentState;
        public SettingState CurrentState
        {
            get
            {
                return _CurrentState;
            }
            internal set
            {
                if (value == _CurrentState)
                    return;
                _CurrentState = value;
                OnPropertyChanged();
            }
        }

        public string Name { get; protected set; }

        protected void OnIndexChanged()
        {
            IndexChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler IndexChanged;

        public IList<string> AllOptions
        {
            get
            {
                List<string> temp = new List<string>();
                foreach (TValue item in this)
                {
                    temp.Add(item.Name);
                }
                return temp;
            }
        }

        public string SelectOptionStr => ItemAtSelectedIndex == null ? string.Empty : ItemAtSelectedIndex.Name;
    }
}
