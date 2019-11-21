using System;
using System.Collections.Generic;
using System.Linq;
using TauCode.Cli.Building;
using TauCode.Cli.Parsing.Tokens;

namespace TauCode.Cli.Parsing
{
    public class Parser
    {
        #region Fields

        private int _pos;
        private string _input;

        #endregion

        #region Private

        private int GetCurrPos()
        {
            return _pos;
        }

        private char GetCurrChar()
        {
            return _input[_pos];
        }

        private char GetChar(int pos)
        {
            return _input[pos];
        }

        private bool IsEnd()
        {
            return _pos == _input.Length;
        }

        private void MoveNext()
        {
            _pos++;
        }

        private bool IsWhiteSpace(char c)
        {
            return char.IsWhiteSpace(c);
        }

        private bool IsQuote(char c)
        {
            return c == '\'' || c == '"';
        }

        private ValueToken ReadValueToken()
        {
            var start = this.GetCurrPos();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrChar();

                if (this.IsWhiteSpace(c))
                {
                    break;
                }
                else if (this.IsQuote(c))
                {
                    throw new ParsingException("Unexpected opening quote.");
                }
                else
                {
                    this.MoveNext();
                }
            }

            var end = this.GetCurrPos();
            var length = end - start;
            var value = _input.Substring(start, length);
            var token = new ValueToken(value);
            return token;
        }

        private ParameterToken ReadParameterToken()
        {
            var start = this.GetCurrPos();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrChar();

                if (this.IsWhiteSpace(c))
                {
                    break;
                }
                else if (this.IsQuote(c))
                {
                    throw new ParsingException("Unexpected opening quote.");
                }
                else if (RootSyntaxBuilder.IsValidParameterChar(c))
                {
                    this.MoveNext();
                    continue;
                }
                else
                {
                    throw new ParsingException("Unexpected character.");
                }
            }
            var end = this.GetCurrPos();
            var length = end - start;
            var value = _input.Substring(start, length);
            var token = new ParameterToken(value);
            return token;
        }

        private QuotedStringToken ReadQuotedStringToken()
        {
            var start = this.GetCurrPos();
            var quoteSymbol = this.GetCurrChar();

            this.MoveNext(); // skip opening quote

            while (true)
            {
                if (this.IsEnd())
                {
                    throw new ParsingException("Non-closed quote.");
                }

                var c = this.GetCurrChar();

                if (c == quoteSymbol)
                {
                    this.MoveNext();
                    break;
                }
                else
                {
                    this.MoveNext();
                }
            }

            var end = this.GetCurrPos();

            var length = end - start;
            var value = _input.Substring(start, length);
            var token = new QuotedStringToken(value);
            return token;
        }

        #endregion

        #region Public

        public List<TokenBase> ParseClause(string input)
        {
            _pos = 0;
            _input = input ?? throw new ArgumentNullException(nameof(input));

            var tokens = new List<TokenBase>();

            while (true)
            {
                if (this.IsEnd())
                {
                    break;
                }

                var c = this.GetCurrChar();
                if (this.IsWhiteSpace(c))
                {
                    this.MoveNext();
                    continue;
                }

                if (c == '-')
                {
                    var token = this.ReadParameterToken();
                    tokens.Add(token);
                }
                else if (this.IsQuote(c))
                {
                    var currPos = this.GetCurrPos();
                    if (currPos > 0)
                    {
                        var prevChar = this.GetChar(currPos - 1);
                        if (!this.IsWhiteSpace(prevChar))
                        {
                            throw new ParsingException("No space before opening quote.");
                        }
                    }

                    var token = this.ReadQuotedStringToken();
                    tokens.Add(token);
                }
                else
                {
                    var token = this.ReadValueToken();
                    tokens.Add(token);
                }
            }

            return tokens;
        }

        public TokenBase ParseSingleToken(string tokenText)
        {
            var tokens = this.ParseClause(tokenText); // will check against null.
            if (tokens.Count != 1)
            {
                throw new ParsingException($"Expected exactly one token, but '{tokens.Count}' were parsed.\r\n{tokenText}");
            }

            return tokens.Single();
        }

        #endregion
    }
}
