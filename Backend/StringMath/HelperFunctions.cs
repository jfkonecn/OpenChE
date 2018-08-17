using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StringMath
{
    internal static class HelperFunctions
    {

        /// <summary>
        /// Checks if this Operator is present at the current index
        /// Removes the matching string
        /// </summary>
        /// <param name="regExpression">The string which represents operator the ie "+" or "sqrt"</param>
        /// <param name="equationString">Changed if match occurs</param>
        /// <returns></returns>
        internal static bool RegularExpressionParser(string regExpression, ref string equationString)
        {
            return RegularExpressionParser(regExpression, ref equationString, out string temp);
        }

        /// <summary>
        /// Checks if this Operator is present at the current index
        /// Removes the matching string
        /// </summary>
        /// <param name="regExpression">The string which represents operator the ie "+" or "sqrt"</param>
        /// <param name="equationString">Changed if match occurs</param>
        /// <param name="matchingString"></param>
        /// <returns></returns>
        internal static bool RegularExpressionParser(string regExpression, ref string equationString, out string matchingString)
        {
            Regex reg = new Regex(regExpression);
            if (!reg.IsMatch(equationString))
            {
                matchingString = string.Empty;
                return false;
            }
            matchingString = reg.Match(equationString).Value;
            equationString = reg.Replace(equationString, string.Empty);
            return true;
        }

        /// <summary>
        /// Does not include functions or brackets!!!
        /// </summary>
        /// <param name="equationString"></param>
        /// <param name="previousToken"></param>
        /// <param name="opt"></param>
        /// <returns></returns>
        internal static bool TryGetOperator(ref string equationString, IEquationToken previousToken, out IOperator opt)
        {
            return UnaryOperator.TryGetUnaryOperator(ref equationString, previousToken, out opt) ||
                BinaryOperator.TryGetBinaryOperator(ref equationString, previousToken, out opt);
        }
    }
}
