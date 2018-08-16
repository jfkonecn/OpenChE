using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal class Number : IEquationToken
    {
        internal static bool TryGetNumber(char[] charArr, ref int idx,
            IEquationToken previousToken, out Number num)
        {
            throw new NotImplementedException();
        }
    }
}
