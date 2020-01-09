using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.TextProcessing.Lab.Tokens
{
    public sealed class LfToken : TokenBase
    {
        public static LfToken Instance = new LfToken();

        private LfToken()
            : base(Position.Zero, 1)
        {
        }

        public override bool HasPayload => false;
    }
}
