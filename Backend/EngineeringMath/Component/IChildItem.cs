using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// An object which contains a parent
    /// </summary>
    /// <typeparam name="P">Parent type</typeparam>
    public interface IChildItem<P> : IChildItemEvent where P : class
    {
        // based on http://www.thomaslevesque.com/2009/06/12/c-parentchild-relationship-and-xml-serialization/
        P Parent { get; set; }
        string Key { get; }

    }

    /// <summary>
    /// NEVER USE THIS use IChildItem<P>
    /// </summary>
    /// <typeparam name="P"></typeparam>
    public interface IChildItemEvent
    {
        event EventHandler<EventArgs> ParentChanged;      
    }

    public static class IChildItemDefaults
    {
        public static void DefaultSetParent<P>(ref P parentObject, Action OnParentChanged, P newParent)
            where P : class
        {
            if (newParent.Equals(parentObject))
                return;




            parentObject = newParent;
            OnParentChanged();
        }

        public static void DefaultSetParent<P>(ref P parentObject, Action OnParentChanged, 
            P newParent, EventHandler<EventArgs> Parent_ParentChanged)
             where P : IChildItemEvent
        {
            if (newParent.Equals(parentObject))
                return;


            if (parentObject != null)
            {
                parentObject.ParentChanged -= Parent_ParentChanged;
            }
            if (newParent != null)
            {
                newParent.ParentChanged += Parent_ParentChanged;
            }

            parentObject = newParent;
            OnParentChanged();
        }
    }
    
}
