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
    public class Equation
    {
        protected Equation()
        {

        }

        public Equation(string equationExpression)
        {
            EquationExpression = equationExpression;
        }

        

        /// <summary>
        /// Calculates the result of this equation
        /// </summary>
        /// <param name="findParameter">Passes the varName found in the equation expression and expects the return of its value</param>
        /// <returns>Result of the equation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public double Evaluate(Func<string, double> findParameter)
        {      
            return Evaluate(EquationExpression, findParameter);
        }


        /// <summary>
        /// Extracts all of the varNames found in the passed equation expression
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>                  
        public static List<string> GetInputList(string equation)
        {
            List<string> list = new List<string>();
            EquationParser.Evaluate(equation, (x) => 
            {
                list.Add(x);
                return double.NaN;
            });
            return list;
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
            return Evaluate(equation, (string name) => { return dic[name]; });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equation"></param>
        /// <param name="getParaValue">Given the name of a parameter the calculation value is returned</param>
        /// <returns></returns>
        private static double Evaluate(string equation, Func<string, double> getParaValue)
        {
            return EquationParser.Evaluate(equation, getParaValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        public bool IsInput(string varName)
        {
            return InputParameterVarNames.Contains(varName);
        }

        [XmlIgnore]
        public List<string> InputParameterVarNames { get; protected set; } = new List<string>();

        private string _EquationExpression = string.Empty;
        public string EquationExpression
        {
            get
            {
                return _EquationExpression;
            }
            set
            {
                if (_EquationExpression.Equals(value))
                    return;
                _EquationExpression = value;
                try
                {
                    InputParameterVarNames = GetInputList(EquationExpression);
                }
                catch
                {
                    InputParameterVarNames = new List<string>();
                }
                
            }
        }

    }
}
