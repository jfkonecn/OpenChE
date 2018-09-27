using EngineeringMath.Component.CustomEventArgs;
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
        event EventHandler<ParentChangedEventArgs> ParentChanged;      
    }

    public static class IChildItemDefaults
    {
        public static void DefaultSetParent<P>(ref P parentObject, Action<ParentChangedEventArgs> OnParentChanged, P newParent)
            where P : class
        {
            if (newParent.Equals(parentObject))
                return;



            P oldParent = parentObject;
            parentObject = newParent;
            OnParentChanged(new ParentChangedEventArgs(oldParent, newParent));
        }

        public static void DefaultSetParent<P>(ref P parentObject, Action<ParentChangedEventArgs> OnParentChanged, 
            P newParent, EventHandler<ParentChangedEventArgs> Parent_ParentChanged)
             where P : class, IChildItemEvent
        {
            if ((newParent == null && parentObject == null) || newParent.Equals(parentObject))
                return;

            P oldParent = parentObject;
            if (parentObject != null)
            {
                parentObject.ParentChanged -= Parent_ParentChanged;
            }
            if (newParent != null)
            {
                newParent.ParentChanged += Parent_ParentChanged;
            }

            parentObject = newParent;
            OnParentChanged(new ParentChangedEventArgs(oldParent, newParent));
        }
    }
    
}
