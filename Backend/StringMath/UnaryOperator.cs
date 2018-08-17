using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StringMath
{
    internal class UnaryOperator : IOperator
    {
        private UnaryOperator(string regularExpression, Func<double, double> evaluator)
        {
            RegularExpression = regularExpression;
            Evaluator = evaluator;
        }

        /// <summary>
        /// i.e. "-" (without quotes) if this is a negative operator
        /// </summary>
        internal readonly string RegularExpression;

        private readonly Func<double, double> Evaluator;

        internal double Evaluate(double num)
        {
            return Evaluator(num);
        }

        public double Evaluate(ref Stack<double> vs)
        {
            if (TotalParameters > vs.Count)
                throw new SyntaxException();
            double num = vs.Pop();
            return Evaluator(num);
        }

        private static readonly ReadOnlyCollection<UnaryOperator> AllOperators =
        new ReadOnlyCollection<UnaryOperator>(new List<UnaryOperator>()
        {
            new UnaryOperator(@"^\s*\+\b",
                (double num)=>{ return num; }),
            new UnaryOperator(@"^\s*[-−]\b",
                (double num)=>{ return -num; })
        });

        public ushort Precedence { get { return 4; } }

        public OperatorAssociativity Associativity { get { return OperatorAssociativity.RightAssociative; } }

        public ushort TotalParameters { get { return 1; } }

        internal static bool TryGetUnaryOperator(ref string equationString,
            IEquationToken previousToken, out IOperator opt)
        {
            if (ValidPreviousOperator(previousToken))
            {
                foreach (UnaryOperator obj in AllOperators)
                {
                    if (HelperFunctions.RegularExpressionParser(obj.RegularExpression, ref equationString))
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
        internal static bool ValidPreviousOperator(IEquationToken previousToken)
        {
            return !BinaryOperator.ValidPreviousOperator(previousToken);
        }
    }
}
