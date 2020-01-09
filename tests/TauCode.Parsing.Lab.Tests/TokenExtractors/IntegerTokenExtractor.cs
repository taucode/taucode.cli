using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.TextProcessing.Lab;

namespace TauCode.Parsing.Lab.Tests.TokenExtractors
{
    public class IntegerTokenExtractor : GammaTokenExtractorBase<IntegerToken>
    {
        public override IntegerToken ProduceToken(string text, int startingIndex, int length)
        {
            throw new System.NotImplementedException();
        }

        protected override bool AcceptsPreviousCharImpl(char previousChar)
        {
            throw new System.NotImplementedException();
        }

        protected override bool AcceptsCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                return
                    c.IsIn('+', '-') ||
                    LexingHelper.IsDigit(c);
            }

            return LexingHelper.IsDigit(c);
        }
    }
}
