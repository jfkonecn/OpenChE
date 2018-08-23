using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class CategoryItemSearchResult<T> : NotifyPropertyChangedExtension, IComparable
        where T : ChildItem<Category<T>>, ICategoryItem
    {
        internal CategoryItemSearchResult(T item)
        {
            _Item = item;
        }

        private readonly T _Item;

        public string FullName { get { return _Item.FullName; } }
        /// <summary>
        /// Category Name
        /// </summary>
        public string CatName { get { return _Item.Parent.Name; } }

        public override bool Equals(object obj)
        {
            if (!(obj is CategoryItemSearchResult<T> temp))
                return false;

            return temp.FullName == FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
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

            if (!(obj is CategoryItemSearchResult<T> temp))
                return 1;

            return FullName.CompareTo(temp.FullName);
        }
    }
}
