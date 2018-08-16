using System;
using System.Collections.Generic;

namespace StringMath
{
    public static class EquationParser
    {

        public static double Parse(string equation)
        {
            return Parse(equation, () => { throw new ArgumentGetterFunctionUndefined(); });
        }

        public static double Parse(string equation, Func<string> argumentGetter)
        {
            /*
             https://en.wikipedia.org/wiki/Shunting-yard_algorithm
             http://wcipeg.com/wiki/Shunting_yard_algorithm#Unary_operators
             http://tutplusplus.blogspot.com/2011/12/c-tutorial-arithmetic-expression.html
             http://tutplusplus.blogspot.com/2010/12/c-tutorial-equation-calculator.html
             */
            Stack<IOperator> operatorStack = new Stack<IOperator>();
            Queue<IEquationToken> equationTokens = new Queue<IEquationToken>();
            IOperator previousOperator = null;

            if(operatorStack.Count < 1)
            {

            }


            if ()
            {

            }

        }

    }

    public class SyntaxException : Exception { }

    public class ArgumentGetterFunctionUndefined : Exception { }

    public class ArgumentNotFound: Exception { }
}
