﻿using TauCode.Cli.TokenProducers;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardProducers;

namespace TauCode.Cli
{
    public class CliLexer : LexerBase
    {
        public CliLexer()
        {   
        }

        protected override ITokenProducer[] CreateProducers()
        {
            return new ITokenProducer[]
            {
                new WhiteSpaceProducer(),
                new IntegerProducer(IsAcceptableIntegerTerminator),
                new TermProducer(),
                new KeyProducer(),
                new CliSingleQuoteStringProducer(),
                new CliDoubleQuoteStringProducer(),
                new UrlProducer(),
                new PathProducer(),
            };
        }

        private bool IsAcceptableIntegerTerminator(char c)
        {
            return LexingHelper.IsInlineWhiteSpaceOrCaretControl(c);
        }
    }
}
