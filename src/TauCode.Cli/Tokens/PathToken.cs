using TauCode.Parsing.Tokens;

namespace TauCode.Cli.Tokens
{
    public class PathToken : TokenBase
    {
        internal PathToken(string value)
        {
            // no checks needed, actually.
            this.Value = value;
        }

        public string Value { get; }
    }
}
