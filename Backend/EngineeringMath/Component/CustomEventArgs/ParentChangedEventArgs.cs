using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.CustomEventArgs
{
    public class ParentChangedEventArgs : EventArgs
    {
        public ParentChangedEventArgs(object oldParent, object newParent)
        {
            OldParent = oldParent;
            NewParent = newParent;
        }
        public object OldParent { get; }
        public object NewParent { get; }
    }
}
