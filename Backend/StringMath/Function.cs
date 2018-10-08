using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace StringMath
{
    internal class Function : IOperator
    {

        private Function(string methodName)
        {
            if(methodName.Equals("Sqrt"))
            {
                Func<double, double> fun = Math.Sqrt;
                MethodInfo = fun.Method;
            }
            else
            {
                MethodInfo = typeof(Math).GetMethod(methodName, new Type[] { typeof(double) });
                if(MethodInfo == null)
                    MethodInfo = typeof(Math).GetMethod(methodName);
            }


            if (MethodInfo == null)
                throw new SyntaxException();
        }

        public ushort TotalParameters { get; internal set; } = 1;


        private readonly MethodInfo MethodInfo;

        public ushort Precedence { get { return ushort.MaxValue; } }

        public OperatorAssociativity Associativity { get { return OperatorAssociativity.LeftAssociative; } }

        internal static bool TryGetFunction(ref string equationString,
            IEquationToken previousToken,
            out Function fun)
        {
            if (previousToken == null || previousToken as IOperator != null)
            {
                if (HelperFunctions.RegularExpressionParser(@"^\s*[\w_]+[\w\d]*\(", ref equationString, out string matchStr))
                {
                    // '(' should be the last character
                    fun = new Function(Regex.Replace(matchStr.Remove(matchStr.Length - 1), @"\s+", ""));
                    equationString = $"({ equationString }";                    
                    return true;
                }
            }
            fun = null;
            return false;
        }
        internal static bool IsFunctionArgumentSeparator(ref string equationString)
        {
            return HelperFunctions.RegularExpressionParser(@"^\s*,", ref equationString);
        }

        public double Evaluate(ref Stack<double> vs)
        {
            if (TotalParameters > vs.Count)
                throw new SyntaxException();

            Stack<object> nums = new Stack<object>();
            for(int i = 0; i < TotalParameters; i++)
            {
                nums.Push(vs.Pop());
            }

            try
            {
                return (double)MethodInfo.Invoke(null, nums.ToArray());
            }
            catch
            {
                throw new SyntaxException();
            }            
        }
    }
}
