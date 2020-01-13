using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TextDecorations;

namespace TauCode.Cli.TokenExtractors
{
    public class StringExtractor : StringExtractorBase
    {
        public StringExtractor() 
            : base(
                '"',
                '"',
                false,
                DoubleQuoteTextDecoration.Instance,
                null)
        {
        }
    }
}
