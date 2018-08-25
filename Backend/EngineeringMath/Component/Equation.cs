using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data;
using StringMath;

namespace EngineeringMath.Component
{
    public class Equation : IChildItem<IParameterContainerLeaf>
    {

        public Equation(IParameterContainerLeaf parameters)
        {
            ParentObject = parameters;
        }

        /// <summary>
        /// Calculates the result of this equation
        /// </summary>
        /// <returns>Result of the equation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public double Evaluate()
        {
            if (ParentObject == null)
                throw new ArgumentNullException();

            

            return Evalutate(ParentObject.EquationExpression, (x) => { return ParentObject.FindParameter(x).BaseUnitValue; });
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
        private IParameterContainerLeaf ParentObject { get; set; }

        public IParameterContainerLeaf Parent
        {
            get
            {
                return this.ParentObject;
            }
            internal set
            {
                this.ParentObject = value;
            }
        }

        IParameterContainerLeaf IChildItem<IParameterContainerLeaf>.Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IChildItem<IParameterContainerLeaf>.Key => throw new NotImplementedException();
    }
}
