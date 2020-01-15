using System;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenExtractors
{
    public class EqualsExtractor : TokenExtractorBase<PunctuationToken>
    {
        public EqualsExtractor()
            : base(null)
        {
        }

        protected override CharAcceptanceResult AcceptCharImpl(char c, int localIndex)
        {
            throw new NotImplementedException();
        }

        protected override void OnBeforeProcess()
        {
            throw new NotImplementedException();
        }

        protected override PunctuationToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            throw new NotImplementedException();
        }
    }
}
