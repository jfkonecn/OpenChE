using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    /// <summary>
    /// An object which contains a parent
    /// </summary>
    /// <typeparam name="P">Parent type</typeparam>
    public interface IChildItem<P> where P : class
    {
        // based on http://www.thomaslevesque.com/2009/06/12/c-parentchild-relationship-and-xml-serialization/
        P Parent { get; set; }
    }
}
