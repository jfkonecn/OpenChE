using System;
using System.Collections.Generic;
using System.Text;

namespace StringMath
{
    internal class Bracket : IOperator
    {
        private Bracket(string validChars) { ValidChars = validChars; }
        public static readonly Bracket LeftBracket = new Bracket("([{");
        public static readonly Bracket RightBracket = new Bracket(")]}");
        private readonly string ValidChars;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="charArr"></param>
        /// <param name="idx">Changes only if a this method returns true</param>
        /// <param name="previousOperator"></param>
        /// <param name="opt">null if BinaryOperator cannot be made</param>
        /// <returns>True if object can be created</returns>
        internal static bool TryGetBracket(char[] charArr, ref int idx,
            IOperator previousOperator, out Bracket opt)
        {
            foreach(char c in LeftBracket.ValidChars)
            {
                if(HelperFunctions.IsThisOperator(c, charArr, ref idx))
                {
                    opt = LeftBracket;
                    return true;
                }
            }

            foreach (char c in RightBracket.ValidChars)
            {
                if (HelperFunctions.IsThisOperator(c, charArr, ref idx))
                {
                    opt = RightBracket;
                    return true;
                }
            }
            opt = null;
            return false;
        }
    }
}
