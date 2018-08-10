using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;
using System.Text.RegularExpressions;

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

            return Evalutate(ParentObject.EquationExpression, ParentObject.GetBaseUnitValue);
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
            /*
                https://stackoverflow.com/questions/378415/how-do-i-extract-text-that-lies-between-parentheses-round-brackets
                \$             # Escaped parenthesis, means "starts with a '$' character"
                    (          # Parentheses in a regex mean "put (capture) the stuff 
                        #     in between into the Groups array" 
                        [^)]    # Any character that is not a ')' character
                        *       # Zero or more occurrences of the aforementioned "non ')' char"
                    )          # Close the capturing group
                \$             # "Ends with a ')' character"
             */
            Match m = Regex.Match(equation, @"\$([^$]*)\$");

            for (int i = 1; i < m.Groups.Count; i += 2)
            {
                double num = getParaValue(m.Groups[i].Value);
                equation = equation.Replace(m.Groups[i - 1].Value, num.ToString());
            }

            return double.Parse(_EquationTable.Compute(equation, "").ToString());
        }



        [XmlIgnore]
        public IParameterContainerLeaf ParentObject { get; internal set; }

        IParameterContainerLeaf IChildItem<IParameterContainerLeaf>.Parent
        {
            get
            {
                return this.ParentObject;
            }
            set
            {
                this.ParentObject = value;
            }
        }


        [XmlIgnore]
        private readonly static DataTable _EquationTable = new DataTable();

    }
}
