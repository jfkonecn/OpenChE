using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class CategoryCollection<T> : NotifyPropertyChangedExtension, IList<Category<T>>
        where T : ChildItem<Category<T>>, ICategoryItem
    {
        protected CategoryCollection(string name)
        {
            Name = name;
            Children = new NotifyPropertySortedList<Category<T>, CategoryCollection<T>>(this);
            Search = new Command(
            SearchFunction,
            CanSearch
            );
            SearchResults = this;
        }

        private bool CanSearch(object parameter)
        {
            return Children.Count > 0;
        }

        private void SearchFunction(object parameter)
        {
            CategoryCollection<T> temp = new CategoryCollection<T>(Name)
            {

            };

            IEnumerable<Category<T>> results = from cat in Children
                                                       where cat.Name.ToLower().Contains(SearchKeyword.ToLower())
                                                       select cat;

            foreach (Category<T> cat in results)
            {
                temp.Children.Add(cat);
            }

            results = from cat in Children
                                               where !temp.Children.Contains(cat)
                                               from fun in cat.Children
                                               where fun.FullName.ToLower().Contains(SearchKeyword.ToLower())
                                               group fun by fun.Parent into newCat
                                               select new Category<T>(newCat.Key.Name, newCat.Key.IsUserDefined) { Children = new NotifyPropertySortedList<T, Category<T>>(newCat.ToList()) };
            foreach (Category<T> cat in results)
            {

                temp.Children.Add(cat);
            }

            SearchResults = temp;
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            protected set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }


        private string _SearchKeyword = string.Empty;

        public string SearchKeyword
        {
            get
            {
                return _SearchKeyword;
            }
            set
            {
                _SearchKeyword = value;
                OnPropertyChanged();
            }
        }



        private Command _Search;
        /// <summary>
        /// Solves this function 
        /// </summary>
        [XmlIgnore]
        public Command Search
        {
            get
            {
                return _Search;
            }
            protected set
            {
                _Search = value;
                OnPropertyChanged();
            }
        }


        private CategoryCollection<T> _SearchResults = null;
        [XmlIgnore]
        public CategoryCollection<T> SearchResults
        {
            get
            {
                return _SearchResults;
            }
            private set
            {
                _SearchResults = value;
                OnPropertyChanged();
            }
        }

        private NotifyPropertySortedList<Category<T>, CategoryCollection<T>> _Children;
        public NotifyPropertySortedList<Category<T>, CategoryCollection<T>> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged();
            }
        }


        public int Count => Children.Count;

        public bool IsReadOnly => Children.IsReadOnly;

        public Category<T> this[int index] { get => Children[index]; set => Children[index] = value; }

        public bool TryGetValue(string key, out Category<T> value)
        {
            return Children.TryGetValue(key, out value);
        }

        public int IndexOf(Category<T> item)
        {
            return Children.IndexOf(item);
        }

        public void Insert(int index, Category<T> item)
        {
            Children.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Children.RemoveAt(index);
        }

        public void Add(Category<T> item)
        {
            Children.Add(item);
        }

        public void Clear()
        {
            Children.Clear();
        }

        public bool Contains(Category<T> item)
        {
            return Children.Contains(item);
        }

        public void CopyTo(Category<T>[] array, int arrayIndex)
        {
            Children.CopyTo(array, arrayIndex);
        }

        public bool Remove(Category<T> item)
        {
            return Children.Remove(item);
        }

        public IEnumerator<Category<T>> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}
