﻿using TauCode.Cli.TokenExtractors;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardTokenExtractors;

namespace TauCode.Cli
{
    public class CliLexer : LexerBase
    {
        protected override void InitTokenExtractors()
        {
            // integer
            var integerExtractor = new IntegerExtractor();
            this.AddTokenExtractor(integerExtractor);

            // term
            var termExtractor = new TermExtractor();
            this.AddTokenExtractor(termExtractor);

            // key
            var keyExtractor = new KeyExtractor();
            this.AddTokenExtractor(keyExtractor);

            // string
            var stringExtractor = new StringExtractor();
            this.AddTokenExtractor(stringExtractor);

            // path
            var pathExtractor = new PathExtractor();
            this.AddTokenExtractor(pathExtractor);

            // equals
            var equalsExtractor = new EqualsExtractor();
            this.AddTokenExtractor(equalsExtractor);

            // *** Links ***
            keyExtractor.AddSuccessors(equalsExtractor);

            equalsExtractor.AddSuccessors(
                integerExtractor,
                termExtractor,
                keyExtractor,
                stringExtractor,
                pathExtractor);
        }
    }
}