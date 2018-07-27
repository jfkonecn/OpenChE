using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using EngineeringMath.Component;

namespace BackendTesting
{
    [TestClass]
    public class ComponentEquation
    {
        [TestMethod]
        public void StringTester()
        {
            TestParameterContainer someParameter = new TestParameterContainer();
            Equation equation = new Equation(new WeakReference<IParameterContainer>(someParameter));
        }


        public class TestParameterContainer : IParameterContainer
        {
            public Parameter GetParameter(string name)
            {
                return _Parameters[name];
            }

            private Dictionary<string, Parameter> _Parameters = new Dictionary<string, Parameter>
            {

            };
        }
    }
}
