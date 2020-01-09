using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.TextProcessing.Lab.Tokens
{
    public sealed class CrLfToken : TokenBase
    {
        public static CrLfToken Instance { get; } = new CrLfToken();

        private CrLfToken()
            : base(Position.Zero, 2)
        {
        }

        public override bool HasPayload => false;
    }
}
