using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    interface ISIParameterUnitCategory : ICategory
    {
        double ConverterToSIUnit(string curUnitFullName, double curValue);

        double ConverterFromSIUnit(string desiredUnitFullName, double curValue);

        NotifyPropertySortedList<Unit, UnitCategory> Children { get; }
    }
}
