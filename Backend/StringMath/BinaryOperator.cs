using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StringMath
{
    internal class BinaryOperator : IOperator
    {

        private BinaryOperator(char opt, UInt16 precedence, 
            OperatorAssociativity associativity, Func<double, double, double> evaluator)
        {
            Operator = opt;
            Associativity = associativity;
            Evaluator = evaluator;
        }

        /// <summary>
        /// i.e. "+" (without quotes) if this is an addition operator
        /// </summary>
        internal readonly char Operator;

        internal readonly UInt16 Precedence;

        internal readonly OperatorAssociativity Associativity;

        private readonly Func<double, double, double> Evaluator;


        internal double Evaluate(double leftNum, double rightNum)
        {
            return Evaluator(leftNum, rightNum);
        }


        //https://en.wikipedia.org/wiki/Operator_associativity
        internal enum OperatorAssociativity
        {
            /// <summary>
            /// operations can be grouped arbitrarily
            /// </summary>
            Associative,
            /// <summary>
            /// operations are grouped from the left
            /// </summary>
            LeftAssociative,
            /// <summary>
            /// operations are grouped from the right
            /// </summary>
            RightAssociative,
            /// <summary>
            /// operations cannot be chained
            /// </summary>
            NonAssociative
        }
        // https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        private static readonly ReadOnlyCollection<BinaryOperator> AllOperators = 
            new ReadOnlyCollection<BinaryOperator>(new List<BinaryOperator>()
            {
                new BinaryOperator('^', 4, OperatorAssociativity.RightAssociative, 
                    (double left, double right)=>{ return Math.Pow(left, right); }),
                new BinaryOperator('*', 3, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left * right; }),
                new BinaryOperator('/', 3, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left / right; }),
                new BinaryOperator('+', 2, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left + right; }),
                new BinaryOperator('-', 2, OperatorAssociativity.LeftAssociative,
                    (double left, double right)=>{ return left - right; })
            });



        /// <summary>
        /// 
        /// </summary>
        /// <param name="charArr"></param>
        /// <param name="idx">Changes only if a this method returns true</param>
        /// <param name="previousToken"></param>
        /// <param name="opt">null if BinaryOperator cannot be made</param>
        /// <returns>True if object can be created</returns>
        internal static bool TryGetBinaryOperator(char[] charArr, ref int idx,
            IEquationToken previousToken, out BinaryOperator opt)
        {
            if (ValidPreviousOperator(previousToken))
            {
                foreach (BinaryOperator obj in AllOperators)
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
