using System;
using System.Collections.Generic;

namespace StringMath
{
    public static class EquationParser
    {

        public static double Evaluate(string equationString)
        {
            return Evaluate(equationString, (string paraName) => { throw new ArgumentGetterFunctionUndefined(); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equationString"></param>
        /// <param name="varFinder">passes the parameter name and return its value</param>
        /// <returns></returns>
        public static double Evaluate(string equationString, Func<string, double> varFinder)
        {


            if (varFinder == null)
                return Evaluate(equationString);


            Queue<IEquationToken> outputQueue = GetReversePolishNotationQueue(equationString, varFinder);

            Stack<double> numStack = new Stack<double>();
            while(outputQueue.Count > 0)
            {
                IEquationToken curToken = outputQueue.Dequeue();
                if (curToken is IOperator opt)
                {
                    numStack.Push(opt.Evaluate(ref numStack));
                }
                else if(curToken is Number num)
                {
                    numStack.Push(num.Num);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            if (numStack.Count != 1)
                throw new SyntaxException();
            return numStack.Peek();
        }


        /// <summary>
        /// Using Shunting yard algorithm
        /// </summary>
        /// <param name="equationString"></param>
        /// <param name="varFinder"></param>
        /// <returns></returns>
        private static Queue<IEquationToken> GetReversePolishNotationQueue(string equationString, Func<string, double> varFinder)
        {
            /*
             https://en.wikipedia.org/wiki/Shunting-yard_algorithm
             http://wcipeg.com/wiki/Shunting_yard_algorithm#Unary_operators
             http://tutplusplus.blogspot.com/2011/12/c-tutorial-arithmetic-expression.html
             http://tutplusplus.blogspot.com/2010/12/c-tutorial-equation-calculator.html
             */


            Stack<IOperator> operatorStack = new Stack<IOperator>();
            Queue<IEquationToken> outputQueue = new Queue<IEquationToken>();
            IEquationToken previousToken = null;
            Function curFun = null;
            while (equationString.Length > 0)
            {
                int startingLength = equationString.Length;

                if (Number.TryGetNumber(ref equationString, previousToken, varFinder, out Number num))
                {
                    outputQueue.Enqueue(num);
                    previousToken = num;
                }                    

                if (Function.TryGetFunction(ref equationString, previousToken, out curFun))
                {
                    previousToken = curFun;
                    operatorStack.Push(curFun);
                }
                    

                if (HelperFunctions.TryGetOperator(ref equationString, previousToken, out IOperator opt))
                {
                    if (operatorStack.Count > 0)
                    {
                        IOperator topStackOpt = operatorStack.Peek();
                        while (
                            (
                            topStackOpt as Function != null ||
                            topStackOpt.Precedence > opt.Precedence ||
                            (topStackOpt.Precedence == opt.Precedence && opt.Associativity == OperatorAssociativity.LeftAssociative)
                            )
                            && !topStackOpt.Equals(Bracket.LeftBracket))
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                    }
                    previousToken = opt;
                    operatorStack.Push(opt);
                }

                if (Bracket.TryGetBracket(ref equationString, out Bracket bracket))
                {
                    if (bracket.Equals(Bracket.LeftBracket))
                    {
                        operatorStack.Push(bracket);
                    }
                    else if (bracket.Equals(Bracket.RightBracket))
                    {
                        while (!operatorStack.Peek().Equals(Bracket.LeftBracket))
                        {
                            // unbalaced parentheses
                            if (operatorStack.Count < 1)
                                throw new SyntaxException();
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        operatorStack.Pop();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    previousToken = bracket;
                }

                if (Function.IsFunctionArgumentSeparator(ref equationString))
                {
                    curFun.TotalParameters++;
                    previousToken = null;
                }
                    
                HelperFunctions.RegularExpressionParser(@"^\s+$", ref equationString);

                // if we didn't do anything in a loop, then there are unsupported strings
                if (startingLength == equationString.Length)
                    throw new SyntaxException();
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() as Bracket != null)
                    throw new SyntaxException();
                outputQueue.Enqueue(operatorStack.Pop());
            }
            return outputQueue;
        }

    }

    public class SyntaxException : Exception { }

    public class ArgumentGetterFunctionUndefined : Exception { }

    public class ArgumentNotFound: Exception { }
}
