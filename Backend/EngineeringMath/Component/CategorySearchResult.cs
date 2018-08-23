using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class CategorySearchResult<T> : NotifyPropertyChangedExtension, IComparable, IEnumerable
        where T : ChildItem<Category<T>>, ICategoryItem
    {
        public CategorySearchResult(Category<T> cat)
        {
            _Cat = cat;
            ItemResults = new SortedSet<CategoryItemSearchResult<T>>();
        }
        private readonly Category<T> _Cat;
        public string Name { get => _Cat.Name; }
        public SortedSet<CategoryItemSearchResult<T>> ItemResults { get; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public CategoryItemSearchResult<T> this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override bool Equals(object obj)
        {
            if (!(obj is CategorySearchResult<T> temp))
                return false;

            return temp.Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            /*Less than zero
            This instance precedes value in the sort order.
            Zero
            This instance occurs in the same position as value in the sort order.
            Greater than zero
            This instance follows value in the sort order.
            -or-
            value is null.*/

            if (!(obj is CategorySearchResult<T> temp))
                return 1;

            return Name.CompareTo(temp.Name);

        }


        public IEnumerator<CategoryItemSearchResult<T>> GetEnumerator()
        {
            return ItemResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ItemResults.GetEnumerator();
        }
    }
}
