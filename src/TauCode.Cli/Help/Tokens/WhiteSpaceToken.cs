using TauCode.Parsing;
using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Help.Tokens
{
    // todo: 'INoPayloadToken' in taucode.parsing.
    // todo: 'bool AddNoPayloadTokens' in LexerBase
    public class WhiteSpaceToken : TokenBase
    {
        public WhiteSpaceToken(Position position, int consumedLength)
            : base(position, consumedLength)
        {
        }

        public override string ToString() => $"<space:{this.ConsumedLength}>";
    }
}
