using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StringMath
{
    internal class BinaryOperator : IOperator
    {

        private BinaryOperator(string regularExpression, ushort precedence, 
            OperatorAssociativity associativity, Func<double, double, double> evaluator)
        {
            RegularExpression = regularExpression;
            Associativity = associativity;
            Evaluator = evaluator;
        }

        /// <summary>
        /// i.e. "+" (without quotes) if this is an addition operator
        /// </summary>
        internal readonly string RegularExpression;


        public ushort Precedence { get; }

        public OperatorAssociativity Associativity { get; }

        public ushort TotalParameters { get { return 2; } }

        private readonly Func<double, double, double> Evaluator;


        public double Evaluate(ref Stack<double> vs)
        {
            if (TotalParameters > vs.Count)
                throw new SyntaxException();
            double rightNum = vs.Pop(), leftNum = vs.Pop();
            return Evaluator(leftNum, rightNum);
        }



        // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        // https://en.wikipedia.org/wiki/Order_of_operations
        private static readonly ReadOnlyCollection<BinaryOperator> AllOperators = 
            new ReadOnlyCollection<BinaryOperator>(new List<BinaryOperator>()
            {
                new BinaryOperator(@"^\s*\^", 4, OperatorAssociativity.RightAssociative, 
                    (double left, double right)=>{ return Math.Pow(left, right); }),
                new BinaryOperator(@"^\s*\*", 3, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left * right; }),
                new BinaryOperator(@"^\s*/", 3, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left / right; }),
                new BinaryOperator(@"^\s*\+", 2, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left + right; }),
                new BinaryOperator(@"^\s*-", 2, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left - right; })
            });



        /// <summary>
        /// 
        /// </summary>
        /// <param name="charArr">Changes only if a this method returns true</param>
        /// <param name="previousToken"></param>
        /// <param name="opt">null if BinaryOperator cannot be made</param>
        /// <returns>True if object can be created</returns>
        internal static bool TryGetBinaryOperator(ref string equationString,
            IEquationToken previousToken, out IOperator opt)
        {
            if (ValidPreviousOperator(previousToken))
            {
                foreach (BinaryOperator obj in AllOperators)
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
        /// true if the previous is valid for a binary operator
        /// </summary>
        /// <param name="previousToken"></param>
        /// <returns></returns>
        private static bool ValidPreviousOperator(IEquationToken previousToken)
        {
            if (previousToken == null || 
                previousToken as IOperator == null || 
                (previousToken as IOperator != null && previousToken.Equals(Bracket.RightBracket)))
                return false;

            return true;
        }


    }
}
