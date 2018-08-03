﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    // adapted from http://www.thomaslevesque.com/2009/06/12/c-parentchild-relationship-and-xml-serialization/
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">List item type</typeparam>
    public class NotifyPropertySortedList<TKey, TValue, P>: NotifyPropertyChangedExtension,
        IEnumerable<TValue>, ICollection<TValue>, IList<TValue>
        where TKey : class
        where TValue : ISortedListItem<TKey, P>
        where P : class
    {
        private P _Parent;
        private SortedList<TKey, TValue> _List;
        public NotifyPropertySortedList(P parent)
        {
            this._Parent = parent;
            _List = new SortedList<TKey, TValue>();
        }

        public NotifyPropertySortedList(P parent, SortedList<TKey, TValue> list)
        {
            _Parent = parent;
            _List = list;
        }


        /// <summary>
        /// Gets the KeyValuePair of an object in _List given an index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private KeyValuePair<TKey, TValue> GetKeyValuePairAtIndex(int index)
        {
            int i = 0;
            foreach (KeyValuePair<TKey, TValue> val in _List)
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
                KeyValuePair<TKey, TValue> obj = GetKeyValuePairAtIndex(index);
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
            _List.Add(item.Key, item);
        }

        public void Clear()
        {
            foreach (KeyValuePair<TKey, TValue> obj in _List)
            {
                if(obj.Value != null)
                    obj.Value.Parent = null;
            }
            _List.Clear();
        }

        public bool Contains(TValue item)
        {
            return _List.ContainsValue(item);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {

            List<KeyValuePair<TKey, TValue>> temp = new List<KeyValuePair<TKey, TValue>>();
            foreach(TValue val in array)
            {
                temp.Add(new KeyValuePair<TKey, TValue>(val.Key, val));
            }
            ((ICollection<KeyValuePair<TKey,TValue>>)_List).CopyTo(temp.ToArray(), arrayIndex);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach(KeyValuePair<TKey, TValue> keyPair in _List)
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
            throw new NotImplementedException();
        }

        public bool Remove(TValue item)
        {
            if (item == null)
                return false;

            bool IsRemoved = _List.Remove(item.Key);
            item.Parent = null;
            return IsRemoved;
        }

        public void RemoveAt(int index)
        {
            KeyValuePair<TKey, TValue> obj = GetKeyValuePairAtIndex(index);
            obj.Value.Parent = _Parent;
            _List[obj.Key] = obj.Value;
            if (obj.Value != null)
                obj.Value.Parent = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }
    }
}
