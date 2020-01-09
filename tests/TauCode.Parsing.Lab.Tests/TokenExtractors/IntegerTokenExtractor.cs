using TauCode.Extensions;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;
using TauCode.TextProcessing.Lab;

namespace TauCode.Parsing.Lab.Tests.TokenExtractors
{
    // todo clean up
    public class IntegerTokenExtractor : GammaTokenExtractorBase<IntegerToken>
    {
        public override IntegerToken ProduceToken(string text, int absoluteIndex, int consumedLength, Position position)
        {
            var intString = text.Substring(absoluteIndex, consumedLength);
            if (int.TryParse(intString, out var dummy))
            {
                return new IntegerToken(intString, position, consumedLength);
            }

            return null;
        }

        protected override bool AcceptsPreviousCharImpl(char previousChar) =>
            LexingHelper.IsStandardPunctuationChar(previousChar) ||
            LexingHelper.IsInlineWhiteSpaceOrCaretControl(previousChar);

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            if (localIndex == 0)
            {
                var ok =
                    c.IsIn('+', '-') ||
                    LexingHelper.IsDigit(c);

                return ok ? CharAcceptanceResult.Continue : CharAcceptanceResult.Fail;
            }

            if (LexingHelper.IsDigit(c))
            {
                return CharAcceptanceResult.Continue;
            }

            if (LexingHelper.IsStandardPunctuationChar(c) || LexingHelper.IsInlineWhiteSpaceOrCaretControl(c))
            {
                return CharAcceptanceResult.Stop;
            }

            return CharAcceptanceResult.Fail;
        }
    }
}
