using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CheApp.ViewModel
{
    public class MasterPageItem : IComparable
    {

        public string Title { get; set; }
        public string IconSource { get; set; }

        public Type TargetType { get; set; }

        public object BindingContext { get; set; }

        public int CompareTo(object obj)
        {
            if(obj is MasterPageItem item)
            {
                return -item.Title.CompareTo(Title);
            }
            return 1;
        }
    }
}
