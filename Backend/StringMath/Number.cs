using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal class Number : IEquationToken
    {
        private Number(double num)
        {
            Num = num;
        }

        public readonly double Num;

        internal static bool TryGetNumber(ref string equationString,
            IEquationToken previousToken, Func<string,double> varFinder, out Number num)
        {
            if(previousToken == null || previousToken as IOperator != null)
            {
                if (HelperFunctions.RegularExpressionParser(@"^\s*\d+(\.\d+)?", ref equationString, out string matchStr))
                {
                    num = new Number(double.Parse(matchStr));
                    return true;
                }
                else if (HelperFunctions.RegularExpressionParser(@"^\s*\$[\w_]+[\w\d]*", ref equationString, out matchStr))
                {
                    // remove $
                    matchStr = matchStr.Remove(0, 1);
                    num = new Number(varFinder(matchStr));
                    return true;
                }
            }
            num = null;
            return false;
        }
    }
}
