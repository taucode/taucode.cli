using System.Collections.Generic;
using TauCode.Cli.TokenExtractors;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Lexing.StandardExtractors;

namespace TauCode.Cli
{
    public class CliLexer : LexerBase
    {
        protected override IList<ITokenExtractor> CreateTokenExtractors()
        {
            return new List<ITokenExtractor>
            {
                new EqualsExtractor(),
                new IntegerExtractor(null), // todo: why 'params'?
                new TermExtractor(),
                new KeyExtractor(),
                new SingleQuoteStringExtractor(),
                new DoubleQuoteStringExtractor(),
                new PathExtractor(),
            };
        }
    }
}
