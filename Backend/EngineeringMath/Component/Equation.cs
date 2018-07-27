using System;
using System.Collections.Generic;
using System.Text;

namespace EngineeringMath.Component
{
    public class Equation
    {
        public Equation(WeakReference<IParameterContainer> parameters)
        {
            Parameters = parameters;
        }

        private WeakReference<IParameterContainer> Parameters { get; set; }

        private string EquationStr { get; set; }
    }
}
