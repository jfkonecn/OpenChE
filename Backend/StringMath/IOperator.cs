using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal interface IOperator : IEquationToken
    {
        ushort Precedence { get; }

        ushort TotalParameters { get; }
        OperatorAssociativity Associativity { get; }
        /// <summary>
        /// Assume top of the stack is the right most value is at the top of the stack
        /// </summary>
        /// <param name="vs"></param>
        /// <returns></returns>
        double Evaluate(ref Stack<double> vs);
    }
    //https://en.wikipedia.org/wiki/Operator_associativity
    internal enum OperatorAssociativity
    {
        /// <summary>
        /// operations can be grouped arbitrarily
        /// </summary>
        Associative,
        /// <summary>
        /// operations are grouped from the left
        /// </summary>
        LeftAssociative,
        /// <summary>
        /// operations are grouped from the right
        /// </summary>
        RightAssociative,
        /// <summary>
        /// operations cannot be chained
        /// </summary>
        NonAssociative
    }
}
