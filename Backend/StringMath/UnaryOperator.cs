using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StringMath
{
    internal class UnaryOperator : IOperator
    {
        private UnaryOperator(char opt, Func<double, double> evaluator)
        {
            Operator = opt;
            Evaluator = evaluator;
        }

        /// <summary>
        /// i.e. "-" (without quotes) if this is a negative operator
        /// </summary>
        internal readonly char Operator;

        private readonly Func<double, double> Evaluator;

        internal double Evaluate(double num)
        {
            return Evaluator(num);
        }

        private static readonly ReadOnlyCollection<UnaryOperator> AllOperators =
        new ReadOnlyCollection<UnaryOperator>(new List<UnaryOperator>()
        {
            new UnaryOperator('+',
                (double num)=>{ return num; }),
            new UnaryOperator('-',
                (double num)=>{ return -num; })
        });



        internal static bool TryGetUnaryOperator(char[] charArr, ref int idx,
            IEquationToken previousToken, out UnaryOperator opt)
        {
            if (ValidPreviousOperator(previousToken))
            {
                foreach (UnaryOperator obj in AllOperators)
                {
                    if (HelperFunctions.IsThisOperator(obj.Operator, charArr, ref idx))
                    {
                        opt = obj;
                        return true;
                    }
                }
            }
            opt = null;
            return false;
        }

        /// <summary>
        /// true if the previous is valid for a unary operator
        /// </summary>
        /// <param name="previousToken"></param>
        /// <returns></returns>
        private static bool ValidPreviousOperator(IEquationToken previousToken)
        {
            if (previousToken != null ||
                previousToken as IOperator == null ||
                (previousToken as IOperator != null && previousToken.Equals(Bracket.RightBracket)))
                return false;

            return true;
        }
    }
}
