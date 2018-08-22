using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EngineeringMath.Component
{
    public class FunctionCategory : Category<Function>
    {
        protected FunctionCategory() : base()
        {

        }

        public FunctionCategory(string name, bool isUserDefined = false) : base(name, isUserDefined)
        {

        }

    }
}
