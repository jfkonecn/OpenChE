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
                KeyValuePair<string, TValue> obj = GetKeyValuePairAtIndex(index);
                if (value != null)
                    value.Parent = _Parent;
                _List[obj.Key] = value;
                if (obj.Value != null)
                    obj.Value.Parent = null;
            }
        }

        public int Count => _List.Count;

        public bool IsReadOnly => true;

        public void Add(TValue item)
        {
            if (item == null)
                throw new ArgumentNullException();
            item.Parent = _Parent;
            _List.Add(item.ToString(), item);
            OnPropertyChanged(nameof(AllOptions));
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, TValue> obj in _List)
            {
                if(obj.Value != null)
                    obj.Value.Parent = null;
            }
            _List.Clear();
            OnPropertyChanged(nameof(AllOptions));
        }

        public bool Contains(TValue item)
        {
            return _List.ContainsValue(item);
        }


        public bool TryGetValue(string key, out TValue value)
        {
            return _List.TryGetValue(key, out value);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {

            List<KeyValuePair<string, TValue>> temp = new List<KeyValuePair<string, TValue>>();
            foreach(TValue val in array)
            {
                temp.Add(new KeyValuePair<string, TValue>(val.ToString(), val));
            }
            ((ICollection<KeyValuePair<string,TValue>>)_List).CopyTo(temp.ToArray(), arrayIndex);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach(KeyValuePair<string, TValue> keyPair in _List)
            {
                yield return keyPair.Value;
            }
        }

        public int IndexOf(TValue item)
        {
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

            bool IsRemoved = _List.Remove(item.ToString());
            item.Parent = null;
            OnPropertyChanged(nameof(AllOptions));
            return IsRemoved;
        }

        public void RemoveAt(int index)
        {
            KeyValuePair<string, TValue> obj = GetKeyValuePairAtIndex(index);
            obj.Value.Parent = _Parent;
            _List[obj.Key] = obj.Value;
            if (obj.Value != null)
                obj.Value.Parent = null;
            OnPropertyChanged(nameof(AllOptions));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }


        public IList<string> AllOptions
        {
            get
            {
                return _List.Keys;
            }
        }
    }
}
