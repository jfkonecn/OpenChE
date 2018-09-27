using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data;
using StringMath;
using EngineeringMath.Component.CustomEventArgs;

namespace EngineeringMath.Component
{
    public class Equation : IChildItem<IParameterContainerLeaf>
    {

        public Equation(IParameterContainerLeaf parameters)
        {
            Parent = parameters;
        }

        

        /// <summary>
        /// Calculates the result of this equation
        /// </summary>
        /// <returns>Result of the equation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public double Evaluate()
        {
            if (Parent == null)
                throw new ArgumentNullException();

            

            return Evalutate(Parent.EquationExpression, (x) => { return ((INumericParameter)Parent.FindParameter(x)).BaseValue; });
        }

        /// <summary>
        /// Standalone evaluate function
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="allParams"></param>
        /// <returns></returns>
        public static double Evaluate(string equation, params KeyValuePair<string, double>[] allParams)
        {
            Dictionary<string, double> dic = new Dictionary<string, double>();
            foreach(KeyValuePair<string, double> keyPair in allParams)
            {
                dic.Add(keyPair.Key, keyPair.Value);
            }
            return Evalutate(equation, (string name) => { return dic[name]; });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="getParaValue">Given the name of a parameter the calculation value is returned</param>
        /// <returns></returns>
        private static double Evalutate(string equation, Func<string, double> getParaValue)
        {

            return EquationParser.Evaluate(equation, getParaValue);
        }



        [XmlIgnore]
        protected IParameterContainerLeaf _Parent;

        public IParameterContainerLeaf Parent
        {
            get
            {
                return _Parent;
            }
            internal set
            {
                IChildItemDefaults.DefaultSetParent(ref _Parent, OnParentChanged, value, Parent_ParentChanged);
            }
        }
        protected virtual void OnParentChanged(ParentChangedEventArgs e)
        {
            ParentChanged?.Invoke(this, e);
        }
        private void Parent_ParentChanged(object sender, ParentChangedEventArgs e)
        {
            OnParentChanged(e);
        }
        public event EventHandler<ParentChangedEventArgs> ParentChanged;
        IParameterContainerLeaf IChildItem<IParameterContainerLeaf>.Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IChildItem<IParameterContainerLeaf>.Key => throw new NotImplementedException();
    }
}
