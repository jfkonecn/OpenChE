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
            Children.ItemAdded += Children_ItemAdded;
            
        }

        private void Children_ItemAdded(object sender, ItemEventArgs<Category<T>> e)
        {
            Search.Execute(null);
        }

        private bool CanSearch(object parameter)
        {
            return Children.Count > 0;
        }

        private void SearchFunction(object parameter)
        {

            SortedSet<CategorySearchResult<T>> temp = new SortedSet<CategorySearchResult<T>>();
            IEnumerable<Category<T>> catResults = 
                from cat in Children
                where cat.Name.ToLower().Contains(SearchKeyword.ToLower())    
                select cat;


            foreach (Category<T> cat in catResults)
            {
                CategorySearchResult<T> catSearchObj = new CategorySearchResult<T>(cat);
                foreach(T item in cat.Children)
                {
                    catSearchObj.ItemResults.Add(new CategoryItemSearchResult<T>(item));
                }
                temp.Add(catSearchObj);
            }

            IEnumerable<T> itemResults = from cat in Children
                                               where !temp.Contains(new CategorySearchResult<T>(cat))
                                               from item in cat.Children
                                               where item.FullName.ToLower().Contains(SearchKeyword.ToLower())
                                               select item;
            foreach (T item in itemResults)
            {
                CategorySearchResult<T> catSearchObj = null;
                if (!temp.Contains(new CategorySearchResult<T>(item.Parent)))
                {
                    catSearchObj = new CategorySearchResult<T>(item.Parent);
                    temp.Add(catSearchObj);
                }
                else
                {
                    catSearchObj = (from obj in temp
                                    where item.Parent.Name == obj.Name
                                    select obj).Single();
                }
                catSearchObj.ItemResults.Add(new CategoryItemSearchResult<T>(item));
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
                if (_SearchKeyword.Equals(value))
                    return;
                _SearchKeyword = value;
                Search.Execute(null);
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


        private SortedSet<CategorySearchResult<T>> _SearchResults = null;
        [XmlIgnore]
        public SortedSet<CategorySearchResult<T>> SearchResults
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
