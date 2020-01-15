using TauCode.Parsing.Lexing.StandardExtractors;
using TauCode.Parsing.TextDecorations;

namespace TauCode.Cli.TokenExtractors
{
    public class SingleQuoteStringExtractor : StringExtractorBase
    {
        public SingleQuoteStringExtractor() 
            : base(
                '\'',
                '\'',
                false,
                SingleQuoteTextDecoration.Instance,
                null)
        {
        }
    }
}
