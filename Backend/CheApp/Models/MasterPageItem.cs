using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CheApp.Models
{
    /// <summary>
    /// Item in menu of MasterDetailPage
    /// </summary>
    public class MasterPageItem
    {
		public MasterPageItem (string title, string icon, Type targetType)
		{
            Title = title;
            Icon = icon;
            TargetType = targetType;
		}
        public string Title { get; private set; }
        /// <summary>
        /// img url
        /// </summary>
        public string Icon { get; private set; }
        public Type TargetType { get; private set; }
    }
}