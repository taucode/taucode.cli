using System;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenExtractors
{
    public class KeyExtractor : TokenExtractorBase<TextToken>
    {
        public KeyExtractor()
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

        protected override TextToken DeliverToken(string text, int absoluteIndex, Position position, int consumedLength)
        {
            throw new NotImplementedException();
        }
    }
}
