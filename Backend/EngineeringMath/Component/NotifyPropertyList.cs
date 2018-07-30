using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class NotifyPropertyList<T>: NotifyPropertyChangedExtension, IEnumerable<T>, ICollection<T>
    {
        private readonly List<T> _List = new List<T>();

        public int Count => _List.Count;

        public bool IsReadOnly => true;

        public void Add(T item)
        {
            _List.Add(item);
        }

        public void Clear()
        {
            _List.Clear();
        }

        public bool Contains(T item)
        {
            return _List.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return _List.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _List.GetEnumerator();
        }
    }
}
