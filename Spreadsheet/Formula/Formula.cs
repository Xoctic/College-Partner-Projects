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
        private List<Token> tokens;


        struct Token
        {
            public string Text { get; set; }
            public TokenType Type;

            public Token(string _text, TokenType _type)
            {
                Text = _text;
                Type = _type;
            }
        }


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
        public Formula(String formula) : this(formula, x => x, x => true)
        {

        }




        public Formula(string formula, Normalizer norm, Validator valid)
        {
            if (formula == null)
            {
                throw new ArgumentNullException("formula passed in was null");
            }
            else if (norm == null)
            {
                throw new ArgumentNullException("normalizer passed in was null");
            }
            else if (valid == null)
            {
                throw new ArgumentNullException("validator passed in was null");
            }

            tokens = new List<Token>(GetTokens(formula));

            int numOfLeftParen = 0;
            int numOfRightParen = 0;
            Token previousToken = new Token("", TokenType.Invalid);
            Boolean isFirst = true;
            int counter = 0;
            string tempText;

            if (tokens.Count == 0)
            {
                //condition 2: There must be at least one token.
                throw new FormulaFormatException("Less than one token in the expression");
            }
            //condition 5: The first token of a formula must be a number, a variable, or an opening parenthesis.
            else if (tokens[0].Type == TokenType.Oper || tokens[0].Type == TokenType.RParen || tokens[0].Type == TokenType.Invalid)
            {
                throw new FormulaFormatException(tokens[0].Text);
            }

            tokens = new List<Token>();

            foreach (Token token in GetTokens(formula))
            {

                if (isFirst == false)
                {
                    //condition 7: Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
                    if (previousToken.Type == TokenType.LParen || previousToken.Type == TokenType.Oper)
                    {
                        if (token.Type == TokenType.Invalid || token.Type == TokenType.Oper || token.Type == TokenType.RParen)
                        {
                            throw new FormulaFormatException(token.Text);
                        }
                    }
                    //condition 8: Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
                    if (previousToken.Type == TokenType.RParen || previousToken.Type == TokenType.Number || previousToken.Type == TokenType.Var)
                    {
                        if (previousToken.Type == TokenType.Number || previousToken.Type == TokenType.Var || previousToken.Type == TokenType.RParen)
                        {
                            if (token.Type != TokenType.Oper && token.Type != TokenType.RParen)
                            {
                                throw new FormulaFormatException(token.Text);
                            }
                        }
                    }

                }

                //condition 3: When reading tokens from left to right, at no point should the number of closing parentheses seen so far be greater than the number of opening parentheses seen so far.
                if (numOfRightParen > numOfLeftParen)
                {
                    throw new FormulaFormatException("Too many right parentheses");
                }

                if (token.Type == TokenType.LParen)
                {
                    numOfLeftParen++;
                }
                else if (token.Type == TokenType.RParen)
                {
                    numOfRightParen++;
                }
                previousToken = token;

                if (isFirst == true)
                {
                    isFirst = false;
                }


                if (token.Type == TokenType.Var)
                {
                    tempText = norm(token.Text);

                    if (!valid(tempText))
                    {
                        throw new FormulaFormatException("Validation failed");
                    }
                    tokens.Add(new Token(tempText, token.Type));
                }
                else
                {
                    tokens.Add(new Token(token.Text, token.Type));
                }


                counter++;

            }
            //condition 6: The last token of a formula must be a number, a variable, or a closing parenthesis.
            if (tokens[tokens.Count - 1].Type == TokenType.Oper || tokens[tokens.Count - 1].Type == TokenType.LParen || tokens[tokens.Count - 1].Type == TokenType.Invalid)
            {
                throw new FormulaFormatException(tokens[tokens.Count - 1].Text);
            }
            //condition 4: The total number of opening parentheses must equal the total number of closing parentheses.
            if (numOfLeftParen != numOfRightParen)
            {
                throw new FormulaFormatException(tokens[tokens.Count - 1].Text);
            }

        }



        public ISet<string> GetVariables()
        {
            HashSet<string> variables = new HashSet<string>();

            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.Var)
                {
                    variables.Add(token.Text);
                }
            }

            return variables;
        }


        public override string ToString()
        {
            string output = "";

            foreach (Token el in tokens)
            {
                output += el.Text;
            }

            return output;
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
            if (lookup == null)
            {
                throw new ArgumentNullException("lookup passed in was null");
            }


            Stack<string> operators = new Stack<string>();
            Stack<double> values = new Stack<double>();


            foreach (Token t in tokens)
            {
                switch (t.Type)
                {
                    case TokenType.LParen:
                        operators.Push(t.Text);
                        break;
                    case TokenType.RParen:

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

                            val = values.Pop() * Double.Parse(t.Text);

                            values.Push(val);

                            operators.Pop();
                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {
                            double lVal = values.Pop();
                            if (Double.Parse(t.Text) == 0)
                            {
                                throw new FormulaEvaluationException("Division by 0");
                            }
                            double val = lVal / Double.Parse(t.Text);
                            values.Push(val);
                            operators.Pop();
                        }
                        else
                        {
                            values.Push(Double.Parse(t.Text));
                        }
                        break;
                    case TokenType.Oper:
                        if (t.Text == "+" || t.Text == "-")
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
                        operators.Push(t.Text);
                        break;

                    case TokenType.Var:
                        if (operators.Count != 0 && operators.Peek() == "*")
                        {
                            double val;

                            try
                            {
                                val = values.Pop() * lookup(t.Text);
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Text);
                            }


                            values.Push(val);
                            operators.Pop();


                        }
                        else if (operators.Count != 0 && operators.Peek() == "/")
                        {

                            double lVal = values.Pop();

                            try
                            {
                                if (lookup(t.Text) == 0)
                                {
                                    throw new FormulaEvaluationException("Division by 0");
                                }
                                double val = lVal / lookup(t.Text);
                                values.Push(val);
                                operators.Pop();
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Text);
                            }



                        }
                        else
                        {

                            try
                            {
                                values.Push(lookup(t.Text));
                            }
                            catch (UndefinedVariableException e)
                            {

                                throw new FormulaEvaluationException("Undefined variable: " + t.Text);
                            }
                        }

                        break;
                    default:
                        break;
                }
            }


            if (operators.Count == 0)
            {
                return values.Pop();
            }

            if (operators.Count == 1)
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
        private static IEnumerable<Token> GetTokens(String formula)
        {

            //if(formula == null)
            //{
            //    throw new ArgumentNullException("formula passed into GetTokens was null");
            //}

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


                    //Yield the token
                    yield return new Token(match.Value, type);
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
    /// 
    /// Normalizer - convert variables into a canonical form
    /// 
    /// Validator - imposes extra restrictions on the validity of a variable, beyond the ones already built into the Formula definition
    /// </summary>
    public delegate double Lookup(string var);
    public delegate string Normalizer(string s);
    public delegate bool Validator(string s);

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