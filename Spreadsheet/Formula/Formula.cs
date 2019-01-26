// Skeleton written by Joe Zachary for CS 3500, January 2019

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static Formulas.TokenType;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        private List<Tuple<string, TokenType>> tokens;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        ///     
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// item1 is a string which shows what the token actually is such as +-*/ ect
        /// item2 is the tokenType enums such as oper, Lparen, rightParen, Ivalid ect...
        /// </summary>
        public Formula(String formula)
        {
            tokens = new List<Tuple<string, TokenType>>(GetTokens(formula));

            int numOfLeftParen = 0;
            int numOfRightParen = 0;
            Tuple<string, TokenType> previousToken = new Tuple<string, TokenType>("", TokenType.Invalid);
            Boolean isFirst = true;


            if (tokens.Count == 0)
            {
                //condition 2
                throw new FormulaFormatException("Less than one token in the expression");
            }
            //condition 5
            else if (tokens[0].Item2 == TokenType.Oper || tokens[0].Item2 == TokenType.RParen || tokens[0].Item2 == TokenType.Invalid)
            {
                throw new FormulaFormatException(tokens[0].Item1);
            }


            foreach (Tuple<string, TokenType> token in tokens)
            {

                if (isFirst == false)
                {
                    //condition 7
                    if (previousToken.Item2 == TokenType.LParen || previousToken.Item2 == TokenType.Oper)
                    {
                        if (token.Item2 == TokenType.Invalid || token.Item2 == TokenType.Oper || token.Item2 == TokenType.RParen)
                        {
                            throw new FormulaFormatException(token.Item1);
                        }
                    }
                    //condition 8
                    if (previousToken.Item2 == TokenType.RParen || previousToken.Item2 == TokenType.Number || previousToken.Item2 == TokenType.Var)
                    {
                        if (previousToken.Item2 == TokenType.Number || previousToken.Item2 == TokenType.Var || previousToken.Item2 == TokenType.RParen)
                        {
                            if (token.Item2 != TokenType.Oper && token.Item2 != TokenType.RParen)
                            {
                                throw new FormulaFormatException(token.Item1);
                            }
                        }
                    }



                }




                //condition 3
                if (numOfRightParen > numOfLeftParen)
                {
                    throw new FormulaFormatException("Too many right parentheses");
                }

                if (token.Item2 == TokenType.LParen)
                {
                    numOfLeftParen++;
                }
                else if (token.Item2 == TokenType.RParen)
                {
                    numOfRightParen++;
                }
                previousToken = token;

                if (isFirst == true)
                {
                    isFirst = false;
                }

            }
            //condition 6
            if (tokens[tokens.Count - 1].Item2 == TokenType.Oper || tokens[tokens.Count - 1].Item2 == TokenType.LParen || tokens[tokens.Count - 1].Item2 == TokenType.Invalid)
            {
                throw new FormulaFormatException(tokens[tokens.Count - 1].Item1);
            }
            //condition 4
            if (numOfLeftParen != numOfRightParen)
            {
                throw new FormulaFormatException(tokens[tokens.Count - 1].Item1);
            }

        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack<string> operators = new Stack<string>();
            Stack<double> values = new Stack<double>();


            foreach (Tuple<string, TokenType> t in tokens)
            {
                switch (t.Item2)
                {
                    case TokenType.LParen:
                        operators.Push(t.Item1);
                        break;
                    case TokenType.RParen:
                        
                        if (operators.Count != 0  && operators.Peek() == "+")
                        {
                            double val;

                            val = values.Pop() + values.Pop();

                            values.Push(val);

                            operators.Pop();
                        }
                        else if (operators.Count != 0 && operators.Peek() == "-")
                        {
                            double rVal = values.Pop();
                            double lVal = values.Pop();
                            double val = lVal - rVal;
                            values.Push(val);
                            operators.Pop();
                        }


                        operators.Pop();

                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            double val;

                            val = values.Pop() * values.Pop();

                            values.Push(val);

                            operators.Pop();
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            double rVal = values.Pop();
                            double lVal = values.Pop();
                            if (rVal == 0)
                            {
                                throw new FormulaEvaluationException("Division by 0");
                            }
                            double val = lVal / rVal;
                            values.Push(val);
                            operators.Pop();
                        }
                        break;
                    case TokenType.Number:
                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            double val;

                            val = values.Pop() * Double.Parse(t.Item1);

                            values.Push(val);

                            operators.Pop();
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            double lVal = values.Pop();
                            if (Double.Parse(t.Item1) == 0)
                            {
                                throw new FormulaEvaluationException("Division by 0");
                            }
                            double val = lVal / Double.Parse(t.Item1);
                            values.Push(val);
                            operators.Pop();
                        }
                        else
                        {
                            values.Push(Double.Parse(t.Item1));
                        }
                        break;
                    case TokenType.Oper:
                        if (t.Item1 == "+" || t.Item1 == "-")
                        {
                            if (operators.Count != 0 && operators.Peek() == "+")
                            {
                                double val;

                                val = values.Pop() + values.Pop();

                                values.Push(val);

                                operators.Pop();
                            }
                            else if (operators.Count != 0 && operators.Peek() == "-")
                            {
                                double rVal = values.Pop();
                                double lVal = values.Pop();
                                double val = lVal - rVal;
                                values.Push(val);
                                operators.Pop();
                            }
                        }
                        operators.Push(t.Item1);
                        break;

                    case TokenType.Var:
                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            double val;

                            try
                            {
                                val = values.Pop() * lookup(t.Item1);
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Item1);
                            }


                            values.Push(val);
                            operators.Pop();


                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {

                            double lVal = values.Pop();

                            try
                            {
                                if (lookup(t.Item1) == 0)
                                {
                                    throw new FormulaEvaluationException("Division by 0");
                                }
                                double val = lVal / lookup(t.Item1);
                                values.Push(val);
                                operators.Pop();
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Item1);
                            }



                        }
                        else
                        {

                            try
                            {
                                values.Push(lookup(t.Item1));
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Item1);
                            }
                        }

                        break;
                    default:
                        break;
                }
            }


            if(operators.Count == 0)
            {
                return values.Pop();
            }

            if(operators.Count == 1)
            {
                double rVal = values.Pop();
                double lVal = values.Pop();
                if (operators.Count != 0 && operators.Peek() == "+")
                {
                    double val;

                    val = rVal + lVal;


                    operators.Pop();
                    return val;
                }
                else if (operators.Count != 0 && operators.Peek() == "-")
                {
                    
                    double val = lVal - rVal;
                   
                    operators.Pop();

                    return val;
                }

            }

            return 0;
            
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Each token is described by a
        /// Tuple containing the token's text and TokenType.  There are no empty tokens, and no
        /// token contains white space.
        /// </summary>
        private static IEnumerable<Tuple<string, TokenType>> GetTokens(String formula)
        {
            // Patterns for individual tokens.
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";

            // NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall token pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String tokenPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5}) | (.)",
                                            spacePattern, lpPattern, rpPattern, opPattern, varPattern, doublePattern);

            // Create a Regex for matching tokens.  Notice the second parameter to Split says 
            // to ignore embedded white space in the pattern.
            Regex r = new Regex(tokenPattern, RegexOptions.IgnorePatternWhitespace);

            // Look for the first match
            Match match = r.Match(formula);

            // Start enumerating tokens
            while (match.Success)
            {
                // Ignore spaces
                if (!match.Groups[1].Success)
                {
                    // Holds the token's type
                    TokenType type;

                    if (match.Groups[2].Success)
                    {
                        type = LParen;
                    }
                    else if (match.Groups[3].Success)
                    {
                        type = RParen;
                    }
                    else if (match.Groups[4].Success)
                    {
                        type = Oper;
                    }
                    else if (match.Groups[5].Success)
                    {
                        type = Var;
                    }
                    else if (match.Groups[6].Success)
                    {
                        type = Number;
                    }
                    else if (match.Groups[7].Success)
                    {
                        type = Invalid;
                        throw new FormulaFormatException(type + "");
                    }
                    else
                    {
                        // We shouldn't get here
                        throw new InvalidOperationException("Regular exception failed in GetTokens");
                    }

                    // Yield the token
                    yield return new Tuple<string, TokenType>(match.Value, type);
                }

                // Look for the next match
                match = match.NextMatch();
            }
        }
    }

    /// <summary>
    /// Identifies the type of a token.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Left parenthesis
        /// </summary>
        LParen,

        /// <summary>
        /// Right parenthesis
        /// </summary>
        RParen,

        /// <summary>
        /// Operator symbol
        /// </summary>
        Oper,

        /// <summary>
        /// Variable
        /// </summary>
        Var,

        /// <summary>
        /// Double literal
        /// </summary>
        Number,

        /// <summary>
        /// Invalid token
        /// </summary>
        Invalid
    };

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
            Console.WriteLine("Formula Format Exception: {0}", message);
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
            Console.WriteLine("Formula Evaluation Exception: {0}", message);
        }
    }
}
