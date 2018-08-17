using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal class Bracket : IOperator
    {
        private Bracket(string regularExpression) { RegularExpression = regularExpression; }
        public static readonly Bracket LeftBracket = new Bracket(@"^\s*\(");
        public static readonly Bracket RightBracket = new Bracket(@"^\s*\)");
        private readonly string RegularExpression;

        public ushort Precedence { get { return 5; } }

        public OperatorAssociativity Associativity => throw new NotSupportedException();

        public ushort TotalParameters => throw new NotSupportedException();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equationString">Changes only if a this method returns true</param>
        /// <param name="previousOperator"></param>
        /// <param name="opt">null if BinaryOperator cannot be made</param>
        /// <returns>True if object can be created</returns>
        internal static bool TryGetBracket(ref string equationString, out Bracket opt)
        {
            if (HelperFunctions.RegularExpressionParser(LeftBracket.RegularExpression, ref equationString))
            {
                opt = LeftBracket;
                return true;
            }

            if (HelperFunctions.RegularExpressionParser(RightBracket.RegularExpression, ref equationString))
            {
                opt = RightBracket;
                return true;
            }
            opt = null;
            return false;
        }

        public double Evaluate(ref Stack<double> vs)
        {
            throw new NotImplementedException();
        }
    }
}
