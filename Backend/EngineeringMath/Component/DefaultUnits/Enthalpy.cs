using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component.DefaultUnits
{
    public class Enthalpy : UnitCategory
    {
        public Enthalpy() : base(LibraryResources.Enthalpy,
        new UnitCategoryElement[]
        {
            new UnitCategoryElement(new Energy(), 1, false),
            new UnitCategoryElement(new Mass(), -1, false)
        }, false)
        {

        }
    }
}
