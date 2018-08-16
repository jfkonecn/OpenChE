using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal static class HelperFunctions
    {

        /// <summary>
        /// Checks if this Operator is present at the current index
        /// </summary>
        /// <param name="operatorRepresentation">The string which represents operator the ie "+" or "sqrt"</param>
        /// <param name="chrArr"></param>
        /// <param name="idx">
        /// Changes the index to the location of the next character which is not apart of this Operator
        /// if this method returns true
        /// </param>
        /// <returns></returns>
        internal static bool IsThisOperator(string operatorRepresentation, char[] chrArr, ref int idx)
        {
            int i = idx;
            // ignore whitespace
            while (chrArr[i] == ' ' && chrArr.Length > i)
                i++;

            foreach (char c in operatorRepresentation)
            {
                if (chrArr.Length == i || chrArr[i] != c)
                    return false;
                i++;
            }
            idx = i;
            return true;
        }

        /// <summary>
        /// Checks if this Operator is present at the current index
        /// </summary>
        /// <param name="operatorRepresentation">The string which represents operator the ie "+" or "sqrt"</param>
        /// <param name="chrArr"></param>
        /// <param name="idx">
        /// Changes the index to the location of the next character which is not apart of this Operator
        /// if this method returns true
        /// </param>
        /// <returns></returns>
        internal static bool IsThisOperator(char operatorRepresentation, char[] chrArr, ref int idx)
        {
            return IsThisOperator($"{operatorRepresentation}", chrArr, ref idx);
        }
    }
}
