using TauCode.Parsing.Lab.Zeta;
using TauCode.Parsing.Lab.Zeta.TokenExtractors;
using TauCode.Parsing.Lexing;

namespace TauCode.Parsing.Lab.Tests
{
    public class StringLexer : LexerBase
    {
        protected override void InitTokenExtractors()
        {
            var stringExtractor = new StringExtractorBase('"', '"', false, new IEscapeSequenceExtractor[] { });
            this.AddTokenExtractor(stringExtractor);
        }
    }
}
