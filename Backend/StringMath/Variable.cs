using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal class Variable : IEquationToken
    {
        internal static bool TryGetFunction(char[] charArr, ref int idx,
            IEquationToken previousToken, ref Func<double>GetVariable, out Function variable)
        {
            throw new NotImplementedException();
        }
    }
}
