using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Data;
using System.ComponentModel;

namespace EngineeringMath.Component
{
    public class Equation : ISpaceSaver
    {

        /// <summary>
        /// Calculates the result of this equation
        /// </summary>
        /// <returns>Result of the equation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public double Evaluate()
        {
            if (Parameters == null)
            {
                throw new ArgumentNullException(nameof(Parameters));
            }
            if (_EquationTable == null)
            {
                _EquationTable = new DataTable();
            }
            // replace all references to variables with numbers
            string temp = EquationExpression;
            foreach(Parameter para in Parameters)
            {
                temp = temp.Replace(para.Name, ParameterToStringValue(para));
            }
            return double.Parse(_EquationTable.Compute(temp, "").ToString());
        }

        private string ParameterToStringValue(Parameter parameter)
        {
            if (parameter as SIUnitParameter != null)
            {
                return ((SIUnitParameter)parameter).SIValue.ToString();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Nullify()
        {
            Parameters = null;
        }

        [XmlIgnore]
        public ParameterList Parameters { get; set; }
        public string EquationExpression { get; set; }


        [XmlIgnore]
        private static DataTable _EquationTable = null;
    }
}
