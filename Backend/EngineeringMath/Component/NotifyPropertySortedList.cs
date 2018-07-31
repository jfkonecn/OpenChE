using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">List item type</typeparam>
    public class NotifyPropertySortedList<TKey, TValue>: NotifyPropertyChangedExtension,
        IEnumerable<TValue>, ICollection<TValue> where TValue : ISortedListItem<TKey>
    {
        private readonly SortedList<TKey, TValue> _List = new SortedList<TKey, TValue>();

        public int Count => _List.Count;

        public bool IsReadOnly => true;

        public void Add(TValue item)
        {
            _List.Add(item.Key, item);
        }

        public void Clear()
        {
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

        public bool Remove(TValue item)
        {
            return _List.Remove(item.Key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }
    }
}
