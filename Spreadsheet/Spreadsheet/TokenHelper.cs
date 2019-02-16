using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static SS.TokenType;
using static SS.FormulaError;


namespace SS
{


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
        name,

        /// <summary>
        /// Double literal
        /// </summary>
        Number,

        /// <summary>
        /// Invalid token
        /// </summary>
        Invalid
    };


    class TokenHelper
    {

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
        /// Given a formula, enumerates the tokens that compose it.  Each token is described by a
        /// Tuple containing the token's text and TokenType.  There are no empty tokens, and no
        /// token contains white space.
        /// </summary>
        private static IEnumerable<Token> GetTokens(String formula)
        {

            if (formula == null)
            {
                throw new ArgumentNullException("formula passed into GetTokens was null");
            }

            // Patterns for individual tokens.
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String namePattern = @"[a-zA-Z]*[0-9][0-9]*";
          

            // NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall token pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String tokenPattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5}) | (.)",
                                            spacePattern, lpPattern, rpPattern, opPattern, namePattern, doublePattern);

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
                        type = name;
                    }
                    else if (match.Groups[6].Success)
                    {
                        type = Number;
                    }
                    else if (match.Groups[7].Success)
                    {
                        type = Invalid;
                        
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




}

