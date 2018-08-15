using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public interface ICategory
    {
        string Name { get; set; }
        bool IsUserDefined { get; set; }
    }
}
