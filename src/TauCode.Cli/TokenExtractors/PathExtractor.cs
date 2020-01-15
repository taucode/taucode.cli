using System;
using TauCode.Parsing;
using TauCode.Parsing.Lexing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.TokenExtractors
{
    public class PathExtractor : TokenExtractorBase<TextToken>
    {
        public PathExtractor()
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
