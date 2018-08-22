using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    // adapted from http://www.thomaslevesque.com/2009/06/12/c-parentchild-relationship-and-xml-serialization/
    /// <summary>
    /// Creates list of objects sorted by their ToString() method
    /// </summary>
    /// <typeparam name="TValue">List item type</typeparam>
    public class NotifyPropertySortedList<TValue, P>: NotifyPropertyChangedExtension,
        IEnumerable<TValue>, ICollection<TValue>, IList<TValue>
        where TValue : ChildItem<P>
        where P : class
    {
        private readonly P _Parent;
        protected SortedList<string, TValue> _List;

        private TValue _TopValue = null;
        /// <summary>
        /// Forces a value to the top of this list
        /// </summary>
        public TValue TopValue
        {
            get
            {
                return _TopValue;
            }
            set
            {
                if (TopValue.Equals(value))
                    return;
                foreach(KeyValuePair<string, TValue> pair in _List)
                {
                    if (pair.Value.Equals(value))
                    {
                        _List.Remove(pair.Key);
                        break;
                    }
                }

                TopValue = value;
                if(TopValue == null)
                {
                    OnItemRemoved(value);
                }
                else
                {
                    OnItemAdded(value);
                }
                OnPropertyChanged();
            }
        }


        public NotifyPropertySortedList(P parent)
        {
            this._Parent = parent;
            _List = new SortedList<string, TValue>();
        }

        public NotifyPropertySortedList(P parent, SortedList<string, TValue> list)
        {
            _Parent = parent;
            _List = list;
        }

        public NotifyPropertySortedList(List<TValue> list)
        {
            _Parent = null;
            _List = new SortedList<string, TValue>();
            AddRange(list);
        }

        /// <summary>
        /// Used to pointer share with another existing list
        /// </summary>
        /// <param name="list"></param>
        protected NotifyPropertySortedList(NotifyPropertySortedList<TValue, P> list)
        {
            _Parent = list._Parent;
            _List = list._List;
        }


        /// <summary>
        /// Gets the KeyValuePair of an object in _List given an index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private KeyValuePair<string, TValue> GetKeyValuePairAtIndex(int index)
        {
            if(index < 0 || index > _List.Count)
                throw new IndexOutOfRangeException();

            if (TopValue != null)
                index--;

            if (index == -1)
                return new KeyValuePair<string, TValue>(TopValue.ToString(), TopValue);

            int i = 0;
            foreach (KeyValuePair<string, TValue> val in _List)
            {
                if (i == index)
                {
                    return val;
                }
                i++;
            }
            throw new IndexOutOfRangeException();
        }

        public TValue this[int index] {
            get
            {
                return GetKeyValuePairAtIndex(index).Value;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                KeyValuePair<string, TValue> obj = GetKeyValuePairAtIndex(index);
                
                value.Parent = _Parent;

                if(index == 0 && TopValue != null)
                {
                    TopValue = value;
                }
                else
                {
                    _List[obj.Key] = value;
                }

                
                if (obj.Value != null)
                    obj.Value.Parent = null;
            }
        }

        public int Count
        {
            get
            {
                if(TopValue == null)
                    return _List.Count;
                return _List.Count + 1;
            }
        } 

        public bool IsReadOnly => true;

        public void Add(TValue item)
        {
            if (item == null)
                throw new ArgumentNullException();
            item.Parent = _Parent;
            _List.Add(item.ToString(), item);
            OnItemAdded(item);
            OnPropertyChanged(nameof(AllOptions));
        }

        public void AddRange(IEnumerable<TValue> c)
        {
            foreach(TValue val in c)
            {
                Add(val);
            }
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, TValue> obj in _List)
            {
                if(obj.Value != null)
                    obj.Value.Parent = null;
            }
            TopValue = null;
            OnItemsCleared(_List.Values);
            _List.Clear();
            OnPropertyChanged(nameof(AllOptions));
        }

        public bool Contains(TValue item)
        {
            if (TopValue != null && TopValue.Equals(item))
                return true;

            return _List.ContainsValue(item);
        }


        public bool TryGetValue(string key, out TValue value)
        {
            if (TopValue != null && TopValue.ToString().Equals(key))
            {
                value = TopValue;
                return true;
            }
            else
            {
                return _List.TryGetValue(key, out value);
            }              
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {

            List<KeyValuePair<string, TValue>> temp = new List<KeyValuePair<string, TValue>>();
            if (TopValue != null)
                temp.Add(new KeyValuePair<string, TValue>(TopValue.ToString(), TopValue));
            foreach (TValue val in array)
            {
                temp.Add(new KeyValuePair<string, TValue>(val.ToString(), val));
            }
            ((ICollection<KeyValuePair<string,TValue>>)_List).CopyTo(temp.ToArray(), arrayIndex);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (TopValue != null)
                yield return TopValue;
            foreach (KeyValuePair<string, TValue> keyPair in _List)
            {
                yield return keyPair.Value;
            }
        }

        public int IndexOf(TValue item)
        {
            if (TopValue != null && TopValue.Equals(item))
            {
                return 0;
            }
            return _List.IndexOfValue(item);
        }

        public void Insert(int index, TValue item)
        {
            throw new NotSupportedException();
        }

        public bool Remove(TValue item)
        {
            if (item == null)
                return false;

            if (TopValue != null && TopValue.Equals(item))
            {
                TopValue = null;
                return false;
            }


            bool IsRemoved = _List.Remove(item.ToString());
            item.Parent = null;
            OnPropertyChanged(nameof(AllOptions));
            OnItemRemoved(item);
            return IsRemoved;
        }

        public void RemoveAt(int index)
        {
            if (TopValue != null && index == 0)
            {
                TopValue = null;
                return;
            }

            KeyValuePair<string, TValue> obj = GetKeyValuePairAtIndex(index);
            obj.Value.Parent = _Parent;
            _List[obj.Key] = obj.Value;
            if (obj.Value != null)
                obj.Value.Parent = null;
            OnItemRemoved(obj.Value);
            OnPropertyChanged(nameof(AllOptions));
        }



        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public IList<string> AllOptions
        {
            get
            {
                if (TopValue != null)
                {
                    List<string> temp = new List<string>() { TopValue.ToString() };
                    temp.AddRange(_List.Keys);
                    return temp;
                }
                return _List.Keys;
            }
        }

        protected void OnItemAdded(TValue item)
        {
            ItemAdded?.Invoke(this, new ItemEventArgs<TValue>(item));
        }
        public event EventHandler<ItemEventArgs<TValue>> ItemAdded;

        protected void OnItemRemoved(TValue item)
        {
            ItemRemoved?.Invoke(this, new ItemEventArgs<TValue>(item));
        }
        public event EventHandler<ItemEventArgs<TValue>> ItemRemoved;

        protected void OnItemsCleared(IList<TValue> list)
        {
            ItemsCleared?.Invoke(this, new ItemEventArgs<IList<TValue>>(list));
        }
        public event EventHandler<ItemEventArgs<IList<TValue>>> ItemsCleared;

    }
}
