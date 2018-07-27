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
    public class Equation
    {

        /// <summary>
        /// Calculates the result of this equation
        /// </summary>
        /// <returns>Result of the equation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public double Evaluate()
        {
            if(Parameters == null)
            {
                throw new ArgumentNullException(nameof(Parameters));
            }
            return (double)_EquationTable.Compute($"Sum({ nameof(EquationExpression) })", "");
        }

        /// <summary>
        /// Removes all rows and columns from _EquationTable then 
        /// </summary>
        private void RebuildEquationTable()
        {
            _EquationTable.Reset();
            if (Parameters == null)
            {
                throw new ArgumentNullException(nameof(Parameters));
            }
            // add columns
            foreach (Parameter para in Parameters)
            {
                _EquationTable.Columns.Add(para.Name, typeof(double));
                para.PropertyChanged += Para_PropertyChanged;
            }
            _EquationTable.Columns.Add(nameof(EquationExpression), typeof(double));
            // add exactly one row
            DataRow equationRow = _EquationTable.NewRow();
            foreach (Parameter para in Parameters)
            {
                equationRow[para.Name] = ParameterToDoubleValue(para);
            }
            List<double> strList = Parameters.ConvertAll<double>(ParameterToDoubleValue);    
            
            _EquationTable.Rows.Add(equationRow);
            _EquationTable.Columns[nameof(EquationExpression)].Expression = EquationExpression;
        }

        private double ParameterToDoubleValue(Parameter parameter)
        {
            if(parameter as SIUnitParameter != null)
            {
                return ((SIUnitParameter)parameter).SIValue;
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private void Para_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(sender as SIUnitParameter != null && e.PropertyName.Equals(nameof(SIUnitParameter.SIValue)))
            {
                _EquationTable.Rows[0][((SIUnitParameter)sender).Name] = ((SIUnitParameter)sender).SIValue;
            }
        }

        [XmlIgnore]
        private DataTable _EquationTable = new DataTable();

        private ParameterList _Parameters;
        [XmlIgnore]
        public ParameterList Parameters
        {
            get
            {
                return _Parameters;
            }
            set
            {
                _Parameters = value;
                RebuildEquationTable();
            }
        }

        private string _EquationExpression;
        public string EquationExpression
        {
            get
            {
                return _EquationExpression;
            }
            set
            {
                _EquationExpression = value;
                if(Parameters != null)
                {
                    RebuildEquationTable();
                }
            }
        }
    }
}
