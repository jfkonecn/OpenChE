using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionCategory : ChildItem<FunctionCategoryCollection>, ICategory, IList<Function>
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

        public bool TryGetValue(string key, out Function value)
        {
            return Children.TryGetValue(key, out value);
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

        public int Count => Children.Count;

        public bool IsReadOnly => Children.IsReadOnly;

        public Function this[int index] { get => Children[index]; set => Children[index] = value; }

        public override string ToString()
        {
            return Name;
        }

        public int IndexOf(Function item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, Function item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void Add(Function item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(Function item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(Function[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(Function item)
        {
            return Children.Remove(item);
        }

        public IEnumerator<Function> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}
