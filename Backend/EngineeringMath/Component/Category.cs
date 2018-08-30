using EngineeringMath.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class Category<T> : NotifyPropertyChangedExtension, IChildItem<CategoryCollection<T>>, IList<T>
        where T : class, IChildItem<Category<T>>, ICategoryItem
    {
        protected Category() : base()
        {
            Children = new NotifyPropertySortedList<T, Category<T>>(this);
            FinishUp();
        }

        public Category(string name, bool isUserDefined = false)
        {
            Children = new NotifyPropertySortedList<T, Category<T>>(this);
            LibraryResourceName = isUserDefined ? string.Empty : name;
            Name = name;
            FinishUp();
        }

        private void FinishUp()
        {
            if(!LibraryResourceName.Equals(string.Empty))
                Name = (string)typeof(LibraryResources).GetProperty(LibraryResourceName).GetValue(null, null);
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


        public bool IsUserDefined { get { return LibraryResourceName.Equals(string.Empty); } }
        /// <summary>
        /// If the full name is a reference to LibraryResources then this string will equal the property name
        /// </summary>
        internal string LibraryResourceName { get; } = string.Empty;


        private NotifyPropertySortedList<T, Category<T>> _Children;
        public NotifyPropertySortedList<T, Category<T>> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }


        public T GetItemByFullName(string itemFullName)
        {
            return (from item in Children
                    where item.FullName.Equals(itemFullName)
                    select item).Single();
        }

        public bool TryGetValue(string key, out T value)
        {
            return Children.TryGetValue(key, out value);
        }

        [XmlIgnore]
        private CategoryCollection<T> ParentObject { get; set; }


        public CategoryCollection<T> Parent
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

        CategoryCollection<T> IChildItem<CategoryCollection<T>>.Parent { get => Parent; set => Parent = value; }
        string IChildItem<CategoryCollection<T>>.Key => Name;

        public T this[int index] { get => Children[index]; set => Children[index] = value; }

        public override string ToString()
        {
            return Name;
        }

        public int IndexOf(T item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void Add(T item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(T item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Children.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}
