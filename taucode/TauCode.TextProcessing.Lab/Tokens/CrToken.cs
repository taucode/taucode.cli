using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.TextProcessing.Lab.Tokens
{
    public sealed class CrToken : TokenBase
    {
        public static CrToken Instance = new CrToken();

        private CrToken()
            : base(Position.Zero, 1)
        {
        }

        public override bool HasPayload => false;
    }
}
