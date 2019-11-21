namespace TauCode.Cli.Parsing.Tokens
{
    public class ValueToken : TokenBase
    {
        internal ValueToken(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString() => Value;
    }
}
