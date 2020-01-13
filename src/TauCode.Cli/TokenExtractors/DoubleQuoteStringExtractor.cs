using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TextDecorations;

namespace TauCode.Cli.TokenExtractors
{
    public class DoubleQuoteStringExtractor : StringExtractorBase
    {
        public DoubleQuoteStringExtractor() 
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
